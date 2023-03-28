using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IInputController))]
[RequireComponent(typeof(IInputSettings))]
[RequireComponent(typeof(ILog))]
[RequireComponent(typeof(ISceneSerializer))]
public class SceneController : MonoBehaviour, ISceneController
{
    private float CameraMoveSpeed { get { return 15.0f; } }
    private float CameraRotateSpeed { get { return 10.0f; } }
    private string SceneSerializationPath { get { return System.IO.Path.Combine(Application.persistentDataPath, "scene.xml"); } }


    public Camera mainCamera = null;
    public GameObject[] prefabs = null;

    private IInputController[] inputControllers = null;
    private ICameraController cameraController = null;
    private IInputSettings inputSettings = null;
    private ILog log = null;
    private ISceneSerializer sceneSerializer = null;
    private readonly Plane landscapePlane = new Plane(Vector3.up, 0.0f);
    private int sceneObjectsLayerMask = 0;
    private readonly string sceneObjectsLayerName = "Scene Objects";
    private MovableObject selectedObject = null;

    private Dictionary<SceneObjectType, GameObject> prototypes = new Dictionary<SceneObjectType, GameObject>();
    private Dictionary<ISceneObject, GameObject> createdObjects = new Dictionary<ISceneObject, GameObject>();

    public ISceneObject CreateObject(SceneObjectType objectType, Vector2 position)
    {
        if (!prototypes.ContainsKey(objectType))
        {
            log.Error("No prototype for type: " + objectType);
            return null;
        }
        var prototype = prototypes[objectType];
        var newObject = Instantiate(prototype, transform);
        newObject.name = prototype.name;
        var movableObject = newObject.GetComponent<MovableObject>();
        if (movableObject == null)
        {
            log.Error("Type is not MovableObject: " + objectType);
            return null;
        }
        var sceneObject = movableObject as ISceneObject;
        if (sceneObject == null)
        {
            log.Error("Type is not ISceneObject: " + sceneObject);
            return null;
        }
        movableObject.Position = position;
        createdObjects.Add(sceneObject, newObject);
        return movableObject;
    }

    public void DestroyObject(ISceneObject sceneObject)
    {
        if (!createdObjects.TryGetValue(sceneObject, out GameObject obj))
        {
            log.Warning("Object not found!");
            return;
        }
        createdObjects.Remove(sceneObject);
        Destroy(obj);
    }

    public IEnumerable<ISceneObject> Objects
    {
        get
        {
            return createdObjects.Keys;
        }
    }

    public SceneObjectType CurrentType { get; set; }

    public void ClearScene()
    {
        foreach (var kvp in createdObjects)
            Destroy(kvp.Value);
        createdObjects.Clear();
    }

    public void SaveScene()
    {
        sceneSerializer?.Save(SceneSerializationPath);
    }

    public void LoadScene()
    {
        sceneSerializer?.Load(SceneSerializationPath);
    }

    private void OnCameraMove(CameraMoveDirection cameraMoveDirection)
    {
        if (cameraController == null)
            return;
        var moveDelta = CameraMoveSpeed * Time.deltaTime;
        if (inputSettings != null)
            moveDelta *= inputSettings.CameraMoveSpeedMultiplier;
        switch (cameraMoveDirection)
        {
            case CameraMoveDirection.Forward:
                cameraController.MoveForward(moveDelta);
                break;
            case CameraMoveDirection.Back:
                cameraController.MoveBack(moveDelta);
                break;
            case CameraMoveDirection.Right:
                cameraController.MoveRight(moveDelta);
                break;
            case CameraMoveDirection.Left:
                cameraController.MoveLeft(moveDelta);
                break;
            default:
                break;
        }
    }

    private void OnCameraRotate(Vector3 delta)
    {
        if (cameraController == null)
            return;
        var rotateDelta = CameraRotateSpeed * delta * Time.deltaTime;
        if (inputSettings != null)
            rotateDelta *= inputSettings.MouseSensitivityMultiplier;
        cameraController.Pitch(-rotateDelta.y);
        cameraController.Yaw(rotateDelta.x);
    }

    private void OnObjectMoveKeyDown(Vector3 cursorPosition)
    {
        if (cameraController == null)
            return;
        var ray = cameraController.ScreenPointToRay(cursorPosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, sceneObjectsLayerMask) && (hitInfo.collider != null))
            selectedObject = hitInfo.collider.gameObject.GetComponentInParent<MovableObject>();
    }

    private void OnObjectMove(Vector3 cursorPosition)
    {
        if (selectedObject == null)
            return;
        if (cameraController == null)
            return;
        var ray = cameraController.ScreenPointToRay(cursorPosition);
        if (!landscapePlane.Raycast(ray, out float distance))
            return;
        var worldPosition = ray.GetPoint(distance);
        selectedObject.Position = new Vector2(worldPosition.x, worldPosition.z);
    }

    private void OnObjectMoveKeyUp(Vector3 cursorPosition)
    {
        selectedObject = null;
    }

    private void CreateObjectUnderCursor(Vector3 cursorPosition)
    {
        if (cameraController == null)
            return;
        var ray = cameraController.ScreenPointToRay(cursorPosition);
        if (!landscapePlane.Raycast(ray, out float distance))
            return;
        var worldPosition = ray.GetPoint(distance);
        CreateObject(CurrentType, new Vector2(worldPosition.x, worldPosition.z));
    }

    private void DestroyObjectUnderCursor(Vector3 cursorPosition)
    {
        if (cameraController == null)
            return;
        var ray = cameraController.ScreenPointToRay(cursorPosition);
        if (Physics.Raycast(ray,out RaycastHit hitInfo, float.MaxValue, sceneObjectsLayerMask) && (hitInfo.collider != null))
        {
            var sceneObject = hitInfo.collider.gameObject.GetComponentInParent<ISceneObject>();
            if (sceneObject != null)
                DestroyObject(sceneObject); 
        }
    }

    private void InitPrototypes()
    {
        foreach (var obj in prefabs)
        {
            if (obj == null)
            {
                log.Warning("null object in objectPrefabs");
                continue;
            }
            var sceneObject = obj.GetComponent<ISceneObject>();
            if (sceneObject == null)
            {
                log.Warning("non-ISceneObject object in objectPrefabs");
                continue;
            }
            if (prototypes.ContainsKey(sceneObject.ObjectType))
            {
                log.Warning("Duplicate scene object type in objectPrefabs: " + sceneObject.ObjectType);
                continue;
            }
            prototypes.Add(sceneObject.ObjectType, obj);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        sceneObjectsLayerMask = LayerMask.GetMask(new string[] { sceneObjectsLayerName });

        log = GetComponent<ILog>();
        inputControllers = GetComponents<IInputController>();
        if ((inputControllers == null) || (inputControllers.Length <= 0))
            log.Error("IInputController not found");
        inputSettings = GetComponent<IInputSettings>();
        if (inputControllers == null)
            log.Error("IInputSettings not found");
        cameraController = mainCamera?.GetComponent<ICameraController>();
        if (cameraController == null)
            log.Error("ICameraController not found");
        sceneSerializer = GetComponent<ISceneSerializer>();
        if (sceneSerializer == null)
            log.Error("ISceneSerializer not found");

        foreach (var inputController in inputControllers)
        {
            inputController.OnCameraMove += OnCameraMove;
            inputController.OnCameraRotate += OnCameraRotate;
            inputController.OnCreateButtonClick += CreateObjectUnderCursor;
            inputController.OnDestroyButtonClick += DestroyObjectUnderCursor;
            inputController.OnObjectMoveKeyDown += OnObjectMoveKeyDown;
            inputController.OnObjectMove += OnObjectMove;
            inputController.OnObjectMoveKeyUp += OnObjectMoveKeyUp;
        }

        InitPrototypes();
    }

    private void OnDestroy()
    {
        foreach (var inputController in inputControllers)
        {
            inputController.OnCameraMove -= OnCameraMove;
            inputController.OnCameraRotate -= OnCameraRotate;
            inputController.OnCreateButtonClick -= CreateObjectUnderCursor;
            inputController.OnDestroyButtonClick -= DestroyObjectUnderCursor;
            inputController.OnObjectMoveKeyDown -= OnObjectMoveKeyDown;
            inputController.OnObjectMove -= OnObjectMove;
            inputController.OnObjectMoveKeyUp -= OnObjectMoveKeyUp;
        }
    }
}

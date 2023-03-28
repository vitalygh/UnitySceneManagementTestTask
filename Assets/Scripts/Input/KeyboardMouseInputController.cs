using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyboardMouseInputController : MonoBehaviour, IInputController
{
    private Vector3 rotateStartCursorPosition = Vector3.zero;
    private bool objectMoveStarted = false;
    private readonly Dictionary<KeyCode, CameraMoveDirection> moveKeys = new Dictionary<KeyCode, CameraMoveDirection>
    {
       { KeyCode.W, CameraMoveDirection.Forward },
       { KeyCode.S, CameraMoveDirection.Back },
       { KeyCode.D, CameraMoveDirection.Right },
       { KeyCode.A, CameraMoveDirection.Left },
    };

    public Vector3 CursorPosition => Input.mousePosition;

    public UnityAction<CameraMoveDirection> OnCameraMoveKeyDown { get; set; }
    public UnityAction<CameraMoveDirection> OnCameraMove { get; set; }
    public UnityAction<CameraMoveDirection> OnCameraMoveKeyUp { get; set; }

    public bool IsCameraMove
    { 
        get
        {
            foreach (var kvp in moveKeys)
                if (Input.GetKey(kvp.Key))
                    return true;
            return false;
        }
    }

    public UnityAction OnCameraRotateKeyDown { get; set; }
    public UnityAction<Vector3> OnCameraRotate { get; set; }
    public UnityAction OnCameraRotateKeyUp { get; set; }

    public bool IsCameraRotate => Input.GetMouseButton(1);

    public UnityAction<Vector3> OnCreateButtonClick { get; set; }
    public UnityAction<Vector3> OnDestroyButtonClick { get; set; }

    public UnityAction<Vector3> OnObjectMoveKeyDown { get; set; }
    public UnityAction<Vector3> OnObjectMove { get; set; }
    public UnityAction<Vector3> OnObjectMoveKeyUp { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var kvp in moveKeys)
        {
            if (Input.GetKeyDown(kvp.Key))
                OnCameraMoveKeyDown?.Invoke(kvp.Value);
            if (Input.GetKey(kvp.Key))
                OnCameraMove?.Invoke(kvp.Value);
            if (Input.GetKeyUp(kvp.Key))
                OnCameraMoveKeyUp?.Invoke(kvp.Value);
        }

        if (Input.GetMouseButtonDown(1))
        {
            rotateStartCursorPosition = CursorPosition;
            OnCameraRotateKeyDown?.Invoke();
        }

        if (Input.GetMouseButton(1))
        {
            var delta = CursorPosition - rotateStartCursorPosition;
            if (delta.magnitude > Mathf.Epsilon)
            {
                rotateStartCursorPosition = CursorPosition;
                OnCameraRotate?.Invoke(delta);
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            OnCameraRotateKeyUp?.Invoke();
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                OnCreateButtonClick?.Invoke(CursorPosition);
            else if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
                OnDestroyButtonClick?.Invoke(CursorPosition);
            else
            {
                objectMoveStarted = true;
                OnObjectMoveKeyDown?.Invoke(CursorPosition);
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (objectMoveStarted)
                OnObjectMove?.Invoke(CursorPosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (objectMoveStarted)
            {
                OnObjectMoveKeyUp?.Invoke(CursorPosition);
                objectMoveStarted = false;
            }
        }
    }
}

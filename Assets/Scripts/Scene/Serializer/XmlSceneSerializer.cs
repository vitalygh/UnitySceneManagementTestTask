using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class XmlSceneSerializer : MonoBehaviour, ISceneSerializer
{
    private ISceneController sceneController = null;
    private IInputSettings inputSettings = null;
    private ILog log = null;

    public void Load(string path)
    {
        if (!File.Exists(path))
        {
            log?.Error("File not found: " + path);
            return;
        }
        if (sceneController == null)
            log?.Error("ISceneController not found");
        if (inputSettings == null)
            log?.Error("IInputSettings not found");
        SerializedScene serializedScene = null;
        try
        {
            var serializer = new XmlSerializer(typeof(SerializedScene));
            using (var stream = new FileStream(path, FileMode.Open))
            {
                serializedScene = serializer.Deserialize(stream) as SerializedScene;
            }
        }
        catch (System.Exception e)
        {
            log?.Error("Scene loading failed: " + e);
        }
        if (serializedScene == null)
            return;
        if (inputSettings != null)
        {
            inputSettings.MouseSensitivityMultiplier = serializedScene.mouseSensitivityMultiplier;
            inputSettings.CameraMoveSpeedMultiplier = serializedScene.cameraMoveSpeedMultiplier;
        }
        sceneController?.ClearScene();
        foreach (var obj in serializedScene.objects)
        {
            if (!System.Enum.TryParse(typeof(SceneObjectType), obj.type, out object value))
            {
                log?.Error("Type deserialization failed: " + obj.type);
                continue;
            }
            var newObject = sceneController?.CreateObject((SceneObjectType)value, obj.position);
            obj.Deserialize(newObject);
        }
    }

    public void Save(string path)
    {
        if (sceneController == null)
        {
            log?.Error("ISceneController not found");
            return;
        }
        if (inputSettings == null)
        {
            log?.Error("IInputSettings not found");
            return;
        }
        var serializedScene = new SerializedScene();
        serializedScene.mouseSensitivityMultiplier = inputSettings.MouseSensitivityMultiplier;
        serializedScene.cameraMoveSpeedMultiplier = inputSettings.CameraMoveSpeedMultiplier;
        serializedScene.objects = new List<SerializedSceneObject>();
        foreach (var obj in sceneController.Objects)
        {
            SerializedSceneObject serializedObject;
            if (obj is CubeObject cubeObject)
                serializedObject = new SerializedSceneCube(cubeObject);
            else if (obj is SphereObject sphereObject)
                serializedObject = new SerializedSceneSphere(sphereObject);
            else
            {
                log?.Error("Unknown object type: " + obj.GetType());
                continue;
            }
            serializedScene.objects.Add(serializedObject);
        }
        try
        {
            var serializer = new XmlSerializer(typeof(SerializedScene));
            using (var memoryStream = new MemoryStream())
            {
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                using (var xmlWriter = XmlWriter.Create(memoryStream, settings))
                {
                    serializer.Serialize(xmlWriter, serializedScene);
                    memoryStream.Flush();
                    memoryStream.Position = 0;
                }
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    memoryStream.CopyTo(fileStream);
                    fileStream.Close();
                }
            }

            log?.Notify("Saved: " + path);
        }
        catch (System.Exception e)
        {
            log?.Error("Scene saving failed: " + e);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        log = GetComponent<ILog>();
        sceneController = GetComponent<ISceneController>();
        inputSettings = GetComponent<IInputSettings>();
    }
}

using System.Xml.Serialization;
using UnityEngine;

[XmlInclude(typeof(SerializedSceneCube))]
[XmlInclude(typeof(SerializedSceneSphere))]
public class SerializedSceneObject
{
    public string type = string.Empty;
    public Vector2 position = Vector2.zero;
    public SerializedSceneObject()
    {
    }

    public SerializedSceneObject(ISceneObject obj)
    {
        type = obj.ObjectType.ToString();
        position = obj.Position;
    }
    public virtual void Deserialize(ISceneObject obj)
    {

    }
}

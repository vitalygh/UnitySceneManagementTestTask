using UnityEngine;

public interface ISceneObject
{
    SceneObjectType ObjectType { get; }
    Vector2 Position { get; }
    SerializedSceneObject Serialize(ISceneSerializer serializer);
}

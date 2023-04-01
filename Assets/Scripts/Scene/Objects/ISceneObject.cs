using UnityEngine;

public interface ISceneObject
{
    string ObjectType { get; }
    Vector2 Position { get; }
    SerializedSceneObject Serialize(ISceneSerializer serializer);
}

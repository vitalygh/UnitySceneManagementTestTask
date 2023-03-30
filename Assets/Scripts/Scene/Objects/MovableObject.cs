using UnityEngine;

public abstract class MovableObject : MonoBehaviour, ISceneObject
{
    public abstract SceneObjectType ObjectType { get; }
    public Vector2 Position
    {
        get
        {
            var position = transform.position;
            return new Vector2(position.x, position.z);
        }
        set
        {
            transform.position = new Vector3(value.x, 0.0f, value.y);
        }
    }

    public abstract SerializedSceneObject Serialize(ISceneSerializer serializer);
}

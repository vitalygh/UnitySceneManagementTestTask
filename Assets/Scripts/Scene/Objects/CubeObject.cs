public class CubeObject : MovableObject
{
    public override SceneObjectType ObjectType => SceneObjectType.Cube;
    public float uniqueCubeData = 0.0f;

    public override SerializedSceneObject Serialize(ISceneSerializer serializer)
    {
        return serializer.Serialize(this);
    }
}

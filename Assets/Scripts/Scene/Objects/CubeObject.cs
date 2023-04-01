public class CubeObject : MovableObject
{
    public override string ObjectType => "Cube";

    public float uniqueCubeData = 0.0f;

    public override SerializedSceneObject Serialize(ISceneSerializer serializer)
    {
        return serializer.Serialize(this);
    }
}

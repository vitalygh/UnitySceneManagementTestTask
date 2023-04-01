public class SphereObject : MovableObject
{
    public override string ObjectType => "Sphere";

    public int uniqueSphereData = 1;

    public override SerializedSceneObject Serialize(ISceneSerializer serializer)
    {
        return serializer.Serialize(this);
    }
}

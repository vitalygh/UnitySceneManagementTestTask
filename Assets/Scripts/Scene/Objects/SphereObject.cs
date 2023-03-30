public class SphereObject : MovableObject
{
    public override SceneObjectType ObjectType => SceneObjectType.Sphere;
    public int uniqueSphereData = 1;

    public override SerializedSceneObject Serialize(ISceneSerializer serializer)
    {
        return serializer.Serialize(this);
    }
}

public class SerializedSceneSphere : SerializedSceneObject
{
    public int uniqueSphereData = 0;

    public SerializedSceneSphere() : base()
    { 
    }

    public SerializedSceneSphere(SphereObject obj) : base(obj)
    {
        uniqueSphereData = obj.uniqueSphereData;
    }

    public override void Deserialize(ISceneObject obj)
    {
        if (obj is SphereObject sphereObject)
            sphereObject.uniqueSphereData = uniqueSphereData;
    }
}

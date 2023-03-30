public class SerializedSceneCube : SerializedSceneObject
{
    public float uniqueCubeData = 0.0f;

    public SerializedSceneCube() : base()
    {
    }

    public SerializedSceneCube(CubeObject obj) : base(obj)
    {
        uniqueCubeData = obj.uniqueCubeData;
    }

    public override void Deserialize(ISceneObject obj)
    {
        ((CubeObject)obj).uniqueCubeData = uniqueCubeData;
    }
}

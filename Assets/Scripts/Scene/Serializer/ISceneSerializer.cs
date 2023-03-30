public interface ISceneSerializer
{
    SerializedSceneObject Serialize(CubeObject cubeObject);
    SerializedSceneObject Serialize(SphereObject cubeObject);
    void Save(string path);
    void Load(string path);
}

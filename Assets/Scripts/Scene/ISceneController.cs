using System.Collections.Generic;
using UnityEngine;

public interface ISceneController
{
    ISceneObject CreateObject(SceneObjectType objectType, Vector2 position);
    void DestroyObject(ISceneObject sceneObject);
    IEnumerable<ISceneObject> Objects { get; }

    SceneObjectType CurrentType { get; set; }
    void ClearScene();
    void SaveScene();
    void LoadScene();
}

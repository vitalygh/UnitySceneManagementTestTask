using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ISceneController
{
    ISceneObject CreateObject(string objectType, Vector2 position);
    void DestroyObject(ISceneObject sceneObject);
    IEnumerable<string> ObjectTypes { get; }
    IEnumerable<ISceneObject> Objects { get; }

    string CurrentType { get; set; }
    UnityAction<string> OnCurrentTypeChanged { get; set; }

    void ClearScene();
    void SaveScene();
    void LoadScene();
}

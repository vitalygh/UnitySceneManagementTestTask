using UnityEngine.Events;

public interface IExtendedInputController
{
    UnityAction OnNextObjectTypeButtonClick { get; set; }
    UnityAction OnClearSceneButtonClick { get; set; }
    UnityAction OnSaveSceneButtonClick { get; set; }
    UnityAction OnLoadSceneButtonClick { get; set; }
}

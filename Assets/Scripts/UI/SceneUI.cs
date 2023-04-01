using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneUI : MonoBehaviour
{
    public TMP_Dropdown objectTypeDropDown = null;
    public Button clearSceneButton = null;
    public Button saveSceneButton = null;
    public Button loadSceneButton = null;
  
    public GameObject sceneControllerHolder = null;
    private ISceneController sceneController = null;
    private ILog log = null;

    // Start is called before the first frame update
    void Start()
    {
        log = sceneControllerHolder.GetComponent<ILog>();
        sceneController = sceneControllerHolder.GetComponent<ISceneController>();
        if (sceneController == null)
            log.Error("ISceneController not found");
        else
        {
            if (objectTypeDropDown == null)
                log.Error("objectTypeDropDown is null");
            else
            {
                var objectTypes = new List<string>(sceneController.ObjectTypes);
                objectTypeDropDown.ClearOptions();
                if (objectTypes.Count < 0)
                    log.Error("Object types list is empty");
                else
                {
                    objectTypeDropDown.AddOptions(objectTypes);
                    objectTypeDropDown.onValueChanged.AddListener((index) => sceneController.CurrentType = objectTypes[index]);
                    sceneController.CurrentType = objectTypes[objectTypeDropDown.value];
                }
            }

            if (clearSceneButton == null)
                log.Error("clearSceneButton is null");
            else
                clearSceneButton.onClick.AddListener(sceneController.ClearScene);

            if (saveSceneButton == null)
                log.Error("saveSceneButton is null");
            else
                saveSceneButton.onClick.AddListener(sceneController.SaveScene);

            if (loadSceneButton == null)
                log.Error("loadSceneButton is null");
            else
                loadSceneButton.onClick.AddListener(sceneController.LoadScene);
        }
    }
}

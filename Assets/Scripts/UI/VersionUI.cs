using UnityEngine;
using TMPro;

public class VersionUI : MonoBehaviour
{
    public  TMP_Text versionText = null;

    public GameObject sceneControllerHolder = null;
    private ILog log = null;

    // Start is called before the first frame update
    void Start()
    {
        log = sceneControllerHolder.GetComponent<ILog>();
        if (versionText == null)
            log.Error("versionText is null");
        else
            versionText.text = Application.version;
    }
}

using UnityEngine;
using TMPro;

public class SettingsUI : MonoBehaviour
{
    public TMP_InputField mouseSensitivityInputField = null;
    public TMP_InputField cameraMoveSpeedInputField = null;
    public GameObject sceneControllerHolder = null;
    private IInputSettings inputSettings = null;
    private ILog log = null;

    private void OnMouseSensitivityMultiplierChanged(float value)
    {
        mouseSensitivityInputField.text = value.ToString();
    }

    private void OnCameraMoveSpeedMultiplierChanged(float value)
    {
        cameraMoveSpeedInputField.text = value.ToString();
    }

    private void ApplyMouseSensitivityMultiplier(string value)
    {
        if (float.TryParse(value, out float result))
            inputSettings.MouseSensitivityMultiplier = result;
    }

    private void ApplyCameraMoveSpeedMultiplier(string value)
    {
        if (float.TryParse(value, out float result))
            inputSettings.CameraMoveSpeedMultiplier = result;
    }

    // Start is called before the first frame update
    void Start()
    {
        log = sceneControllerHolder.GetComponent<ILog>();
        inputSettings = sceneControllerHolder.GetComponent<IInputSettings>();
        if (inputSettings == null)
            log.Error("IInputSettings not found");
        else
        {
            if (mouseSensitivityInputField == null)
                log.Error("mouseSensitivityInputField is null");
            else
            {
                mouseSensitivityInputField.text = inputSettings.MouseSensitivityMultiplier.ToString();
                mouseSensitivityInputField.onEndEdit.AddListener(ApplyMouseSensitivityMultiplier);
                inputSettings.OnMouseSensitivityMultiplierChanged += OnMouseSensitivityMultiplierChanged;
            }

            if (cameraMoveSpeedInputField == null)
                log.Error("cameraMoveSpeedInputField is null");
            else
            {
                cameraMoveSpeedInputField.text = inputSettings.CameraMoveSpeedMultiplier.ToString();
                cameraMoveSpeedInputField.onEndEdit.AddListener(ApplyCameraMoveSpeedMultiplier);
                inputSettings.OnCameraMoveSpeedMultiplierChanged += OnCameraMoveSpeedMultiplierChanged;
            }
        }

    }

    private void OnDestroy()
    {
        if (inputSettings != null)
        {
            if (mouseSensitivityInputField != null)
                inputSettings.OnMouseSensitivityMultiplierChanged -= OnMouseSensitivityMultiplierChanged;
            if (cameraMoveSpeedInputField != null)
                inputSettings.OnCameraMoveSpeedMultiplierChanged -= OnCameraMoveSpeedMultiplierChanged;
        }
    }
}

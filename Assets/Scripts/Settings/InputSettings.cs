using UnityEngine;
using UnityEngine.Events;

public class InputSettings : MonoBehaviour, IInputSettings
{
    private float mouseSensitivityMultiplier = 1.0f;
    public float MouseSensitivityMultiplier
    {
        get
        {
            return mouseSensitivityMultiplier;
        }
        set
        {
            if (Mathf.Abs(mouseSensitivityMultiplier - value) > Mathf.Epsilon)
            {
                OnMouseSensitivityMultiplierChanged?.Invoke(value);
                mouseSensitivityMultiplier = value;
            }
        }
    }
    public UnityAction<float> OnMouseSensitivityMultiplierChanged { get; set; }

    private float cameraMoveSpeedMultiplier = 1.0f;
    public float CameraMoveSpeedMultiplier
    {
        get
        {
            return cameraMoveSpeedMultiplier;
        }
        set
        {
            if (Mathf.Abs(cameraMoveSpeedMultiplier - value) > Mathf.Epsilon)
            {
                OnCameraMoveSpeedMultiplierChanged?.Invoke(value);
                cameraMoveSpeedMultiplier = value;
            }
        }
    }
    public UnityAction<float> OnCameraMoveSpeedMultiplierChanged { get; set; }
}

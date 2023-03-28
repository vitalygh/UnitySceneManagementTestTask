using UnityEngine.Events;

public interface IInputSettings
{
    float MouseSensitivityMultiplier { get; set; }
    float CameraMoveSpeedMultiplier { get; set; }

    UnityAction<float> OnMouseSensitivityMultiplierChanged { get; set; }
    UnityAction<float> OnCameraMoveSpeedMultiplierChanged { get; set; }
}

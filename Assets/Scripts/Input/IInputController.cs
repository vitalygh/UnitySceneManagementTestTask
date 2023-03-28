using UnityEngine;
using UnityEngine.Events;

public interface IInputController
{
    Vector3 CursorPosition { get; }

    UnityAction<CameraMoveDirection> OnCameraMoveKeyDown { get; set; }
    UnityAction<CameraMoveDirection> OnCameraMove { get; set; }
    UnityAction<CameraMoveDirection> OnCameraMoveKeyUp { get; set; }

    bool IsCameraMove { get; }

    UnityAction OnCameraRotateKeyDown { get; set; }
    UnityAction<Vector3> OnCameraRotate { get; set; }
    UnityAction OnCameraRotateKeyUp { get; set; }

    bool IsCameraRotate { get; }

    UnityAction<Vector3> OnCreateButtonClick { get; set; }
    UnityAction<Vector3> OnDestroyButtonClick { get; set; }

    UnityAction<Vector3> OnObjectMoveKeyDown { get; set; }
    UnityAction<Vector3> OnObjectMove { get; set; }
    UnityAction<Vector3> OnObjectMoveKeyUp { get; set; }

}

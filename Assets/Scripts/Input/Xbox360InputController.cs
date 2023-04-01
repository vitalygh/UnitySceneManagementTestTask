using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Xbox360InputController : MonoBehaviour, IInputController
{
    private ILog log = null;
    private IInputSettings inputSettings = null;
    private bool objectMoveStarted = false;
    private const float JoystickDeadzone = 0.2f;
    private const float JoystickRotateMultiplier = 10.0f;
    private Dictionary<CameraMoveDirection, bool> cameraMove = new Dictionary<CameraMoveDirection, bool>
    {
        { CameraMoveDirection.Forward, false },
        { CameraMoveDirection.Back, false },
        { CameraMoveDirection.Right, false },
        { CameraMoveDirection.Left, false },
    };
    private bool pressedPitch = false;
    private bool pressedYaw = false;

    private readonly string LeftJoyX = "LeftJoyX";
    private readonly string LeftJoyY = "LeftJoyY";
    private readonly string RigthJoyX = "RightJoyX";
    private readonly string RigthJoyY = "RightJoyY";
    private readonly string JoyA = "JoyA";
    private readonly string JoyB = "JoyB";
    private readonly string JoyX = "JoyX";

    private Vector3 cursorPosition = Vector3.zero;
    public Vector3 CursorPosition => cursorPosition;

    public UnityAction<CameraMoveDirection> OnCameraMoveKeyDown { get; set; }
    public UnityAction<CameraMoveDirection> OnCameraMove { get; set; }
    public UnityAction<CameraMoveDirection> OnCameraMoveKeyUp { get; set; }

    public bool IsCameraMove
    { 
        get
        {
            if (Mathf.Abs(Input.GetAxis(LeftJoyX)) > JoystickDeadzone)
                return true;
            if (Mathf.Abs(Input.GetAxis(LeftJoyY)) > JoystickDeadzone)
                return true;
            return false;
        }
    }

    public UnityAction OnCameraRotateKeyDown { get; set; }
    public UnityAction<Vector3> OnCameraRotate { get; set; }
    public UnityAction OnCameraRotateKeyUp { get; set; }

    public bool IsCameraRotate
    {
        get
        {
            if (Mathf.Abs(Input.GetAxis(RigthJoyX)) > JoystickDeadzone)
                return true;
            if (Mathf.Abs(Input.GetAxis(RigthJoyY)) > JoystickDeadzone)
                return true;
            return false;
        }
    }

    public UnityAction<Vector3> OnCreateButtonClick { get; set; }
    public UnityAction<Vector3> OnDestroyButtonClick { get; set; }

    public UnityAction<Vector3> OnObjectMoveKeyDown { get; set; }
    public UnityAction<Vector3> OnObjectMove { get; set; }
    public UnityAction<Vector3> OnObjectMoveKeyUp { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        log = GetComponent<ILog>();
        inputSettings = GetComponent<IInputSettings>();
        if (inputSettings == null)
            log.Error("IInputSettings not found");
        cursorPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        var leftJoystickX = Input.GetAxis(LeftJoyX);
        var pressed = Mathf.Abs(leftJoystickX) > JoystickDeadzone;
        var dir = leftJoystickX > 0.0f ? CameraMoveDirection.Right : CameraMoveDirection.Left;
        if (cameraMove[dir] != pressed)
        {
            if (pressed)
                OnCameraMoveKeyDown?.Invoke(dir);
            else
                OnCameraMoveKeyUp?.Invoke(dir);
            cameraMove[dir] = pressed;
        }
        if (pressed)
            OnCameraMove?.Invoke(dir);

        var leftJoystickY = Input.GetAxis(LeftJoyY);
        pressed = Mathf.Abs(leftJoystickY) > JoystickDeadzone;
        dir = leftJoystickY > 0.0f ? CameraMoveDirection.Back : CameraMoveDirection.Forward;
        if (cameraMove[dir] != pressed)
        {
            if (pressed)
                OnCameraMoveKeyDown?.Invoke(dir);
            else
                OnCameraMoveKeyUp?.Invoke(dir);
            cameraMove[dir] = pressed;
        }
        if (pressed)
            OnCameraMove?.Invoke(dir);

        var rightJoystick = Vector3.zero;
        rightJoystick.x = Input.GetAxis(RigthJoyX);
        rightJoystick.y = -Input.GetAxis(RigthJoyY);
        var yaw = Mathf.Abs(rightJoystick.x) > JoystickDeadzone;
        var pitch = Mathf.Abs(rightJoystick.y) > JoystickDeadzone;
        if (pitch != pressedPitch)
        {
            if (pitch)
                OnCameraRotateKeyDown?.Invoke();
            else
                OnCameraRotateKeyUp?.Invoke();
        }
        if (yaw != pressedYaw)
        {
            if (yaw)
                OnCameraRotateKeyDown?.Invoke();
            else
                OnCameraRotateKeyUp?.Invoke();
        }
        pressedPitch = pitch;
        pressedYaw = yaw;
        if (pitch || yaw)
            OnCameraRotate?.Invoke(rightJoystick * JoystickRotateMultiplier);

        if (Input.GetButtonDown(JoyA))
            OnCreateButtonClick?.Invoke(CursorPosition);
        if (Input.GetButtonDown(JoyB))
            OnDestroyButtonClick?.Invoke(CursorPosition);
        if (Input.GetButtonDown(JoyX))
        {
            objectMoveStarted = true;
            OnObjectMoveKeyDown?.Invoke(CursorPosition);
        }
        if (Input.GetButton(JoyX))
        {
            if (objectMoveStarted)
                OnObjectMove?.Invoke(CursorPosition);
        }
        if (Input.GetButtonUp(JoyX))
        {
            if (objectMoveStarted)
            {
                OnObjectMoveKeyUp?.Invoke(CursorPosition);
                objectMoveStarted = false;
            }
        }
    }
}

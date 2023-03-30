using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour, ICameraController
{
    private Camera attachedCamera = null;
    public Camera AttachedCamera
    {
        get
        {
            if (attachedCamera == null)
                attachedCamera = GetComponent<Camera>();
            return attachedCamera;
        }
    }

    private Vector3 angle = Vector3.zero;
    private float MinPitch { get { return -90.0f; } }
    private float MaxPitch { get { return 90.0f; } }
    private float MinHeight { get { return 0.4f; } }

    public void MoveForward(float delta)
    {
        SetPosition(transform.position + transform.forward * delta);
    }

    public void MoveBack(float delta)
    {
        SetPosition(transform.position - transform.forward * delta);
    }

    public void MoveRight(float delta)
    {
        SetPosition(transform.position + transform.right * delta);
    }

    public void MoveLeft(float delta)
    {
        SetPosition(transform.position - transform.right * delta);
    }

    public void Pitch(float delta)
    {
        angle.x = Mathf.Clamp(angle.x + delta, MinPitch, MaxPitch);
        transform.rotation = Quaternion.Euler(angle);
    }

    public void Yaw(float delta)
    {
        angle.y = (angle.y + delta) % 360.0f;
        transform.rotation = Quaternion.Euler(angle);
    }

    public Ray ScreenPointToRay(Vector3 screenPoint)
    {
        if (AttachedCamera != null)
            return AttachedCamera.ScreenPointToRay(screenPoint);
        return new Ray();
    }

    private void SetPosition(Vector3 newPosition)
    {
        if (newPosition.y < MinHeight)
            newPosition.y = MinHeight;
        transform.position = newPosition;
    }
}

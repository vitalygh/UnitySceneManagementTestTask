using UnityEngine;

public interface ICameraController
{
    void MoveForward(float delta);
    void MoveBack(float delta);
    void MoveRight(float delta);
    void MoveLeft(float delta);
    void Pitch(float delta);
    void Yaw(float delta);
    Ray ScreenPointToRay(Vector3 screenPoint);
}

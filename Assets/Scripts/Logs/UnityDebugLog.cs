using UnityEngine;

public class UnityDebugLog : MonoBehaviour, ILog
{
    public void Notify(string message)
    {
        Debug.Log(message);
    }

    public void Warning(string message)
    {
        Debug.LogWarning(message);
    }

    public void Error(string message)
    {
        Debug.LogError(message);
    }
}

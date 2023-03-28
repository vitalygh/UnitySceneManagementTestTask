public interface ILog
{
    public void Notify(string message);
    public void Warning(string message);
    public void Error(string message);
}

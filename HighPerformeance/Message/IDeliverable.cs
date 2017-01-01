namespace HighPerformanceTests.Message
{
    public interface IDeliverable
    {
        Message Send(Message m);
    }
}
namespace HighPerformance.Messaging
{
    public interface IDeliverable
    {
        Message Send(Message m);
    }
}
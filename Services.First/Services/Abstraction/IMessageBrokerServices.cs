namespace Services.First.Services.Abstraction
{
    public interface IMessageBrokerServices
    {
        void SendMessage<T>(T message);

        void ReceivedMessage<T>(Action<T> action);
    }
}

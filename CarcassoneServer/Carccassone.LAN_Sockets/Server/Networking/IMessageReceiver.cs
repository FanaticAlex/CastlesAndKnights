namespace Sockets.Server.Networking
{
    /// <summary>
    /// Интерфейс сервиса принимающего сообщения от клиентов.
    /// </summary>
    internal interface IMessageReceiver
    {
        void Start();
    }
}

namespace Sockets.Client.Networking
{
    /// <summary>
    /// Интерфейс сервисов отправляющих сообщения на сервер.
    /// </summary>
    public interface IMessageSender
    {
        /// <summary>
        /// Отправляет сообщение.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        void SendMessage(string message);
    }
}

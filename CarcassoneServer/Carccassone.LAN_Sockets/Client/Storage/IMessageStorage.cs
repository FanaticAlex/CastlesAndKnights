namespace Sockets.Client.Storage
{
    using System.Collections.Generic;
    using Sockets.Client.Storage.DataModel;

    /// <summary>
    /// Интерфейс сервиса сохранения сообщений.
    /// </summary>
    internal interface IMessageStorage
    {
        /// <summary>
        /// Сохраняет сообщение.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <returns>Объект нового сообщения.</returns>
        MessageEntity SaveMessage(string message);

        /// <summary>
        /// Устанавливает статус сообщения "отправлено".
        /// </summary>
        /// <param name="messageId">Идентификатор сообщения.</param>
        void SetAsSent(int messageId);

        /// <summary>
        /// Возвращает все неотправленные сообщения.
        /// </summary>
        /// <returns>Список неотправленных сообщений.</returns>
        List<MessageEntity> GetAllNotSentMessages();
    }
}

namespace Sockets.Client
{
    using System;
    using Sockets.Client.Networking;
    using Sockets.Client.Storage;
    using Sockets.Client.Storage.DataModel;

    /// <summary>
    /// Класс принимает от пользователя сообщение в коммандной строке и передает в сервис сохранения сообщений.
    /// </summary>
    internal class ConsoleMessager : IMessager
    {
        private readonly IMessageStorage _storage;
        private readonly IMessageSender _sender;
        private readonly ILogger _logger;

        private readonly string _successMessage = "сообщение отправлено";
        private readonly string _failureMessage = "сообщение не отправлено. Ожидается повторная отправка.";

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleMessager"/> class.
        /// </summary>
        /// <param name="storage">Storage service.</param>
        /// <param name="sender">Message sender service.</param>
        /// <param name="logger">Logger service.</param>
        public ConsoleMessager(IMessageStorage storage, IMessageSender sender, ILogger logger)
        {
            _storage = storage;
            _sender = sender;
            _logger = logger;
        }

        /// <inheritdoc/>
        public void Run()
        {
            while (true)
            {
                Console.WriteLine("Введите сообщение: ");
                var message = Console.ReadLine();

                if (string.IsNullOrEmpty(message))
                    continue;

                if (message.ToLower() == "-exit")
                    return;

                ProcessMessage(message);
            }
        }

        public void ReSendPreviousMessages()
        {
            var messages = _storage.GetAllNotSentMessages();
            foreach (var message in messages)
                Send(message);
        }

        public void ProcessMessage(string message)
        {
            var messageEntity = _storage.SaveMessage(message);
            Send(messageEntity);
        }

        /// <summary>
        /// Регистрирует сообщение неопределенного типа.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        public void SetMessage(object message)
        {
            if (message == null)
                return;

            if (message is string)
                ProcessMessage((string)message);

            throw new Exception($"Сообщения типа {message.GetType()} недопустимы");
        }

        private void Send(MessageEntity messageEntity)
        {
            try
            {
                _sender.SendMessage(messageEntity.Value);
                _storage.SetAsSent(messageEntity.Id);
                _logger.Info(_successMessage);
            }
            catch
            {
                _logger.Error(_failureMessage);
            }
        }
    }
}

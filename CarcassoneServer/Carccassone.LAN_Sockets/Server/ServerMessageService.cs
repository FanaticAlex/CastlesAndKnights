using Sockets.Server.Networking;
using Sockets.Server.Storage;
using System;

namespace Sockets.Server
{
    /// <summary>
    /// Сервис, обрабатывающий сообщения от клиентов.
    /// </summary>
    class ServerMessageService
    {
        private IMessageReceiver _receiver;
        private readonly Logger _logger;

        public ServerMessageService(IMessageStorage storage, IMessageReceiver receiver, Logger logger)
        {
            _receiver = receiver;
            _storage = storage;
            _logger= logger;
            receiver.MessageReceived += ReceiverOnMessageReceived;
        }

        public void Start()
        {
            _receiver.Start();
        }

        /// <summary>
        /// Печатает в кнсоль все сообщения от пользователей
        /// </summary>
        public void Print()
        {
            var messages = _storage.GetMessages();
            for (int i = 0; i < messages.Count; i++)
            {
                _logger.Info($"{i}  time:{messages[i].Time}  IP:{messages[i].IpAddress}  Value:{messages[i].Value}");
            }
        }

        protected void ReceiverOnMessageReceived(object? sender, MessageEntityServer message)
        {
            _storage.StorageMessage(message);
        }
    }
}

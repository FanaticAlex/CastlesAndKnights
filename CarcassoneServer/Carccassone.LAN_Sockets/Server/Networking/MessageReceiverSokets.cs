using Microsoft.Extensions.Logging;
using System.Net.Sockets;

namespace Sockets.Server.Networking
{
    /// <summary>
    /// Осуществляет управление подключенными клиентами.
    /// </summary>
    public partial class MessageReceiverSokets : IMessageReceiver
    {
        private readonly TcpListener _tcpListener;
        private readonly List<ClientMessagesHandler> clients = new();
        private readonly ILogger _logger;

        /// <summary>
        /// Запускает асинхронную процедуру прослушивания сообщений от клиентов.
        /// </summary>
        public MessageReceiverSokets(ServerMessageServiceSettings settings, ILogger logger)
        {
            _tcpListener = new TcpListener(settings.IPAddress, settings.Port);
            _logger = logger;
        }

        public void Start()
        {
            try
            {
                var task = Task.Factory.StartNew(Listen);
            }
            catch (Exception ex)
            {
                Disconnect();
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Прослушивание входящих подключений.
        /// </summary>
        protected void Listen()
        {
            try
            {
                _tcpListener.Start();
                _logger.LogInformation("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    TcpClient tcpClient = _tcpListener.AcceptTcpClient();

                    ClientMessagesHandler clientObject = new(tcpClient);
                    clientObject.MessageReceived += ClientObjectOnMessageReceived;
                    clientObject.NameReceived += ClientObjectOnNameReceived;
                    clientObject.ClientLeft += ClientObjectOnClentLeft;
                    clientObject.Closed += ClientObjectOnClosed;
                    clients.Add(clientObject);

                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                Disconnect();
            }
        }

        protected void RemoveConnection(string id)
        {
            var client = clients.FirstOrDefault(item => item.Id == id);
            if (client != null)
            {
                client.MessageReceived -= ClientObjectOnMessageReceived;
                client.NameReceived -= ClientObjectOnNameReceived;
                client.Closed -= ClientObjectOnClosed;
                clients.Remove(client);
            }
        }

        /// <summary>
        /// Отключение всех клиентов.
        /// </summary>
        public void Disconnect()
        {
            _tcpListener.Stop();
            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close();
            }

            Environment.Exit(0);
        }

        protected void ClientObjectOnMessageReceived(object? sender, string message)
        {
            if (sender == null)
                return;

            var client = (ClientMessagesHandler)sender;
            _logger.LogInformation($"получено сообщение: {message}");
        }

        protected void ClientObjectOnNameReceived(object? sender, string name)
        {
            _logger.LogInformation($"клиент {name} подключился");
        }

        protected void ClientObjectOnClentLeft(object? sender, EventArgs e)
        {
            if (sender == null)
                return;

            var client = (ClientMessagesHandler)sender;
            _logger.LogInformation($"{client.UserName}: отключился");
        }

        protected void ClientObjectOnClosed(object? sender, EventArgs e)
        {
            if (sender == null)
                return;

            var client = (ClientMessagesHandler)sender;
            RemoveConnection(client.Id);
            _logger.LogInformation($"{client.UserName} отключился");
        }
    }
}

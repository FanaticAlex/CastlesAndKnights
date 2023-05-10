using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Sockets.Server.Networking
{
    /// <summary>
    /// Обьект получающий сообщения от клиента.
    /// </summary>
    public class ClientMessagesHandler
    {
        private readonly TcpClient _client;

        public string Id { get; private set; }
        public NetworkStream? Stream { get; private set; }
        public string? UserName { get; private set; }
        public string IPAddress { get; private set; }

        public event EventHandler<string>? MessageReceived;
        public event EventHandler<string>? NameReceived;
        public event EventHandler? ClientLeft;
        public event EventHandler? Closed;

        public ClientMessagesHandler(TcpClient tcpClient)
        {
            Id = Guid.NewGuid().ToString();

            if ((tcpClient?.Client?.RemoteEndPoint as IPEndPoint) == null)
                throw new Exception("Неверно настроен клиент");

            IPAddress = ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString();
            _client = tcpClient;
        }

        public void Process()
        {
            try
            {
                Stream = _client.GetStream();

                // получаем имя пользователя
                string userName = GetMessage();
                NameReceived?.Invoke(this, userName);

                // в бесконечном цикле получаем сообщения от клиента
                while (true)
                {
                    string message = "";
                    try
                    {
                        message = GetMessage();
                        if (message != "")
                        {
                            MessageReceived?.Invoke(this, message);
                        }
                    }
                    catch
                    {
                        ClientLeft?.Invoke(this, new EventArgs());
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
            finally
            {
                // в случае выхода из цикла закрываем ресурсы
                Closed?.Invoke(this, new EventArgs());
                Close();
            }
        }

        // чтение входящего сообщения и преобразование в строку
        private string GetMessage()
        {
            byte[] data = new byte[64]; // буфер для получаемых данных
            var builder = new StringBuilder();
            int bytes;

            if (Stream == null)
                throw new Exception("Ошибка обработки сообщения");

            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (Stream.DataAvailable);

            return builder.ToString();
        }

        // закрытие подключения
        protected internal void Close()
        {
            Stream?.Close();
            _client?.Close();
        }
    }
}

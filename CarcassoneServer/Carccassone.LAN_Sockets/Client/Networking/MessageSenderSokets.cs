namespace Sockets.Client.Networking
{
    using System.Net.Sockets;
    using System.Text;

    internal class MessageSenderSokets : IMessageSender
    {
        private string _userName;
        private TcpClient _client;
        private NetworkStream _stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSenderSokets"/> class.
        /// </summary>
        /// <param name="settinds">Настройки.</param>
        /// <param name="userName">Имя пользователя.</param>
        public MessageSenderSokets(ClientSettings settinds, string userName)
        {
            _userName = userName;

            try
            {
                _client = new TcpClient();
                _client.Connect(settinds.ServerIpAddress, settinds.Port); // подключение клиента
                _stream = _client.GetStream(); // получаем поток

                string message = _userName;
                byte[] data = Encoding.Unicode.GetBytes(message);
                _stream.Write(data, 0, data.Length);

                var task = Task.Factory.StartNew(ReceiveMessage);
            }
            catch (Exception ex)
            {
                Disconnect();
                throw new Exception("Проблема при подключении к серверу", ex);
            }
        }

        /// <summary>
        /// Отправляет сообщение на сервер.
        /// </summary>
        /// <param name="message">Сообщение..</param>
        public void SendMessage(string message)
        {
            var data = Encoding.Unicode.GetBytes(message);
            _stream.Write(data, 0, data.Length);
        }

        /// <summary>
        /// Получает сообщение с сервера.
        /// </summary>
        private void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[64]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;

                    do
                    {
                        bytes = _stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (_stream.DataAvailable);

                    string message = builder.ToString();
                    Console.WriteLine(message);//вывод сообщения
                }
                catch
                {
                    Console.WriteLine("Подключение прервано!"); //соединение было прервано
                    Console.ReadLine();
                    Disconnect();
                }
            }
        }

        /// <summary>
        /// Отключает клиент от сервера.
        /// </summary>
        public void Disconnect()
        {
            if (_stream != null)
            {
                _stream.Close();//отключение потока
            }

            if (_client != null)
            {
                _client.Close();//отключение клиента
            }

            Environment.Exit(0); //завершение процесса
        }
    }
}

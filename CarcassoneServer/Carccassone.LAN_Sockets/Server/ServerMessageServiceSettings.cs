using System.Net;

namespace Sockets.Server
{
    /// <summary>
    /// Настройки сервиса сообщений.
    /// </summary>
    public class ServerMessageServiceSettings
    {
        public int Port { get; set; } = 8888;
        public IPAddress IPAddress { get; set; } = IPAddress.Any;
    }
}

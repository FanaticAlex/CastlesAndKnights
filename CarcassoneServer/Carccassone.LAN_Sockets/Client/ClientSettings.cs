namespace Sockets.Client
{
    /// <summary>
    /// Настройки сервиса сообщений.
    /// </summary>
    internal class ClientSettings
    {
        /// <summary>
        /// Gets or sets server IP address.
        /// </summary>
        required public string ServerIpAddress { get; set; } = "127.0.0.1";

        /// <summary>
        /// Gets or sets server connection port.
        /// </summary>
        required public int Port { get; set; } = 8888;
    }
}

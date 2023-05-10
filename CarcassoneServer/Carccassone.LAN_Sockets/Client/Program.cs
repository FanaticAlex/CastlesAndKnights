namespace Sockets.Client
{
    using Sockets.Client.Networking;
    using Sockets.Client.Storage;

    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var settings = LoadSettings();

                Console.Write("Введите свое имя: ");
                var userName = Console.ReadLine();
                if (string.IsNullOrEmpty(userName))
                    throw new Exception("Имя пользователя не может быть пустым");

                var storage = new MessageStorageEntityFramework();
                var sender = new MessageSenderSokets(settings, userName);
                var logger = new Logger();

                var client = new ConsoleMessager(storage, sender, logger);
                client.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка:");
                Console.WriteLine(ex);
                if (ex.InnerException != null)
                    Console.WriteLine(ex.InnerException.Message);

                Console.ReadKey();
            }
        }

        private static ClientSettings LoadSettings()
        {
            var settings = new ClientSettings()
            {
                ServerIpAddress = "127.0.0.1",
                Port = 8888,
            };

            return settings;
        }
    }
}

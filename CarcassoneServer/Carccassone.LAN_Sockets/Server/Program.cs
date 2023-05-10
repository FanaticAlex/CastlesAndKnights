using Sockets.Server.Networking;
using System;

namespace Sockets.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = LoadSettings();

            var receiver = new MessageReceiverSokets(settings, logger);

            var service = new ServerMessageService(storage, receiver, logger);
            service.Start();

            // прослушиваем комманды от пользователя.
            while (true)
            {
                var command = Console.ReadLine();
                if (command == "print")
                {
                    service.Print();
                }
            }
        }

        protected static ServerMessageServiceSettings LoadSettings()
        {
            var settings = new ServerMessageServiceSettings();

            // здесь могла быть загрузка из файла настроек...
            settings.Port = 8888;

            return settings;
        }
    }
}

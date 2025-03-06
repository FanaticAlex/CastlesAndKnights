using System;

namespace Assets.Scripts
{
    /// <summary>
    /// Отвечает за создание синглтона сервиса игры 
    /// </summary>
    internal sealed class GameManager
    {
        private static readonly Lazy<GameManager> _instance = new Lazy<GameManager>(() => new GameManager());

        private GameManager()
        {
        }

        public static GameManager Instance => _instance.Value;

        public IGameService RoomService { get; private set; }

        public void SetOfflineMode()
        {
            if (RoomService is not OfflineGameService)
                RoomService = new OfflineGameService();
        }

        public void SetOnlineMode()
        {
            try
            {
                //if (RoomService is not OnlineGameService)
                //    RoomService = new OnlineGameService();
            }
            catch (Exception ex)
            {
                Logger.Info($"No connection. {ex.Message}");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carcassone.ApiClient;

namespace Assets.Scripts
{
    internal sealed class GameManager
    {
        private static readonly Lazy<GameManager> instanceHolder =
            new Lazy<GameManager>(() => new GameManager());

        private GameManager()
        {
        }

        public static GameManager Instance => instanceHolder.Value;

        public IGameService RoomService { get; private set; }

        public void SetOfflineMode()
        {
            RoomService = new OfflineGameService();
        }

        public void SetOnlineMode()
        {
            RoomService = new OnlineGameService();
        }
    }
}

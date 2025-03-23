using Carcassone.Core;
using Carcassone.Core.Calculation;
using Carcassone.Core.Calculation.Objects;
using Carcassone.Core.Cards;
using Carcassone.Core.Fields;
using Carcassone.Core.Players;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    /// <summary>
    /// Provide functons to access game logic
    /// </summary>
    internal interface IGameService
    {
        GameRoom Room { get; }

        void AddPlayer(string playerName, PlayerType type);
        void DeletePlayer(string playerName);
        void Start();

        BasePlayer GetPlayer(string playerName);
        List<BasePlayer> GetPlayers();
        BasePlayer GetCurrentPlayer();

        List<Field> GetFields();

        List<ObjectPart> GetActiveParts();

        List<PlayerScore> GetGameScores();
        PlayerScore GetScore(string playerName);
        List<Road> GetRoads();
        List<Castle> GetCastles();
        List<Cornfield> GetCornfields();
        List<Church> GetChurches();
    }
}

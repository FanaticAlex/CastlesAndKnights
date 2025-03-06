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
        void AddPlayer(string playerName, PlayerType type);
        void DeletePlayer(string playerName);
        void Start();
        void EndTurn(string playerName);

        GameRoom GetRoom();

        BasePlayer GetPlayer(string playerName);
        List<BasePlayer> GetPlayers();
        Task<BasePlayer> GetCurrentPlayer();

        Card GetCurrentCard();
        List<Card> GetCards();
        Card GetCard(string cardName);
        bool CanPutCard(string fieldId, string cardName);
        void PutCard(string fieldId, string cardName, string playerName);
        void RotateCard(string cardName);

        List<Field> GetFields();
        List<Field> GetAvailableFields(string cardName);
        List<Field> GetNotAvailableFields();

        List<ObjectPart> GetAvailableObjectParts(string cardId);
        List<ObjectPart> GetActiveParts();
        void PutChip(string cardName, string partId, string playerName);

        List<PlayerScore> GetGameScores();
        PlayerScore GetScore(string playerName);
        List<Road> GetRoads();
        List<Castle> GetCastles();
        List<Cornfield> GetCornfields();
        List<Church> GetChurches();

        int GetCardsRemain();

        void Reset();
    }
}

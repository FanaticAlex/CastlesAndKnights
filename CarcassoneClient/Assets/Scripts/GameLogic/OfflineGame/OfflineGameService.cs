using Assets.Scripts.Game;
using Carcassone.Core;
using Carcassone.Core.Calculation;
using Carcassone.Core.Calculation.Objects;
using Carcassone.Core.Cards;
using Carcassone.Core.Fields;
using Carcassone.Core.Players;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    internal class OfflineGameService : IGameService
    {
        public GameRoom Room { get; private set; }

        public OfflineGameService()
        {
            Room = new GameRoom();
        }

        public void AddPlayer(string playerName, PlayerType type)
        {
            Room.PlayersPool.AddPlayer(playerName, type);
        }

        public void Start() => Room.Start();

        public BasePlayer GetPlayer(string playerName) => Room.PlayersPool.GetPlayer(playerName);
        public List<BasePlayer> GetPlayers() => Room.PlayersPool.Players;
        BasePlayer IGameService.GetCurrentPlayer()
        {
            return Room.PlayersPool.GetCurrentPlayer();
        }
        public void DeletePlayer(string userName) => Room.PlayersPool.DeletePlayer(userName);

        public Card GetCurrentCard() => Room.GetCurrentCard();
        public List<Card> GetCards() => Room.CardsPool.AllCards;
        public Card GetCard(string cardName) => Room.GetCard(cardName);
        public bool CanPutCard(string fieldId, string cardName) => Room.CanPutCard(fieldId, cardName);

        public List<Field> GetFields() => Room.FieldBoard.Fields;
        public List<Field> GetAvailableFields(string cardName) => Room.GetAvailableFields(cardName);
        public List<Field> GetNotAvailableFields() => Room.GetNotAvailableFields();

        public List<ObjectPart> GetActiveParts() => Room.GetActiveParts();

        public List<PlayerScore> GetGameScores() => throw new NotImplementedException();
        public PlayerScore GetScore(string playerName) => Room.GetPlayerScore(Room.PlayersPool.GetPlayer(playerName));
        public List<Road> GetRoads() => Room.ScoreCalculator.Roads;
        public List<Castle> GetCastles() => Room.ScoreCalculator.Castles;
        public List<Cornfield> GetCornfields() => Room.ScoreCalculator.Cornfields;
        public List<Church> GetChurches() => Room.ScoreCalculator.Churches;
    }
}

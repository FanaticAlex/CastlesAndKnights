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
        private GameRoom _room;

        public OfflineGameService()
        {
            _room = new GameRoom();
        }

        public void AddPlayer(string playerName, PlayerType type)
        {
            _room.PlayersPool.AddPlayer(playerName, type);
        }

        public void Start() => _room.Start();

        public GameRoom GetRoom() => _room;

        public BasePlayer GetPlayer(string playerName) => _room.PlayersPool.GetPlayer(playerName);
        public List<BasePlayer> GetPlayers() => _room.PlayersPool.Players;
        Task<BasePlayer> IGameService.GetCurrentPlayer()
        {
            return Task.FromResult(_room.PlayersPool.GetCurrentPlayer());
        }
        public void DeletePlayer(string userName) => _room.PlayersPool.DeletePlayer(userName);

        public Card GetCurrentCard() => _room.GetCurrentCard();
        public List<Card> GetCards() => _room.CardsPool.AllCards;
        public Card GetCard(string cardName) => _room.GetCard(cardName);
        public bool CanPutCard(string fieldId, string cardName) => _room.CanPutCard(fieldId, cardName);

        public void RotateCard(string cardName) => _room.RotateCard(cardName);

        public List<Field> GetFields() => _room.FieldBoard.Fields;
        public List<Field> GetAvailableFields(string cardName) => _room.GetAvailableFields(cardName);

        public List<Field> GetNotAvailableFields() => _room.GetNotAvailableFields();

        public List<ObjectPart> GetAvailableObjectParts(string cardId) => _room.GetAvailableParts(cardId);
        
        public List<ObjectPart> GetActiveParts() => _room.GetActiveParts();

        public List<PlayerScore> GetGameScores() => throw new NotImplementedException();
        public PlayerScore GetScore(string playerName) => _room.GetPlayerScore(_room.PlayersPool.GetPlayer(playerName));
        public List<Road> GetRoads() => _room.ScoreCalculator.Roads;
        public List<Castle> GetCastles() => _room.ScoreCalculator.Castles;
        public List<Cornfield> GetCornfields() => _room.ScoreCalculator.Cornfields;
        public List<Church> GetChurches() => _room.ScoreCalculator.Churches;

        public int GetCardsRemain() => _room.GetCardsRemain();

        public void Reset() => throw new NotImplementedException();

        public void PutCard(string fieldId, string cardName, string playerName)
        {
            var human = _room.PlayersPool.GetPlayer(playerName);
            human.SetPlayerMove1(_room, cardName, fieldId);
        }

        public void PutChip(string cardName, string partId, string playerName)
        {
            var human = _room.PlayersPool.GetPlayer(playerName);
            human.SetPlayerMove2(_room, cardName, partId);
        }

        public void EndTurn(string playerName)
        {
            var human = _room.PlayersPool.GetPlayer(playerName);
            human.SetPlayerMove3(_room);

            // AI players move
            Task.Run(_room.AllAiPlayersMove);
        }
    }
}

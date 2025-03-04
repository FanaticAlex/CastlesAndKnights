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
        private Carcassone.Core.GameRoom _room;

        public OfflineGameService()
        {
            Create();
            MenuManager.IAmGameMaster = true;
        }

        public string HumanUser { get; set; }

        public void Create() { _room = new Carcassone.Core.GameRoom(); }
        public void Connect(string roomId) => throw new NotImplementedException();
        public void Login(string login, string password) => throw new NotImplementedException();
        public void Login(SavedAuthData data) => throw new NotImplementedException();
        public void AddPlayer(string playerName, PlayerType type)
        {
            _room.PlayersPool.AddPlayer(playerName, (Carcassone.Core.Players.PlayerType)type);
            if (type == PlayerType.Human)
                HumanUser = playerName;
        }

        public void Start() => _room.Start();

        public GameRoom GetRoom() => _room;

        public BasePlayer GetPlayer(string playerName) => ToCommon<BasePlayer>(_room.PlayersPool.GetPlayer(playerName));
        public List<BasePlayer> GetPlayers() => ToCommon<List<BasePlayer>>(_room.PlayersPool.Players);
        Task<BasePlayer> IGameService.GetCurrentPlayer()
        {
            return Task.FromResult(ToCommon<BasePlayer>(_room.PlayersPool.GetCurrentPlayer()));
        }
        public void DeletePlayer(string userName)
        {
            _room.PlayersPool.DeletePlayer(userName);
        }

        public Card GetCurrentCard() => ToCommon<Card>(_room.GetCurrentCard());
        public List<Card> GetCards() => ToCommon<List<Card>>(_room.CardsPool.AllCards);
        public Card GetCard(string cardName) => ToCommon<Card>(_room.GetCard(cardName));
        public bool CanPutCard(string fieldId, string cardName) => _room.CanPutCard(fieldId, cardName);

        public void RotateCard(string cardName) => _room.RotateCard(cardName);

        public List<Field> GetFields() => ToCommon<List<Field>>(_room.FieldBoard.Fields);
        public List<Field> GetAvailableFields(string cardName) => ToCommon<List<Field>>(_room.GetAvailableFields(cardName));

        public List<Field> GetNotAvailableFields() => ToCommon<List<Field>>(_room.GetNotAvailableFields());

        public List<ObjectPart> GetAvailableObjectParts(string cardId) => ToCommon<List<ObjectPart>>(_room.GetAvailableParts(cardId));
        
        public List<ObjectPart> GetActiveParts() => ToCommon<List<ObjectPart>>(_room.GetActiveParts());

        public List<PlayerScore> GetGameScores() => throw new NotImplementedException();
        public PlayerScore GetScore(string playerName) => ToCommon<PlayerScore>(_room.GetPlayerScore(_room.PlayersPool.GetPlayer(playerName)));
        public List<Road> GetRoads() => ToCommon<List<Road>>(_room.ScoreCalculator.Roads);
        public List<Castle> GetCastles() => ToCommon<List<Castle>>(_room.ScoreCalculator.Castles);
        public List<Cornfield> GetCornfields() => ToCommon<List<Cornfield>>(_room.ScoreCalculator.Cornfields);
        public List<Church> GetChurches() => ToCommon<List<Church>>(_room.ScoreCalculator.Churches);

        public int GetCardsRemain() => _room.GetCardsRemain();

        public void Reset() => HumanUser = string.Empty;

        public UserInfo GetUserInfo(string userName) => new();

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

        private T ToCommon<T>(object obj)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));
        }
    }
}

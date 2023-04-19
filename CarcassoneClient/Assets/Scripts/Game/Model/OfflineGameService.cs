using Assets.Scripts.Game;
using Carcassone.ApiClient;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Assets.Scripts
{
    /*internal class OfflineGameService : IGameService
    {
        private Carcassone.Core.GameRoom room;

        public OfflineGameService()
        {
            User = "LocalPlayerMe";

            Create();
            AddHuman(User);
        }

        public String User { get; private set; }

        public void Create() { room = new Carcassone.Core.GameRoom(); }
        public void Connect(string roomId) => throw new NotImplementedException();
        public void Login(string login, string password) => throw new NotImplementedException();
        public void AddHuman(string userName) => room.AddHumanPlayer(userName);
        public void AddAI() => room.AddAIPlayer();
        public void Start() => room.Start();
        public void EndTurn(string userName) => room.EndTurn();

        public Carcassone.ApiClient.GameRoom GetRoom() => room.ToCommon();
        public List<string> GetRoomsIds() => throw new NotImplementedException();

        public Carcassone.ApiClient.BasePlayer GetPlayer(string playerName) => room.GetPlayer(playerName).ToCommon();
        public List<Carcassone.ApiClient.BasePlayer> GetPlayers() => room.GetPlayers().Select(p => p.ToCommon()).ToList();
        public Carcassone.ApiClient.BasePlayer GetCurrentPlayer() => room.GetCurrentPlayer().ToCommon();

        public Carcassone.ApiClient.Card GetCurrentCard() => room.GetCurrentCard().ToCommon();
        public List<Carcassone.ApiClient.Card> GetCards() => room.GetAllCards().Select(c => c.ToCommon()).ToList();
        public List<Carcassone.ApiClient.Card> GetActiveCards() => room.GetFields().Where(f => f.Card != null).Select(f => f.Card.ToCommon()).ToList();
        public Carcassone.ApiClient.Card GetCard(string cardName) => room.GetCard(cardName).ToCommon();
        public bool CanPutCard(string fieldId, string cardName) => room.CanPutCard(fieldId, cardName);
        public void PutCard(string fieldId, string cardName, string userName) => room.PutCardInField(room.GetCard(cardName), room.GetField(fieldId));
        public void RotateCard(string cardName) => room.RotateCard(cardName);

        public List<Field> GetFields() => room.GetFields().Select(f => f.ToCommon()).ToList();
        public List<Field> GetAvailableFields(string cardName) => room.GetAvailableFields(cardName).Select(f => f.ToCommon()).ToList();
        public List<Field> GetNotAvailableFields() => room.GetNotAvailableFields().Select(f => f.ToCommon()).ToList();

        public List<Carcassone.ApiClient.ObjectPart> GetAvailableObjectParts(string cardId) => room.GetAvailableParts(cardId).Select(p => p.ToCommon()).ToList();
        public void PutChip(string cardName, string partId, string playerName)
        {
            var card = room.GetCard(cardName);
            var part = card.Parts.FirstOrDefault(p => p.PartId == partId);
            var player = room.GetPlayer(playerName);
            room.PutChipInCard(part, player);
        }

        public List<UserGameScore> GetGameScores() => throw new NotImplementedException();
        public PlayerScore GetScore(string playerName) => room.GetPlayerScore(room.GetPlayer(playerName)).ToCommon();
        public List<Road> GetRoads() => room.GetRoads().Select(r => r.ToCommon()).ToList();
        public List<Castle> GetCastles() => room.GetCastles().Select(c => c.ToCommon()).ToList();
        public List<Cornfield> GetCornfields() => room.GetCornfields().Select(c => c.ToCommon()).ToList();
        public List<Church> GetChurches() => room.GetChurches().Select(c => c.ToCommon()).ToList();

        public int GetCardsRemain() => room.GetCardsRemain();

        public void Reset()
        {
            User = null;
        }

        public UserStatistic GetUserStatistic(string userName) => new UserStatistic();

        public void LoginWithSavedToken(string login, string token)
        {
            throw new NotImplementedException();
        }

        public void Login(SavedAuthData data)
        {
            throw new NotImplementedException();
        }
    }*/
}

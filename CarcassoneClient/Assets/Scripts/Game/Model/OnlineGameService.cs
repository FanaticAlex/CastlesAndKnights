using Assets.Scripts.Game;
using Carcassone.ApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using UnityEngine;

namespace Assets.Scripts
{
    internal class OnlineGameService : IGameService
    {
        private Client client;
        private HttpClient _httpClient;
        private string RoomId;

        public OnlineGameService()
        {
            _httpClient = new HttpClient() { Timeout = new TimeSpan(0, 0, 1) };
            client = new Client(@"http://192.168.1.65:82/", _httpClient);
            //client = new Client(@"https://localhost:7170/", _httpClient);


            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
        }

        public string User { get; private set; }

        public void Create() => RoomId = client.CreateAsync().Result.Id;
        public void Connect(string roomId) => RoomId = client.RoomGETAsync(roomId).Result.Id;

        public void Login(SavedAuthData data)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", data.Token);
            client.StatisticAsync(data.Login).Wait();

            User = data.Login;
        }

        public void Login(string login, string password)
        {
            var tokenResult = client.LoginAsync(login, password).Result;
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenResult.Token);
            User = login;

            CarcassonePrefs.SetSavedAuthData(login, tokenResult.Token);
        }

        public void AddHuman(string userName) => client.AddHumanAsync(RoomId, userName).Wait();
        public void AddAI() => client.AddAIAsync(RoomId).Wait();
        public void Start() => client.StartAsync(RoomId).Wait();
        public void EndTurn(string userName) => client.EndTurnAsync(RoomId, userName);

        public GameRoom GetRoom() => client.RoomGETAsync(RoomId).Result;
        public List<string> GetRoomsIds() => client.ListAsync().Result.ToList();

        public BasePlayer GetPlayer(string playerName) => client.PlayerGETAsync(RoomId, playerName).Result;
        public List<BasePlayer> GetPlayers() => client.List2Async(RoomId).Result.ToList();
        public BasePlayer GetCurrentPlayer()
        {
            try
            {
                return client.CurrentAsync(RoomId).Result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Card GetCurrentCard() => client.Current2Async(RoomId).Result;
        public List<Card> GetCards() => client.List3Async(RoomId).Result.ToList();
        public List<Card> GetActiveCards() => client.ActiveAsync(RoomId).Result.ToList();
        public Card GetCard(string cardName) => client.CardAsync(RoomId, cardName).Result;
        public bool CanPutCard(string fieldId, string cardName) => client.CanPutCardAsync(RoomId, fieldId, cardName).Result;
        public void PutCard(string fieldId, string cardName, string userName) => client.PutCardInFieldAsync(RoomId, fieldId, cardName, userName).Wait();
        public void RotateCard(string cardName) => client.RotateCardAsync(RoomId, cardName).Wait();

        public ObjectPart GetObjectPart(string partId) => client.ObjectPartAsync(RoomId, partId).Result;

        public List<Field> GetFields() => client.AllAsync(RoomId).Result.ToList();
        public List<Field> GetAvailableFields(string cardName) => client.AvailableFieldsAsync(RoomId, cardName).Result.ToList();
        public List<Field> GetNotAvailableFields() => client.NotAvailableFieldsAsync(RoomId).Result.ToList();

        public List<ObjectPart> GetAvailableObjectParts(string cardId) => client.AvailablePartsAsync(RoomId, cardId).Result.ToList();
        public List<ObjectPart> GetActiveParts() => client.Active2Async(RoomId).Result.ToList();
        public void PutChip(string cardName, string partId, string playerName) => client.PutChipInCardAsync(RoomId, cardName, partId, playerName).Wait();

        public List<UserGameScore> GetGameScores() => client.GameAsync(RoomId).Result.ToList();
        public PlayerScore GetScore(string playerName) => client.ScoreAsync(RoomId, playerName).Result;
        public List<Road> GetRoads() => client.RoadsAsync(RoomId).Result.ToList();
        public List<Castle> GetCastles() => client.CastlesAsync(RoomId).Result.ToList();
        public List<Cornfield> GetCornfields() => client.CornfieldsAsync(RoomId).Result.ToList();
        public List<Church> GetChurches() => client.ChurchesAsync(RoomId).Result.ToList();

        public int GetCardsRemain() => client.RemainAsync(RoomId).Result;

        public void Reset()
        {
            User = null;
            RoomId = null;
        }

        public UserStatistic GetUserStatistic(string userName) => client.StatisticAsync(userName).Result;
    }
}

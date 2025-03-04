using Assets.Scripts.Game;
using Carcassone.Core;
using Carcassone.Core.Calculation;
using Carcassone.Core.Calculation.Objects;
using Carcassone.Core.Cards;
using Carcassone.Core.Fields;
using Carcassone.Core.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    /*internal class OnlineGameService : IGameService, IOnlineGame
    {
        private readonly Carcassone.ApiClient.Client _client;
        private readonly HttpClient _httpClient;
        private string RoomId;

        public OnlineGameService()
        {
            _httpClient = new HttpClient() { Timeout = new TimeSpan(0, 0, 0, 0, 500) };
            //_client = new Client(@"http://192.168.1.68:32772/", _httpClient);
            _client = new Carcassone.ApiClient.Client(@"http://192.168.1.68:32774/", _httpClient);

            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
        }

        public string HumanUser { get; set; }

        public void Create() => RoomId = _client.CreateAsync().Result.Id;
        public void Connect(string roomId) => RoomId = _client.RoomGETAsync(roomId).Result.Id;

        public void Login(SavedAuthData data)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", data.Token);
            var cancelSource = new CancellationTokenSource(500);
            _client.StatisticAsync(data.Login, cancelSource.Token).Wait();
            HumanUser = data.Login;
        }

        public void Login(string login, string password)
        {
            var tokenResult = _client.LoginAsync(login, password).Result;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenResult.Token);
            HumanUser = login;
            CarcassonePrefs.SetSavedAuthData(login, tokenResult.Token);
        }

        public void AddPlayer(string userName, PlayerType type) => _client.AddAsync(RoomId, userName, type).Wait();
        public void DeletePlayer(string userName) { _client.PlayerDELETEAsync(RoomId, userName).Wait(); }
        public void Start() => _client.StartAsync(RoomId).Wait();
        public void EndTurn(string userName) => _client.EndTurnAsync(RoomId, userName).Wait();

        public GameRoom GetRoom() => _client.RoomGETAsync(RoomId).Result;
        public List<string> GetRoomsIds() => _client.ListAsync().Result.ToList();

        public BasePlayer GetPlayer(string playerName) => _client.PlayerGETAsync(RoomId, playerName).Result;
        public List<BasePlayer> GetPlayers() => _client.List2Async(RoomId).Result.ToList();
        public Task<BasePlayer> GetCurrentPlayer()
        {
            try
            {
                return _client.CurrentAsync(RoomId);
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException is ApiException apiex)
                {
                    if (apiex.StatusCode == 204)
                        return null;
                }

                throw ex;
            }
        }

        public Card GetCurrentCard()
        {
            try
            {
                return _client.Current2Async(RoomId).Result;
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException is ApiException apiex)
                {
                    if (apiex.StatusCode == 204)
                        return null;
                }

                throw ex;
            }
        }

        public List<Card> GetCards() => _client.List3Async(RoomId).Result.ToList();
        public Card GetCard(string cardName) => _client.CardAsync(RoomId, cardName).Result;
        public bool CanPutCard(string fieldId, string cardName) => _client.CanPutCardAsync(RoomId, fieldId, cardName).Result;
        public void PutCard(string fieldId, string cardName, string userName) => _client.PutCardInFieldAsync(RoomId, fieldId, cardName, userName).Wait();
        public void RotateCard(string cardName) => _client.RotateCardAsync(RoomId, cardName).Wait();

        public List<Field> GetFields() => _client.AllAsync(RoomId).Result.ToList();
        public List<Field> GetAvailableFields(string cardName) => _client.AvailableFieldsAsync(RoomId, cardName).Result.ToList();
        public List<Field> GetNotAvailableFields() => _client.NotAvailableFieldsAsync(RoomId).Result.ToList();

        public List<ObjectPart> GetAvailableObjectParts(string cardId) => _client.AvailablePartsAsync(RoomId, cardId).Result.ToList();
        public List<ObjectPart> GetActiveParts() => _client.ActiveAsync(RoomId).Result.ToList();
        public void PutChip(string cardName, string partId, string playerName) => _client.PutChipInCardAsync(RoomId, cardName, partId, playerName).Wait();

        public List<PlayerScore> GetGameScores() => _client.GameAsync(RoomId).Result.ToList();
        public PlayerScore GetScore(string playerName) => _client.ScoreAsync(RoomId, playerName).Result;
        public List<Road> GetRoads() => _client.RoadsAsync(RoomId).Result.ToList();
        public List<Castle> GetCastles() => _client.CastlesAsync(RoomId).Result.ToList();
        public List<Cornfield> GetCornfields() => _client.CornfieldsAsync(RoomId).Result.ToList();
        public List<Church> GetChurches() => _client.ChurchesAsync(RoomId).Result.ToList();

        public int GetCardsRemain() => _client.RemainAsync(RoomId).Result;

        public void Reset()
        {
            HumanUser = string.Empty;
            RoomId = null;
        }

        public UserInfo GetUserInfo(string userName) => _client.StatisticAsync(userName).Result;
    }*/
}

using Assets.Scripts.Game;
using Carcassone.ApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    internal class OnlineGameService : IGameService
    {
        private Client client;
        private HttpClient _httpClient;
        private string RoomId;

        public OnlineGameService()
        {
            _httpClient = new HttpClient() { Timeout = new TimeSpan(0, 0, 0, 0, 500) };
            //client = new Client(@"http://192.168.1.65:82/", _httpClient);
            //client = new Client(@"https://localhost:7170/", _httpClient);
            client = new Client(@"http://localhost:32772/", _httpClient);

            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
        }

        public List<string> HumanUsers { get; set; } = new List<string>();

        public void Create() => RoomId = client.CreateAsync().Result.Id;
        public void Connect(string roomId) => RoomId = client.RoomGETAsync(roomId).Result.Id;

        public void Login(SavedAuthData data)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", data.Token);
            var cancelSource = new CancellationTokenSource(500);
            client.StatisticAsync(data.Login, cancelSource.Token).Wait();

            HumanUsers.Add(data.Login);
        }

        public void Login(string login, string password)
        {
            var tokenResult = client.LoginAsync(login, password).Result;
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenResult.Token);
            HumanUsers.Add(login);

            CarcassonePrefs.SetSavedAuthData(login, tokenResult.Token);
        }

        public void AddPlayer(string userName, PlayerType type) => client.AddAsync(RoomId, userName, type).Wait();
        public void DeletePlayer(string userName) { client.PlayerDELETEAsync(RoomId, userName).Wait(); }
        public void Start() => client.StartAsync(RoomId).Wait();
        public void EndTurn(string userName) => client.EndTurnAsync(RoomId, userName).Wait();

        public GameRoom GetRoom() => client.RoomGETAsync(RoomId).Result;
        public List<string> GetRoomsIds() => client.ListAsync().Result.ToList();

        public BasePlayer GetPlayer(string playerName) => client.PlayerGETAsync(RoomId, playerName).Result;
        public List<BasePlayer> GetPlayers() => client.List2Async(RoomId).Result.ToList();
        public Task<BasePlayer> GetCurrentPlayer()
        {
            try
            {
                return client.CurrentAsync(RoomId);
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
                return client.Current2Async(RoomId).Result;
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
            HumanUsers.Clear();
            RoomId = null;
        }

        public UserStatistic GetUserStatistic(string userName) => client.StatisticAsync(userName).Result;
    }
}

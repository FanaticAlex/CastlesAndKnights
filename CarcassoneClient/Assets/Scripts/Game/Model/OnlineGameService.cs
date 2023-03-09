using Carcassone.ApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    internal class OnlineGameService : IGameService
    {
        private Client client;
        private string RoomId;

        public OnlineGameService()
        {
            var httpClient = new System.Net.Http.HttpClient() { Timeout = new TimeSpan(0, 0, 1) };
            //client = new Client(@"https://192.168.1.65:443/", httpClient);
            client = new Client(@"http://192.168.1.65:81/", httpClient);
            //client = new Client(@"https://localhost:44322/", httpClient);

            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
        }

        public User User { get; private set; }

        public void Create() => RoomId = client.CreateAsync().Result.Id;
        public void Connect(string roomId) => RoomId = client.RoomGETAsync(roomId).Result.Id;
        public void Login(string login, string password) => User = client.LoginAsync(login, password).Result;
        public void AddHuman(string userName) => client.AddHumanAsync(RoomId, userName).Wait();
        public void AddAI() => client.AddAIAsync(RoomId).Wait();
        public void Start() => client.StartAsync(RoomId).Wait();
        public void EndTurn(string userName) => client.EndTurnAsync(RoomId, userName).Wait();

        public GameRoom GetRoom() => client.RoomGETAsync(RoomId).Result;
        public List<string> GetRoomsIds() => client.ListAsync().Result.ToList();

        public BasePlayer GetPlayer(string playerName) => client.PlayerGETAsync(RoomId, playerName).Result;
        public List<BasePlayer> GetPlayers() => client.List2Async(RoomId).Result.ToList();
        public BasePlayer GetCurrentPlayer() => client.CurrentAsync(RoomId).Result;

        public Card GetCurrentCard() => client.Current2Async(RoomId).Result;
        public List<Card> GetCards() => client.List3Async(RoomId).Result.ToList();
        public Card GetCard(string cardName) => client.CardAsync(RoomId, cardName).Result;
        public bool CanPutCard(string fieldId, string cardName) => client.CanPutCardAsync(RoomId, fieldId, cardName).Result;
        public void PutCard(string fieldId, string cardName, string userName) => client.PutCardInFieldAsync(RoomId, fieldId, cardName, userName).Wait();
        public void RotateCard(string cardName) => client.RotateCardAsync(RoomId, cardName).Wait();

        public List<Field> GetFields() => client.AllAsync(RoomId).Result.ToList();
        public List<Field> GetAvailableFields(string cardName) => client.AvailableFieldsAsync(RoomId, cardName).Result.ToList();
        public List<Field> GetNotAvailableFields() => client.NotAvailableFieldsAsync(RoomId).Result.ToList();

        public List<ObjectPart> GetAvailableObjectParts(string cardId) => client.AvailablePartsAsync(RoomId, cardId).Result.ToList();
        public void PutChip(string cardName, string partId, string playerName) => client.PutChipInCardAsync(RoomId, cardName, partId, playerName).Wait();

        public List<GameScore> GetGameScores() => client.GameAsync(RoomId).Result.ToList();
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
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carcassone.Core;
using Carcassone.Core.Calculation;
using Carcassone.Core.Calculation.Objects;
using Carcassone.Core.Cards;
using Carcassone.Core.Fields;
using Carcassone.Core.Players;
using Carcassone.DAL;
using Carcassone.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CarcassoneServer.Controllers
{
    /// <summary>
    /// Контроллер игры.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class RoomController : ControllerBase
    {
        private readonly IGamesService _service;
        private readonly ILogger _logger;
        private static IConfiguration _configuration;

        public RoomController(
            ILogger<RoomController> logger,
            IGamesService service,
            IConfiguration configuration)
        {
            _logger = logger;
            _service = service;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("create")]
        public GameRoom CreateRoom()
        {
            var room = _service.CreateRoom();
            room.Finished += Room_Finished;
            _logger.LogInformation("RoomCreated");
            return room;
        }

        private void Room_Finished(object sender, GameRoom room)
        {
            _logger.LogInformation("Game Finished");
            SaveGameResults(room);
            Task.Run(async () => await RemoveGame(room.Id));
        }

        private async Task RemoveGame(string roomId)
        {
            await Task.Delay(TimeSpan.FromSeconds(30));
            _service.DeleteRoom(roomId);
        }

        private void SaveGameResults(GameRoom room)
        {
            // записать результаты в базу
            foreach (var player in room.GetPlayers())
            {
                if (player is not Player) // не записываем в базу результаты AI игроков
                    continue;

                var playerScore = room.GetPlayerScore(player);
                var userScore = new UserGameScore()
                {
                    UserName = player.Name,
                    RoomId = room.Id,
                    FinalScore = playerScore.GetOverallScore(),
                    Rank = playerScore.Rank
                };

                var optionsBuilder = new DbContextOptionsBuilder<CarcassoneContext>();
                optionsBuilder.UseSqlite(_configuration["DbConnectionString"]);
                var context = new CarcassoneContext(optionsBuilder.Options);
                var scoreService = new GameScoreService(context);
                scoreService.SaveUserGameScore(userScore);
            }
        }

        [HttpGet]
        [Route("list")]
        public List<string> GetRoomsList() => _service.GetAvailableRoomsId();

        [HttpDelete]
        [Route("{roomId}")]
        public void DeleteRoom(string roomId) => _service.DeleteRoom(roomId);

        [HttpGet]
        [Route("{roomId}")]
        public GameRoom GetRoom(string roomId) => _service.GetRoom(roomId);

        [HttpGet]
        [Route("{roomId}/start")]
        public void Start(string roomId) => _service.GetRoom(roomId).Start();

        [HttpGet]
        [Route("{roomId}/availableFields/{cardId}")]
        public List<Field> GetAvailableFields(string roomId, string cardId) => _service.GetRoom(roomId).GetAvailableFields(cardId);

        [HttpGet]
        [Route("{roomId}/notAvailableFields")]
        public List<Field> GetNotAvailableFields(string roomId) => _service.GetRoom(roomId).GetNotAvailableFields();


        [HttpGet]
        [Route("{roomId}/field/all")]
        public List<Field> GetFields(string roomId) => _service.GetRoom(roomId).GetFields();

        [HttpGet]
        [Route("{roomId}/field/{fieldId}")]
        public Field GetField(string roomId, string fieldId) => _service.GetRoom(roomId).GetField(fieldId);



        [HttpGet]
        [Route("{roomId}/canPutCard/{fieldId}/{cardName}")]
        public bool CanPutCard(string roomId, string fieldId, string cardName)
        {
            return _service.GetRoom(roomId).CanPutCard(fieldId, cardName);
        }

        [HttpGet]
        [Route("{roomId}/putCardInField/{fieldId}/{cardName}/{playerName}")]
        public void PutCardInField(string roomId, string fieldId, string cardName, string playerName)
        {
            var room = _service.GetRoom(roomId);
            var human = GetHumanPlayer(room, playerName);
            human.SetPlayerMove1(room, cardName, fieldId);
        }

        [HttpGet]
        [Route("{roomId}/putChipInCard/{cardName}/{partId}/{playerName}")]
        public void PutChipInCard(string roomId, string cardName, string partId, string playerName)
        {
            var room = _service.GetRoom(roomId);
            var human = GetHumanPlayer(room, playerName);
            human.SetPlayerMove2(room, cardName, partId);
        }

        [HttpGet]
        [Route("{roomId}/endTurn/{playerName}")]
        public void EndTurn(string roomId, string playerName)
        {
            var room = _service.GetRoom(roomId);
            var human = GetHumanPlayer(room, playerName);
            human.SetPlayerMove3(room);
        }

        private Player GetHumanPlayer(GameRoom room, string playerName)
        {
            var player = room.GetCurrentPlayer();
            if (player.Name != playerName)
                throw new Exception($"Its '{player.Name}' turn!");

            if (player is not Player)
                throw new Exception($"Its AI '{player.Name}' turn!");

            return ((Player)player);
        }

        [HttpGet]
        [Route("{roomId}/roads")]
        public List<Road> GetRoads(string roomId) => _service.GetRoom(roomId).GetRoads();

        [HttpGet]
        [Route("{roomId}/castles")]
        public List<Castle> GetCastles(string roomId) => _service.GetRoom(roomId).GetCastles();

        [HttpGet]
        [Route("{roomId}/churches")]
        public List<Church> GetChurches(string roomId) => _service.GetRoom(roomId).GetChurches();

        [HttpGet]
        [Route("{roomId}/cornfields")]
        public List<Cornfield> GetCornfields(string roomId) => _service.GetRoom(roomId).GetCornfields();



        [HttpPost]
        [Route("{roomId}/player/addAI")]
        public void AddAIPlayer(string roomId) => _service.GetRoom(roomId).AddAIPlayer();

        [HttpPost]
        [Route("{roomId}/player/addHuman")]
        public void AddHumanPlayer(string roomId, string playerName) => _service.GetRoom(roomId).AddHumanPlayer(playerName);

        [HttpGet]
        [Route("{roomId}/player/list")]
        public List<BasePlayer> GetPlayersList(string roomId) => _service.GetRoom(roomId).GetPlayers();

        [HttpGet]
        [Route("{roomId}/player/current")]
        public BasePlayer GetCurrentPlayer(string roomId) => _service.GetRoom(roomId).GetCurrentPlayer();

        [HttpGet]
        [Route("{roomId}/player/{playerName}")]
        public BasePlayer GetPlayer(string roomId, string playerName) => _service.GetRoom(roomId).GetPlayer(playerName);

        [HttpGet]
        [Route("{roomId}/player/{playerName}/score")]
        public PlayerScore Score(string roomId, string playerName)
        {
            var room = _service.GetRoom(roomId);
            var player = room.GetPlayer(playerName);
            var score = room.GetPlayerScore(player);
            return score;
        }

        [HttpDelete]
        [Route("{roomId}/player/{playerName}")]
        public void Delete(string roomId, string playerName) => _service.GetRoom(roomId).DeletePlayer(playerName);



        [HttpGet]
        [Route("{roomId}/card/{cardId}")]
        public Card GetCard(string roomId, string cardId) => _service.GetRoom(roomId).GetCard(cardId);

        [HttpGet]
        [Route("{roomId}/card/list")]
        public List<Card> GetAllCards(string roomId) => _service.GetRoom(roomId).GetAllCards();

        [HttpGet]
        [Route("{roomId}/card/remain")]
        public int GetCardsRemain(string roomId) => _service.GetRoom(roomId).GetCardsRemain();

        [HttpGet]
        [Route("{roomId}/card/{cardId}/availableParts")]
        public List<ObjectPart> GetAvailableParts(string roomId, string cardId) => _service.GetRoom(roomId).GetAvailableParts(cardId);

        [HttpGet]
        [Route("{roomId}/card/current")]
        public Card GetCurrentCard(string roomId) => _service.GetRoom(roomId).GetCurrentCard();

        [HttpGet]
        [Route("{roomId}/card/rotateCard/{cardId}")]
        public void RotateCard(string roomId, string cardId) => _service.GetRoom(roomId).RotateCard(cardId);
    }
}

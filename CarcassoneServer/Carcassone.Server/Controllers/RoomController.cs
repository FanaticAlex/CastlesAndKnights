using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carcassone.Core;
using Carcassone.Core.Calculation;
using Carcassone.Core.Calculation.Objects;
using Carcassone.Core.Cards;
using Carcassone.Core.Fields;
using Carcassone.Core.Players;
using Carcassone.DAL;
using Carcassone.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CarcassoneServer.Controllers
{
    /// <summary>
    /// Контроллер игры.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IGamesService _service;
        private readonly IGameScoreService _scoreService;

        public RoomController(IGamesService service, IGameScoreService scoreService)
        {
            _service = service;
            _scoreService = scoreService;
        }

        [HttpPost]
        [Route("create")]
        public GameRoom CreateRoom() => _service.CreateRoom();

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
        [Route("{roomId}/endTurn")]
        public void EndTurn(string roomId)
        {
            var room = _service.GetRoom(roomId);
            room.EndTurn();
            if (room.IsFinished)
            {
                WriteGameResults(room);
                Task.Run(async () => await RemoveGame(room.Id));
            }
        }

        private async Task RemoveGame(string roomId)
        {
            await Task.Delay(TimeSpan.FromSeconds(30));
            _service.DeleteRoom(roomId);
        }

        private void WriteGameResults(GameRoom room)
        {
            // записать результаты в базу
            foreach (var player in room.GetPlayers())
            {
                var playerScore = room.GetPlayerScore(player);
                var userScore = new GameScore();
                userScore.UserName = player.Name;
                userScore.RoomId = room.Id;
                userScore.FinalScore = playerScore.GetOverallScore();
                _scoreService.WriteUserScore(userScore);
            }
        }

        [HttpGet]
        [Route("{roomId}/field/all")]
        public List<Field> GetFields(string roomId) => _service.GetRoom(roomId).GetFields();

        [HttpGet]
        [Route("{roomId}/field/{fieldId}")]
        public Field GetField(string roomId, string fieldId) => _service.GetRoom(roomId).GetField(fieldId);



        [HttpGet]
        [Route("{roomId}/canPutCard/{fieldId}/{cardName}")]
        public bool CanPutCard(string roomId, string fieldId, string cardName) => _service.GetRoom(roomId).CanPutCard(fieldId, cardName);

        [HttpGet]
        [Route("{roomId}/putChipInCard/{cardName}/{partId}/{playerName}")]
        public void PutChipInCard(string roomId, string cardName, string partId, string playerName) => _service.GetRoom(roomId).PutChipInCard(cardName, partId, playerName);

        [HttpGet]
        [Route("{roomId}/putCardInField/{fieldId}/{cardName}")]
        public void PutCardInField(string roomId, string fieldId, string cardName) => _service.GetRoom(roomId).PutCardInField(fieldId, cardName);

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
        public List<Player> GetPlayersList(string roomId) => _service.GetRoom(roomId).GetPlayers();

        [HttpGet]
        [Route("{roomId}/player/current")]
        public Player GetCurrentPlayer(string roomId) => _service.GetRoom(roomId).GetCurrentPlayer();

        [HttpGet]
        [Route("{roomId}/player/{playerName}")]
        public Player GetPlayer(string roomId, string playerName) => _service.GetRoom(roomId).GetPlayer(playerName);

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

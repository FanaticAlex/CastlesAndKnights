using Carcassone.Core;
using Carcassone.Core.Calculation;
using Carcassone.Core.Calculation.Objects;
using Carcassone.Core.Cards;
using Carcassone.Core.Fields;
using Carcassone.Core.Players;
using Carcassone.Web.Blazor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarcassoneServer.Web.Blazor.Controllers
{
    public class GameRoomDto
    {
        public string Id { get; set; }
        public bool IsStarted { get; set; }
        public bool IsFinished { get; set; }
    }

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
        private readonly IConfiguration _configuration;
        private readonly PlayedGameStore _playedGameStore;

        public RoomController(
            ILogger<RoomController> logger,
            IGamesService service,
            IConfiguration configuration,
            PlayedGameStore playedGameStore)
        {
            _logger = logger;
            _service = service;
            _configuration = configuration;
            _playedGameStore = playedGameStore;
        }

        [HttpPost]
        [Route("create")]
        public GameRoomDto CreateRoom()
        {
            _logger.LogInformation("Called CreateRoom");
            var room = _service.CreateRoom();
            return new GameRoomDto() { Id = room.Id, IsFinished = room.IsFinished, IsStarted = room.IsStarted };
        }

        [HttpGet]
        [Route("list")]
        public List<string> GetRoomsList() => _service.GetAvailableRoomsId();

        [HttpDelete]
        [Route("{roomId}")]
        public void DeleteRoom(string roomId) => _service.DeleteRoom(roomId);

        [HttpGet]
        [Route("{roomId}")]
        public GameRoomDto GetRoom(string roomId)
        {
            var room = _service.GetRoom(roomId);
            return new GameRoomDto() { Id = room.Id, IsFinished = room.IsFinished, IsStarted = room.IsStarted };
        }

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
        public List<Field> GetFields(string roomId) => _service.GetRoom(roomId).FieldBoard.Fields.ToList();

        [HttpGet]
        [Route("{roomId}/field/{fieldId}")]
        public Field GetField(string roomId, string fieldId) => _service.GetRoom(roomId).FieldBoard.GetField(fieldId);



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
            var human = room.PlayersPool.GetHumanPlayer(room, playerName);
            human.SetPlayerMove1(room, cardName, fieldId);
        }

        [HttpGet]
        [Route("{roomId}/putChipInCard/{cardName}/{partId}/{playerName}")]
        public void PutChipInCard(string roomId, string cardName, string partId, string playerName)
        {
            var room = _service.GetRoom(roomId);
            var human = room.PlayersPool.GetHumanPlayer(room, playerName);
            human.SetPlayerMove2(room, cardName, partId);
        }

        [HttpGet]
        [Route("{roomId}/endTurn/{playerName}")]
        public IActionResult EndTurn(string roomId, string playerName)
        {
            var room = _service.GetRoom(roomId);
            var human = room.PlayersPool.GetHumanPlayer(room, playerName);
            human.SetPlayerMove3(room);

            // AI players move
            var task = Task.Run(() => room.AllAiPlayersMove());
            task.ContinueWith((obj) => FinishingTheGame(room));

            return Ok();
        }

        private void FinishingTheGame(GameRoom room)
        {
            if (room.IsFinished)
            {
                _logger.LogInformation("Game Finished");
                _playedGameStore.AddGameResults(room);
            }
        }

        [HttpGet]
        [Route("{roomId}/roads")]
        public List<Road> GetRoads(string roomId) => _service.GetRoom(roomId).ScoreCalculator.Roads;

        [HttpGet]
        [Route("{roomId}/castles")]
        public List<Castle> GetCastles(string roomId) => _service.GetRoom(roomId).ScoreCalculator.Castles;

        [HttpGet]
        [Route("{roomId}/churches")]
        public List<Church> GetChurches(string roomId) => _service.GetRoom(roomId).ScoreCalculator.Churches;

        [HttpGet]
        [Route("{roomId}/cornfields")]
        public List<Cornfield> GetCornfields(string roomId) => _service.GetRoom(roomId).ScoreCalculator.Cornfields;



        [HttpPost]
        [Route("{roomId}/player/add")]
        public void AddPlayer(string roomId, string playerName, PlayerType type) => _service.GetRoom(roomId).PlayersPool.AddPlayer(playerName, type);

        [HttpGet]
        [Route("{roomId}/player/list")]
        public List<BasePlayer> GetPlayersList(string roomId) => _service.GetRoom(roomId).PlayersPool.Players;

        [HttpGet]
        [Route("{roomId}/player/current")]
        public Task<BasePlayer?> GetCurrentPlayer(string roomId) => Task.FromResult(_service.GetRoom(roomId).PlayersPool.GetCurrentPlayer());

        [HttpGet]
        [Route("{roomId}/player/{playerName}")]
        public BasePlayer GetPlayer(string roomId, string playerName) => _service.GetRoom(roomId).PlayersPool.GetPlayer(playerName);

        [HttpGet]
        [Route("{roomId}/player/{playerName}/score")]
        public PlayerScore Score(string roomId, string playerName)
        {
            var room = _service.GetRoom(roomId);
            var player = room.PlayersPool.GetPlayer(playerName);
            var score = room.GetPlayerScore(player);
            return score;
        }

        [HttpDelete]
        [Route("{roomId}/player/{playerName}")]
        public void Delete(string roomId, string playerName) => _service.GetRoom(roomId).PlayersPool.DeletePlayer(playerName);



        [HttpGet]
        [Route("{roomId}/card/{cardId}")]
        public Card GetCard(string roomId, string cardId) => _service.GetRoom(roomId).GetCard(cardId);

        [HttpGet]
        [Route("{roomId}/card/list")]
        public List<Card> GetAllCards(string roomId) => _service.GetRoom(roomId).CardsPool.AllCards;

        [HttpGet]
        [Route("{roomId}/card/remain")]
        public int GetCardsRemain(string roomId) => _service.GetRoom(roomId).GetCardsRemain();

        [HttpGet]
        [Route("{roomId}/card/{cardId}/availableParts")]
        public List<ObjectPart> GetAvailableParts(string roomId, string cardId) => _service.GetRoom(roomId).GetAvailableParts(cardId);

        [HttpGet]
        [Route("{roomId}/card/current")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Card))]
        public Task<Card?> GetCurrentCard(string roomId) => Task.FromResult(_service.GetRoom(roomId).GetCurrentCard());

        [HttpGet]
        [Route("{roomId}/card/rotateCard/{cardId}")]
        public void RotateCard(string roomId, string cardId) => _service.GetRoom(roomId).RotateCard(cardId);


        [HttpGet]
        [Route("{roomId}/objectPart/active")]
        public List<ObjectPart> GetActiveParts(string roomId) => _service.GetRoom(roomId).GetActiveParts();
    }
}

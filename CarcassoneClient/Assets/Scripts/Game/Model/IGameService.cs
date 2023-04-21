using Assets.Scripts.Game;
using Carcassone.ApiClient;
using System.Collections.Generic;

namespace Assets.Scripts
{
    /// <summary>
    /// Provide functons to access game logic
    /// </summary>
    internal interface IGameService
    {
        string User { get; }

        void Login(SavedAuthData data); // этот метод не подхдит интерфейсу он только для сетевой игры
        void Login(string login, string password); // этот метод не подхдит интерфейсу он только для сетевой игры
        void Create();
        void Connect(string roomId); // этот метод не подхдит интерфейсу он только для сетевой игры
        void AddHuman(string userName);
        void AddAI();
        void Start();
        void EndTurn(string userName);

        GameRoom GetRoom();
        List<string> GetRoomsIds(); // этот метод не подхдит интерфейсу он только для сетевой игры

        BasePlayer GetPlayer(string playerName);
        List<BasePlayer> GetPlayers();
        BasePlayer GetCurrentPlayer();

        Card GetCurrentCard();
        List<Card> GetCards();
        List<Card> GetActiveCards();
        Card GetCard(string cardName);
        ObjectPart GetObjectPart(string partId);
        bool CanPutCard(string fieldId, string cardName);
        void PutCard(string fieldId, string cardName, string userName);
        void RotateCard(string cardName);

        List<Field> GetFields();
        List<Field> GetAvailableFields(string cardName);
        List<Field> GetNotAvailableFields();

        List<ObjectPart> GetAvailableObjectParts(string cardId);
        void PutChip(string cardName, string partId, string playerName);

        List<UserGameScore> GetGameScores();
        PlayerScore GetScore(string playerName);
        List<Road> GetRoads();
        List<Castle> GetCastles();
        List<Cornfield> GetCornfields();
        List<Church> GetChurches();

        int GetCardsRemain();

        void Reset();

        UserStatistic GetUserStatistic(string userName);
    }
}

using Carcassone.Core;
using Carcassone.Core.Cards;
using Carcassone.Core.Players;
using Xunit;

namespace Carcassone.Core.Tests.Room
{
    public class GameRoomTest
    {
        [Fact]
        public void Workflow()
        {
            var room = new GameRoom();
            var jack = room.AddHumanPlayer("jack");
            var bob = room.AddHumanPlayer("bob");
            room.AddAIPlayer();
            room.Start();

            Card? newCard;
            Player? currentPlayer = null;
            while (true)
            {
                currentPlayer = GetNextPlayer(jack, bob, currentPlayer);
                newCard = room.GetCurrentCard();
                if (room.IsFinished)
                    break;

                PlayerMakeMove(newCard, room, currentPlayer);
            }

            var jackScore = room.GetPlayerScore(jack);
            var bobScore = room.GetPlayerScore(bob);

            Assert.NotEqual(0, jackScore.GetOverallScore());
            Assert.NotEqual(0, bobScore.GetOverallScore());
        }

        private static void PlayerMakeMove(Card newCard, GameRoom room, Player player)
        {
            var availableFields = room.GetAvailableFields(newCard.CardName);
            var choosenField = availableFields[0];
            choosenField.RotateCardTilFit(newCard);
            room.PutCardInField(newCard, choosenField);

            var availableParts = room.GetAvailableParts(newCard.CardName);
            if (availableParts.Count > 0)
            {
                var choosenPart = availableParts[0];
                room.PutChipInCard(newCard.CardName, choosenPart.PartId, player.Name);
            }
            room.EndTurn();
        }

        private Player GetNextPlayer(Player jack, Player bob, Player? currentPlayer)
        {
            Player newCurrentPlayer;
            if (currentPlayer == jack)
            {
                newCurrentPlayer = bob;
                return newCurrentPlayer;
            }

            if (currentPlayer == bob)
            {
                newCurrentPlayer = jack;
                return newCurrentPlayer;
            }

            if (currentPlayer == null)
            {
                newCurrentPlayer = jack;
                return newCurrentPlayer;
            }

            throw new System.Exception("не найден игрок");
        }
    }
}
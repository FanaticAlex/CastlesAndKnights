using Carcassone.Core.Calculation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace Carcassone.Core.Players
{
    public enum PlayerColor
    {
        Blue,
        Gray,
        Green,
        Red,
        //White,
        Yellow
    }

    public class PlayerInfo
    {
        public PlayerInfo(string name, PlayerType playerType, PlayerColor color)
        {
            Name = name;
            PlayerType = playerType;
            Color = color;
        }

        public string Name { get; }
        public PlayerType PlayerType { get;  }
        public PlayerColor Color { get; }
    }

    /// <summary>
    /// Players proprties in particular room
    /// </summary>
    public class GamePlayer
    {
        public const int MeeplesMaxCount = 7; 

        public GamePlayer(PlayerInfo info)
        {
            Info = info;
        }

        public PlayerInfo Info { get; set; }

        public bool IsAIProcessing {  get; set; }

        public int GetPlayerOverallScore(MoveState state)
        {
            // score is a sum of scores of all owned objects
            var playerObjects = state.Calculated.Where(o => o.IsPlayerOwner(Info.Name, state));
            return playerObjects.Select(o => o.GetScore(state)).Sum();
        }

        public int GetMeeplesCount(MoveState state)
        {
            // Meeples count - is a count of used meeples placed on uncomplete objects

            var usedMeeples = 0;
            foreach (var obj in state.Calculated)
            {
                var completableObj = obj as ICompletableGameObject;
                if (completableObj != null)
                {
                    if (completableObj.IsComplete(state))
                        continue;
                }

                foreach (var part in obj.Parts)
                {
                    if (part.GetPartOwner(state) == Info.Name)
                        usedMeeples++;
                }
            }

            return MeeplesMaxCount - usedMeeples; // TODO Create test to check correct add remove and limit meeples
        }

        public void ProcessMove(GameRoom room)
        {
            if (room == null)
                throw new Exception("The room cannot be null.");

            IsAIProcessing = true;

            // where to put a card
            var currentTile = room.GetCurrentTile();
            List<GameMove> possibleMoves = room.GetAvailableMoves(currentTile.Id);
            GameMove bestMove = GetBestMove(room, possibleMoves);
            room.MakeMove(bestMove);

            IsAIProcessing = false;
        }

        private GameMove GetBestMove(GameRoom room, List<GameMove> possibleMoves)
        {
            if (possibleMoves == null || possibleMoves.Count == 0)
                throw new Exception("AI cant make a move. No possible moves.");

            var dic = new Dictionary<GameMove, Tuple<int,int>>();
            foreach (GameMove move in possibleMoves)
            {
                var newState = room.CalculateNewState(room.State, move);
                var expectedScore = GetPlayerOverallScore(newState);
                var meeplesCount = GetMeeplesCount(newState);
                dic.Add(move, new Tuple<int, int> (expectedScore, meeplesCount));
            }

            // ходы возвращающие фишки
            var currentMeeplesCount = GetMeeplesCount(room.State);
            var maxReturnMeeples = dic.Max(item => (item.Value.Item2 - currentMeeplesCount));
            var returnMeeplesMoves = dic.Where(item => (item.Value.Item2 - currentMeeplesCount) == maxReturnMeeples);
            if (maxReturnMeeples > 0 && returnMeeplesMoves.Count() > 0)
            {
                return returnMeeplesMoves.FirstOrDefault().Key;
            }

            // ходы дающие наибольшее число очков
            var maxScore = dic.Max(item => item.Value.Item1);
            var bestScoreMove = dic
                .Where(item => item.Value.Item1 == maxScore)
                .First();
            return bestScoreMove.Key;
        }
    }
}

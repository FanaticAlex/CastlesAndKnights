using Carcassone.Core.Tiles;
using Carcassone.Core.Board;
using Carcassone.Core.Players;
using System.Collections.Generic;
using System.Linq;
using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Calculation.Base.Monasteries;

namespace Carcassone.Core.Calculation
{
    /// <summary>
    /// Calculates game objects
    /// </summary>
    public class ScoreCalculator
    {
        public MultipartObjectsManager<City> CastlesManager { get; set; } = new MultipartObjectsManager<City>();
        public MultipartObjectsManager<Road> RoadsManager { get; set; } = new MultipartObjectsManager<Road>();
        public MultipartObjectsManager<Farm> FarmsManager { get; set; } = new MultipartObjectsManager<Farm>();
        public List<Monastery> Churches { get; set; } = new List<Monastery>();
        

        public ScoreCalculator()
        {

        }

        /// <summary>
        /// Добавляет карту и пересчитывает очки.
        /// </summary>
        /// <param name="card"></param>
        public void AddCard(Tile card, Cell cell, Grid grid, Stack cardPool)
        {
            foreach (ObjectPart part in card.Parts)
            {
                if (part is FieldPart)
                    FarmsManager.ProcessPart(part, cardPool);

                if (part is MonasteryPart)
                {
                    var church = new Monastery((MonasteryPart)part, cell, grid);
                    Churches.Add(church);
                }

                if (part is CityPart)
                    CastlesManager.ProcessPart(part, cardPool);

                if (part is RoadPart)
                    RoadsManager.ProcessPart(part, cardPool);
            }
        }

        public void CloseObjectsAndReturnChips(GamePlayersPool playersPool, Stack cardPool)
        {
            foreach (var church in Churches)
                church.TryToClose(playersPool, cardPool);

            foreach (var castle in CastlesManager.Objects)
                castle.TryToClose(playersPool, cardPool);

            foreach (var road in RoadsManager.Objects)
                road.TryToClose(playersPool, cardPool);

            foreach (var cornfield in FarmsManager.Objects)
                cornfield.RecalculatePartsOwner(cardPool);
        }

        public PlayerScore GetPlayerScore(string playerName, GamePlayersPool plyersPool, Stack cardPool)
        {
            var scores = GetPlayersScores(plyersPool, cardPool);
            var score = scores.Single(s => s.PlayerName == playerName);
            return score;
        }

        private IEnumerable<PlayerScore> GetPlayersScores(GamePlayersPool plyersPool, Stack cardPool)
        {
            var scores = new List<PlayerScore>();
            foreach (var player in plyersPool.GamePlayers)
            {
                var playerCastles = CastlesManager.Objects.Where(castle => castle.IsPlayerOwner(player, cardPool));
                var playerRoads = RoadsManager.Objects.Where(road => road.IsPlayerOwner(player, cardPool));
                var playerCornfields = FarmsManager.Objects.Where(cornfield => cornfield.IsPlayerOwner(player, cardPool));
                var playerChurches = Churches.Where(church => church.IsPlayerOwner(player, cardPool));

                var score = new PlayerScore()
                {
                    PlayerName = player.Name,
                    ChurchesScore = playerChurches.ToList().Sum(church => church.GetPoints()),
                    ChurchesCount = playerChurches.ToList().Count(),
                    CornfieldsScore = playerCornfields.ToList().Sum(cornfield => cornfield.GetPoints(CastlesManager.Objects, cardPool)),
                    CornfieldsCount = playerCornfields.ToList().Count(),
                    RoadsScore = playerRoads.ToList().Sum(road => road.GetPoints(cardPool)),
                    RoadsCount = playerRoads.ToList().Count(),
                    CastlesScore = playerCastles.ToList().Sum(castle => castle.GetPoints(cardPool)),
                    CastlesCount = playerCastles.ToList().Count(),
                    ChipCount = player.СhipList.Count
                };
                scores.Add(score);
            }

            scores.Sort(delegate (PlayerScore x, PlayerScore y)
            {
                return y.GetOverallScore().CompareTo(x.GetOverallScore());
            });

            foreach(var score  in scores)
            {
                score.Rank = scores.IndexOf(score);
            }

            return scores;
        }

        
    }
}


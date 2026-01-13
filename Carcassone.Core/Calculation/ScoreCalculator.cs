using Carcassone.Core.Tiles;
using Carcassone.Core.Board;
using Carcassone.Core.Players;
using System.Collections.Generic;
using System.Linq;
using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Calculation.Base.Monasteries;
using Carcassone.Core.Calculation.RiverExtension.Rivers;

namespace Carcassone.Core.Calculation
{
    /// <summary>
    /// Calculates game objects
    /// </summary>
    public class ScoreCalculator
    {
        public RiversManager RiversManager { get; set; } = new RiversManager();

        public MultipartObjectsManager<City> CitysManager { get; set; } = new MultipartObjectsManager<City>();
        public MultipartObjectsManager<Road> RoadsManager { get; set; } = new MultipartObjectsManager<Road>();
        public MultipartObjectsManager<Farm> FarmsManager { get; set; } = new MultipartObjectsManager<Farm>();
        public List<Monastery> Churches { get; set; } = new List<Monastery>();
        

        public ScoreCalculator()
        {

        }

        /// <summary>
        /// Добавляет карту и пересчитывает очки.
        /// </summary>
        /// <param name="tile"></param>
        public void AddCard(Tile tile, Cell cell, Grid grid, TileStack tileStack)
        {
            foreach (ObjectPart part in tile.Parts)
            {
                if (part is RiverPart)
                    RiversManager.ProcessPart(part, tileStack);

                if (part is FieldPart)
                    FarmsManager.ProcessPart(part, tileStack);

                if (part is MonasteryPart)
                {
                    var church = new Monastery((MonasteryPart)part, cell, grid);
                    Churches.Add(church);
                }

                if (part is CityPart)
                    CitysManager.ProcessPart(part, tileStack);

                if (part is RoadPart)
                    RoadsManager.ProcessPart(part, tileStack);
            }
        }

        public void CloseObjectsAndReturnChips(GamePlayersPool playersPool, TileStack cardPool)
        {
            foreach (var church in Churches)
                church.TryToClose(playersPool, cardPool);

            foreach (var City in CitysManager.Objects)
                City.TryToClose(playersPool, cardPool);

            foreach (var road in RoadsManager.Objects)
                road.TryToClose(playersPool, cardPool);

            foreach (var Farm in FarmsManager.Objects)
                Farm.RecalculatePartsOwner(cardPool);
        }

        public PlayerScore GetPlayerScore(string playerName, GamePlayersPool plyersPool, TileStack cardPool)
        {
            var scores = GetPlayersScores(plyersPool, cardPool);
            var score = scores.Single(s => s.PlayerName == playerName);
            return score;
        }

        private IEnumerable<PlayerScore> GetPlayersScores(GamePlayersPool plyersPool, TileStack cardPool)
        {
            var scores = new List<PlayerScore>();
            foreach (var player in plyersPool.GamePlayers)
            {
                var playerCitys = CitysManager.Objects.Where(City => City.IsPlayerOwner(player, cardPool));
                var playerRoads = RoadsManager.Objects.Where(road => road.IsPlayerOwner(player, cardPool));
                var playerFarms = FarmsManager.Objects.Where(Farm => Farm.IsPlayerOwner(player, cardPool));
                var playerChurches = Churches.Where(church => church.IsPlayerOwner(player, cardPool));

                var score = new PlayerScore()
                {
                    PlayerName = player.Name,
                    ChurchesScore = playerChurches.ToList().Sum(church => church.GetPoints()),
                    ChurchesCount = playerChurches.ToList().Count(),
                    FarmsScore = playerFarms.ToList().Sum(Farm => Farm.GetPoints(CitysManager.Objects, cardPool)),
                    FarmsCount = playerFarms.ToList().Count(),
                    RoadsScore = playerRoads.ToList().Sum(road => road.GetPoints(cardPool)),
                    RoadsCount = playerRoads.ToList().Count(),
                    CitysScore = playerCitys.ToList().Sum(City => City.GetPoints(cardPool)),
                    CitysCount = playerCitys.ToList().Count(),
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


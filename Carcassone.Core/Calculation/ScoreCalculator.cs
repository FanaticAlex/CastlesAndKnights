using Carcassone.Core.Calculation.Objects;
using Carcassone.Core.Tiles;
using Carcassone.Core.Board;
using Carcassone.Core.Players;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carcassone.Core.Calculation
{
    public class MultipartObjectsManager<T> where T : IMultipartObject
    {
        public List<T> Objects { get; set; } = new List<T>();

        public void ProcessPart(ObjectPart part, Stack cardPool)
        {
            var objectsToMerge = GetObjectsToConnectWithPart(part, Objects);
            var merged = Merge(part, objectsToMerge, cardPool);
            objectsToMerge.ForEach(c => Objects.Remove(c));
            Objects.Add(merged);
        }

        /// <summary>
        /// Если часть присоединяется к нескольким обьектам то их можно мержить
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="part"></param>
        /// <param name="objects"></param>
        /// <returns></returns>
        protected List<T> GetObjectsToConnectWithPart(ObjectPart part, List<T> objects) 
        {
            var objectsToMerge = new List<T>();
            foreach (var gameObject in objects)
            {
                if (gameObject.CanConnect(part))
                    objectsToMerge.Add(gameObject);
            }

            return objectsToMerge;
        }

        protected T Merge(ObjectPart connectingPart, IEnumerable<T> objectsToMerge, Stack cardPool)
        {
            if (objectsToMerge == null)
                throw new Exception("Failed to process merge objects. objectsToMerge shouldn't be null");

            // object contained new part
            var mergedObject = (T)Activator.CreateInstance(typeof(T));
            mergedObject.AddPart(connectingPart, cardPool);

            var allMergedParts = objectsToMerge.SelectMany(obj => obj.PartsIds.Select(id => cardPool.GetPart(id)));
            foreach (var part in allMergedParts)
                mergedObject.AddPart(part, cardPool);

            return mergedObject;
        }
    }

    /// <summary>
    /// Считает очки по расположению карт на полях.
    /// </summary>
    public class ScoreCalculator
    {
        public MultipartObjectsManager<City> CastlesManager { get; set; } = new MultipartObjectsManager<City>();
        public MultipartObjectsManager<Road> RoadsManager { get; set; } = new MultipartObjectsManager<Road>();
        public MultipartObjectsManager<Farm> FarmsManager { get; set; } = new MultipartObjectsManager<Farm>();
        public List<Church> Churches { get; set; } = new List<Church>();
        

        /// <summary>
        /// Добавляет карту и пересчитывает очки.
        /// </summary>
        /// <param name="card"></param>
        public void AddCard(Tile card, Cell field, Grid grid, Stack cardPool)
        {
            // проверяет подходит ли карта к одной из существующих церквей
            foreach (var church in Churches)
                church.TryAddCard(card, field, grid, cardPool);

            // подключаем карту
            foreach (ObjectPart part in card.Parts)
            {
                if (part is FieldPart)
                    FarmsManager.ProcessPart(part, cardPool);

                if (part is ChurchPart)
                    ProcessChurchPart(part, grid);

                if (part is CityPart)
                    CastlesManager.ProcessPart(part, cardPool);

                if (part is RoadPart)
                    RoadsManager.ProcessPart(part, cardPool);
            }
        }

        private void ProcessChurchPart(ObjectPart part, Grid grid)
        {
            var church = new Church((ChurchPart)part, grid);
            Churches.Add(church);
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


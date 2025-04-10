using Carcassone.Core.Calculation.Objects;
using Carcassone.Core.Cards;
using Carcassone.Core.Fields;
using Carcassone.Core.Players;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carcassone.Core.Calculation
{
    /// <summary>
    /// Считает очки по расположению карт на полях.
    /// </summary>
    public class ScoreCalculator
    {
        public List<Castle> Castles { get; set; } = new List<Castle>();
        public List<Road> Roads { get; set; } = new List<Road>();
        public List<Church> Churches { get; set; } = new List<Church>();
        public List<Cornfield> Cornfields { get; set; } = new List<Cornfield>();

        /// <summary>
        /// Добавляет карту и пересчитывает очки.
        /// </summary>
        /// <param name="card"></param>
        public void AddCard(Card card, Field field, FieldBoard fieldBoard, CardPool cardPool)
        {
            // проверяет подходит ли карта к одной из существующих церквей
            foreach (var church in Churches)
                church.TryAddCard(card, field, fieldBoard, cardPool);

            // подключаем карту
            foreach (ObjectPart part in card.Parts)
            {
                if (part is CornfieldPart)
                    ProcessCornfieldPart(part, cardPool);

                if (part is ChurchPart)
                    ProcessChurchPart(part, fieldBoard);

                if (part is CastlePart)
                    ProcessCastlePart(part, cardPool);

                if (part is RoadPart)
                    ProcessRoadPart(part, cardPool);
            }
        }

        private void ProcessRoadPart(ObjectPart part, CardPool cardPool)
        {
            List<Road> roadsToMerge = GetObjectsToMerge(part, Roads);

            var needMerge = (roadsToMerge.Count != 0);
            if (!needMerge)
            {
                var newRoad = new Road();
                newRoad.AddPart(part, cardPool);
                Roads.Add(newRoad);
            }
            else
            {
                var merged = Merge<Road>(part, roadsToMerge, cardPool);
                roadsToMerge.ForEach(r => Roads.Remove(r));
                Roads.Add(merged);
            }
        }

        private void ProcessCastlePart(ObjectPart part, CardPool cardPool)
        {
            List<Castle> castlesToMerge = GetObjectsToMerge(part, Castles);

            var needMerge = (castlesToMerge.Count != 0);
            if (!needMerge)
            {
                var newCastle = new Castle();
                newCastle.AddPart(part, cardPool);
                Castles.Add(newCastle);

            }
            else
            {
                var merged = Merge<Castle>(part, castlesToMerge, cardPool);
                castlesToMerge.ForEach(c => Castles.Remove(c));
                Castles.Add(merged);
            }
        }

        private void ProcessChurchPart(ObjectPart part, FieldBoard fieldBoard)
        {
            var church = new Church((ChurchPart)part, fieldBoard);
            Churches.Add(church);
        }

        private void ProcessCornfieldPart(ObjectPart part, CardPool cardPool)
        {
            List<Cornfield> cornfieldsToMerge = GetObjectsToMerge(part, Cornfields);

            var needMerge = (cornfieldsToMerge.Count != 0);
            if (!needMerge)
            {
                var newCornfield = new Cornfield();
                newCornfield.AddPart(part, cardPool);
                Cornfields.Add(newCornfield);

            }
            else
            {
                var merged = Merge<Cornfield>(part, cornfieldsToMerge, cardPool);
                cornfieldsToMerge.ForEach(c => Cornfields.Remove(c));
                Cornfields.Add(merged);
            }
        }

        public void CloseObjectsAndReturnChips(GamePlayersPool playersPool, CardPool cardPool)
        {
            foreach (var church in Churches)
                church.TryToClose(playersPool, cardPool);

            foreach (var castle in Castles)
                castle.TryToClose(playersPool, cardPool);

            foreach (var road in Roads)
                road.TryToClose(playersPool, cardPool);

            foreach (var cornfield in Cornfields)
                cornfield.RecalculatePartsOwner(cardPool);
        }

        public PlayerScore GetPlayerScore(string playerName, GamePlayersPool plyersPool, CardPool cardPool)
        {
            var scores = GetPlayersScores(plyersPool, cardPool);
            var score = scores.Single(s => s.PlayerName == playerName);
            return score;
        }

        private IEnumerable<PlayerScore> GetPlayersScores(GamePlayersPool plyersPool, CardPool cardPool)
        {
            var scores = new List<PlayerScore>();
            foreach (var player in plyersPool.GamePlayers)
            {
                var score = new PlayerScore()
                {
                    PlayerName = player.Name,
                    ChurchesScore = GetChurchesScore(player, cardPool),
                    CornfieldsScore = GetCornfieldsScore(player, cardPool),
                    RoadsScore = GetRoadsScore(player, cardPool),
                    CastlesScore = GetCastlesScore(player, cardPool),
                    CastlesCount = GetCastles(player, cardPool).Count(),
                    RoadsCount = GetRoads(player, cardPool).Count(),
                    CornfieldsCount = GetCornfields(player, cardPool).Count(),
                    ChurchesCount = GetChurches(player, cardPool).Count(),
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

        /// <summary>
        /// Если часть присоединяется к нескольким обьектам то их можно мержить
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="part"></param>
        /// <param name="objects"></param>
        /// <returns></returns>
        private List<T> GetObjectsToMerge<T>(ObjectPart part, List<T> objects) where T : IMultipartObject
        {
            var objectsToMerge = new List<T>();
            foreach (var gameObject in objects)
            {
                if (gameObject.CanConnect(part))
                    objectsToMerge.Add(gameObject);
            }

            return objectsToMerge;
        }

        private T Merge<T>(
            ObjectPart connectingPart,
            IEnumerable<IMultipartObject> objectsToMerge,
            CardPool cardPool)
            where T : IMultipartObject
        {
            if (objectsToMerge == null)
                return default;

            if (!objectsToMerge.Any())
                return default;

            var mergedObject = (T)Activator.CreateInstance(typeof(T));
            mergedObject.AddPart(connectingPart, cardPool);

            foreach (var gameObject in objectsToMerge)
            {
                foreach (var part in gameObject.PartsIds.Select(id => cardPool.GetPart(id)))
                    mergedObject.AddPart(part, cardPool);
            }

            return mergedObject;
        }

        private IEnumerable<Castle> GetCastles(GamePlayer player, CardPool cardPool)
        {
            return Castles.Where(castle => castle.IsPlayerOwner(player, cardPool));
        }

        private IEnumerable<Road> GetRoads(GamePlayer player, CardPool cardPool)
        {
            return Roads.Where(road => road.IsPlayerOwner(player, cardPool));
        }

        private IEnumerable<Cornfield> GetCornfields(GamePlayer player, CardPool cardPool)
        {
            return Cornfields.Where(cornfield => cornfield.IsPlayerOwner(player, cardPool));
        }

        private IEnumerable<Church> GetChurches(GamePlayer player, CardPool cardPool)
        {
            return Churches.Where(church => church.IsPlayerOwner(player, cardPool));
        }


        private int GetCastlesScore(GamePlayer player, CardPool cardPool)
        {
            return GetCastles(player, cardPool).ToList().Sum(castle => castle.GetPoints(cardPool));
        }

        private int GetRoadsScore(GamePlayer player, CardPool cardPool)
        {
            return GetRoads(player, cardPool).ToList().Sum(road => road.GetPoints(cardPool));
        }

        private int GetCornfieldsScore(GamePlayer player, CardPool cardPool)
        {
            return GetCornfields(player, cardPool).ToList().Sum(cornfield => cornfield.GetPoints(Castles, cardPool));
        }

        private int GetChurchesScore(GamePlayer player, CardPool cardPool)
        {
            return GetChurches(player, cardPool).ToList().Sum(church => church.GetPoints());
        }
    }
}


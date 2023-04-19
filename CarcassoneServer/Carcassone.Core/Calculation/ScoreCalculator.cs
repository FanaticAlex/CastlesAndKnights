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
        public void AddCard(Card card, Field field, FieldBoard fieldBoard)
        {
            // проверяет подходит ли карта к одной из существующих церквей
            foreach (var church in Churches)
                church.TryAddCard(card, field, fieldBoard);

            // подключаем карту
            foreach (ObjectPart part in card.Parts)
            {
                if (part is CornfieldPart)
                    ProcessCornfieldPart(part);

                if (part is ChurchPart)
                    ProcessChurchPart(part, fieldBoard);

                if (part is CastlePart)
                    ProcessCastlePart(part);

                if (part is RoadPart)
                    ProcessRoadPart(part);
            }
        }

        private void ProcessRoadPart(ObjectPart part)
        {
            List<Road> roadsToMerge = GetObjectsToMerge(part, Roads);

            var needMerge = (roadsToMerge.Count != 0);
            if (!needMerge)
            {
                var newRoad = new Road();
                newRoad.AddPart(part);
                Roads.Add(newRoad);
            }
            else
            {
                var merged = Merge<Road>(part, roadsToMerge);
                roadsToMerge.ForEach(r => Roads.Remove(r));
                Roads.Add(merged);
            }
        }

        private void ProcessCastlePart(ObjectPart part)
        {
            List<Castle> castlesToMerge = GetObjectsToMerge(part, Castles);

            var needMerge = (castlesToMerge.Count != 0);
            if (!needMerge)
            {
                var newCastle = new Castle();
                newCastle.AddPart(part);
                Castles.Add(newCastle);

            }
            else
            {
                var merged = Merge<Castle>(part, castlesToMerge);
                castlesToMerge.ForEach(c => Castles.Remove(c));
                Castles.Add(merged);
            }
        }

        private void ProcessChurchPart(ObjectPart part, FieldBoard fieldBoard)
        {
            var church = new Church((ChurchPart)part, fieldBoard);
            Churches.Add(church);
        }

        private void ProcessCornfieldPart(ObjectPart part)
        {
            List<Cornfield> cornfieldsToMerge = GetObjectsToMerge(part, Cornfields);

            var needMerge = (cornfieldsToMerge.Count != 0);
            if (!needMerge)
            {
                var newCornfield = new Cornfield();
                newCornfield.AddPart(part);
                Cornfields.Add(newCornfield);

            }
            else
            {
                var merged = Merge<Cornfield>(part, cornfieldsToMerge);
                cornfieldsToMerge.ForEach(c => Cornfields.Remove(c));
                Cornfields.Add(merged);
            }
        }

        public void CloseObjectsAndReturnChips(PlayersPool playersPool)
        {
            foreach (var church in Churches)
                church.TryToClose(playersPool);

            foreach (var castle in Castles)
                castle.TryToClose(playersPool);

            foreach (var road in Roads)
                road.TryToClose(playersPool);

            foreach (var cornfield in Cornfields)
                cornfield.RecalculatePartsOwner();
        }

        public PlayerScore GetPlayerScore(BasePlayer player, PlayersPool plyersPool, CardPool cardPool)
        {
            var scores = GetPlayersScores(plyersPool, cardPool);
            var score = scores.Single(s => s.PlayerName == player.Name);
            return score;
        }

        private IEnumerable<PlayerScore> GetPlayersScores(PlayersPool plyersPool, CardPool cardPool)
        {
            var scores = new List<PlayerScore>();
            foreach (var player in plyersPool.Players)
            {
                var score = new PlayerScore()
                {
                    PlayerName = player.Name,
                    ChurchesScore = GetChurchesScore(player),
                    CornfieldsScore = GetCornfieldsScore(player, cardPool),
                    RoadsScore = GetRoadsScore(player),
                    CastlesScore = GetCastlesScore(player),
                    CastlesCount = GetCastles(player).Count(),
                    RoadsCount = GetRoads(player).Count(),
                    CornfieldsCount = GetCornfields(player).Count(),
                    ChurchesCount = GetChurches(player).Count(),
                    ChipCount = player.ChipCount
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

        private T Merge<T>(ObjectPart connectingPart, IEnumerable<IMultipartObject> objectsToMerge)
            where T : IMultipartObject
        {
            if (objectsToMerge == null)
                return default;

            if (!objectsToMerge.Any())
                return default;

            var mergedObject = (T)Activator.CreateInstance(typeof(T));
            mergedObject.AddPart(connectingPart);

            foreach (var gameObject in objectsToMerge)
            {
                foreach (var part in gameObject.Parts)
                    mergedObject.AddPart(part);
            }

            return mergedObject;
        }

        private IEnumerable<Castle> GetCastles(BasePlayer player)
        {
            return Castles.Where(castle => castle.IsPlayerOwner(player));
        }

        private IEnumerable<Road> GetRoads(BasePlayer player)
        {
            return Roads.Where(road => road.IsPlayerOwner(player));
        }

        private IEnumerable<Cornfield> GetCornfields(BasePlayer player)
        {
            return Cornfields.Where(cornfield => cornfield.IsPlayerOwner(player));
        }

        private IEnumerable<Church> GetChurches(BasePlayer player)
        {
            return Churches.Where(church => church.IsPlayerOwner(player));
        }


        private int GetCastlesScore(BasePlayer player)
        {
            return GetCastles(player).Sum(castle => castle.GetPoints());
        }

        private int GetRoadsScore(BasePlayer player)
        {
            return GetRoads(player).Sum(road => road.GetPoints());
        }

        private int GetCornfieldsScore(BasePlayer player, CardPool cardPool)
        {
            return GetCornfields(player).Sum(cornfield => cornfield.GetPoints(Castles, cardPool));
        }

        private int GetChurchesScore(BasePlayer player)
        {
            return GetChurches(player).Sum(church => church.GetPoints());
        }
    }
}


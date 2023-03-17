using Carcassone.Core.Calculation.Objects;
using Carcassone.Core.Cards;
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
        public void AddCard(Card card)
        {
            // проверяет подходит ли карта к одной из существующих церквей
            foreach (var church in Churches)
                church.TryAddCard(card);

            // подключаем карту
            foreach (ObjectPart part in card.Parts)
            {
                if (part is CornfieldPart)
                    ProcessCornfieldPart(part);

                if (part is ChurchPart)
                    ProcessChurchPart(part);

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

        private void ProcessChurchPart(ObjectPart part)
        {
            var church = new Church((ChurchPart)part);
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

        public void CloseObjectsAndReturnChips()
        {
            foreach (var church in Churches)
                church.TryToClose();

            foreach (var castle in Castles)
                castle.TryToClose();

            foreach (var road in Roads)
                road.TryToClose();

            foreach (var cornfield in Cornfields)
                cornfield.RecalculatePartsOwner();
        }

        public PlayerScore GetPlayerScore(BasePlayer player, GameRoom room)
        {
            var scores = GetPlayersScores(room);
            var score = scores.Single(s => s.PlayerName == player.Name);
            return score;
        }

        private IEnumerable<PlayerScore> GetPlayersScores(GameRoom room)
        {
            var scores = new List<PlayerScore>();
            foreach (var player in room.GetPlayers())
            {
                var pChurches = GetChurchesScore(player);
                var pRoads = GetRoadsScore(player);
                var pCornfields = GetCornfieldsScore(player, room);
                var pCastles = GetCastlesScore(player);
                var pChipCount = player.ChipCount;
                var score = new PlayerScore(player.Name, pChurches, pCornfields, pRoads, pCastles, pChipCount);
                scores.Add(score);
            }

            scores.Sort(delegate (PlayerScore x, PlayerScore y)
            {
                return x.GetOverallScore().CompareTo(y.GetOverallScore());
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
                foreach (var part in gameObject.GetParts())
                    mergedObject.AddPart(part);
            }

            return mergedObject;
        }

        private int GetCastlesScore(BasePlayer player)
        {
            return Castles
                .Where(castle => castle.IsPlayerOwner(player))
                .Sum(castle => castle.GetPoints());
        }

        private int GetRoadsScore(BasePlayer player)
        {
            return Roads
                .Where(road => road.IsPlayerOwner(player))
                .Sum(road => road.GetPoints());
        }

        private int GetCornfieldsScore(BasePlayer player, GameRoom room)
        {
            return Cornfields
                .Where(cornfield => cornfield.IsPlayerOwner(player))
                .Sum(cornfield => cornfield.GetPoints(Castles, room));
        }

        private int GetChurchesScore(BasePlayer player)
        {
            return Churches
                .Where(church => church.IsPlayerOwner(player))
                .Sum(church => church.GetPoints());
        }
    }
}


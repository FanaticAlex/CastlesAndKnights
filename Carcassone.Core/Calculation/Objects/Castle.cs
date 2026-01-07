using Carcassone.Core.Tiles;
using System;
using System.Linq;

namespace Carcassone.Core.Calculation.Objects
{
    /// <summary>
    /// Объект замок для подсчета очков. состоит из частей на картах. Завершаемый.
    /// </summary>
    public class Castle : ClosingMultipartObject
    {
        /// <summary>
        /// Возвращает количество очков за замок.
        /// </summary>
        /// <returns></returns>
        public int GetPoints(Stack cardPool)
        {
            // за каждую часть замка по 1 очку, за карту замка со щитом 2 очка.
            // если замок завершен очки удваиваются
            var parts = PartsIds.Select(id => cardPool.GetPart(id));
            var score = parts.Sum(part => ((CastlePart)part).IsThereShield ? 2 : 1);
            if (IsFinished)
                score *= 2;

            return score;
        }
    }
}
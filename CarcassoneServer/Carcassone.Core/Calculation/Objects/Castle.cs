using Carcassone.Core.Cards;
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
        public int GetPoints()
        {
            // тест
            foreach (var part in Parts)
            {
                try
                {
                    var part1 = (CastlePart)part;
                }
                catch (InvalidCastException ex)
                {
                    throw new Exception($"Неверный тип присоединенной части замка {part.PartName}. Карта {part.CardId}. Тип {part.GetType()}", ex);
                }
            }

            // за каждую часть замка по 1 очку, за карту замка со щитом 2 очка.
            // если замок завершен очки удваиваются
            var score = Parts.Sum(part => ((CastlePart)part).IsThereShield ? 2 : 1);
            if (IsFinished)
                score *= 2;

            return score;
        }
    }
}
using System.Linq;


namespace Assets.Scripts
{
    /*internal static class GameObjectTranslator
    {
        public static Carcassone.ApiClient.GameRoom ToCommon(this Carcassone.Core.GameRoom room)
        {
            if (room == null)
                return null;

            var commonRoom = new Carcassone.ApiClient.GameRoom();
            commonRoom.Id = room.Id;
            commonRoom.IsStarted = room.IsStarted;
            commonRoom.IsFinished = room.IsFinished;
            return commonRoom;
        }

        public static Carcassone.ApiClient.BasePlayer ToCommon(this Carcassone.Core.Players.BasePlayer player)
        {
            if (player == null)
                return null;

            var commonPlayer = new Carcassone.ApiClient.BasePlayer();
            commonPlayer.ChipCount = player.ChipCount;
            commonPlayer.LastCardId = player.LastCardId;
            commonPlayer.Name = player.Name;
            commonPlayer.Color = player.Color;
            return commonPlayer;
        }

        public static Carcassone.ApiClient.Flag ToCommon(this Carcassone.Core.Players.Flag flag)
        {
            if (flag == null)
                return null;

            var commonFlag = new Carcassone.ApiClient.Flag();
            commonFlag.Owner = flag.Owner?.ToCommon();
            return commonFlag;
        }

        public static Carcassone.ApiClient.ChipType ToCommon(this Carcassone.Core.Players.ChipType chipType)
        {
            switch (chipType)
            {
                case Carcassone.Core.Players.ChipType.Knight: return Carcassone.ApiClient.ChipType._0;
                case Carcassone.Core.Players.ChipType.Bishop: return Carcassone.ApiClient.ChipType._1;
                case Carcassone.Core.Players.ChipType.Merchant: return Carcassone.ApiClient.ChipType._2;
                case Carcassone.Core.Players.ChipType.Peasant: return Carcassone.ApiClient.ChipType._3;
                case Carcassone.Core.Players.ChipType.None: return Carcassone.ApiClient.ChipType._4;
            }

            return Carcassone.ApiClient.ChipType._4;
        }

        public static Carcassone.ApiClient.Chip ToCommon(this Carcassone.Core.Players.Chip chip)
        {
            if (chip == null)
                return null;

            var commonChip = new Carcassone.ApiClient.Chip();
            commonChip.Owner = chip.Owner?.ToCommon();
            commonChip.Type = chip.Type.ToCommon();
            return commonChip;
        }

        public static Carcassone.ApiClient.ObjectPart ToCommon(this Carcassone.Core.Cards.ObjectPart part)
        {
            if (part == null)
                return null;

            var commonPart = new Carcassone.ApiClient.ObjectPart();
            commonPart.PartName = part.PartName;
            commonPart.CardName = part.CardName;
            commonPart.PartId = part.PartId;
            commonPart.Chip = part.Chip?.ToCommon();
            commonPart.Flag = part.Flag?.ToCommon();
            commonPart.IsOwned = part.IsOwned;
            commonPart.PartType = part.PartType;
            return commonPart;
        }

        public static Carcassone.ApiClient.ChurchPart ToCommon(this Carcassone.Core.Cards.ChurchPart part)
        {
            if (part == null)
                return null;

            var commonChurchPart = new Carcassone.ApiClient.ChurchPart();
            commonChurchPart.PartName = part.PartName;
            commonChurchPart.CardName = part.CardName;
            commonChurchPart.PartId = part.PartId;
            commonChurchPart.Chip = part.Chip?.ToCommon();
            commonChurchPart.Flag = part.Flag?.ToCommon();
            commonChurchPart.ChurchField = part.ChurchField?.ToCommon();
            commonChurchPart.IsOwned = part.IsOwned;
            commonChurchPart.PartType = part.PartType;
            return commonChurchPart;
        }

        public static Carcassone.ApiClient.Field ToCommon(this Carcassone.Core.Fields.Field field)
        {
            if (field == null)
                return null;

            var commonField = new Carcassone.ApiClient.Field();
            commonField.X = field.X;
            commonField.Y = field.Y;
            commonField.Id = field.Id;
            return commonField;
        }

        public static Carcassone.ApiClient.Card ToCommon(this Carcassone.Core.Cards.Card card)
        {
            if (card == null)
                return null;

            var commonCard = new Carcassone.ApiClient.Card();
            commonCard.CardName = card.CardName;
            commonCard.Parts = card.Parts.Select(p => p.ToCommon()).ToList();
            commonCard.RotationsCount = card.RotationsCount;
            return commonCard;
        }

        public static Carcassone.ApiClient.PlayerScore ToCommon(this Carcassone.Core.Calculation.PlayerScore score)
        {
            var commonScore = new Carcassone.ApiClient.PlayerScore();
            commonScore.ChipCount = score.ChipCount;
            commonScore.Churches = score.Churches;
            commonScore.Castles = score.Castles;
            commonScore.Cornfields = score.Cornfields;
            commonScore.Roads = score.Roads;
            return commonScore;
        }

        public static Carcassone.ApiClient.Road ToCommon(this Carcassone.Core.Calculation.Objects.Road road)
        {
            if (road == null)
                return null;

            var commonRoad = new Carcassone.ApiClient.Road();
            commonRoad.Parts = road.Parts.Select(p => p.ToCommon()).ToList();
            commonRoad.IsFinished = road.IsFinished;
            return commonRoad;
        }

        public static Carcassone.ApiClient.Castle ToCommon(this Carcassone.Core.Calculation.Objects.Castle castle)
        {
            if (castle == null)
                return null;

            var commonCastle = new Carcassone.ApiClient.Castle();
            commonCastle.Parts = castle.Parts.Select(p => p.ToCommon()).ToList();
            commonCastle.IsFinished = castle.IsFinished;
            return commonCastle;
        }

        public static Carcassone.ApiClient.Cornfield ToCommon(this Carcassone.Core.Calculation.Objects.Cornfield cornfield)
        {
            if (cornfield == null)
                return null;

            var commonCornfield = new Carcassone.ApiClient.Cornfield();
            commonCornfield.Parts = cornfield.Parts.Select(p => p.ToCommon()).ToList();
            return commonCornfield;
        }

        public static Carcassone.ApiClient.Church ToCommon(this Carcassone.Core.Calculation.Objects.Church church)
        {
            if (church == null)
                return null;

            var commonChurch = new Carcassone.ApiClient.Church();
            commonChurch.IsFinished = church.IsFinished;
            commonChurch.BaseChurchPart = church.BaseChurchPart.ToCommon();
            return commonChurch;
        }
    }
    */
}

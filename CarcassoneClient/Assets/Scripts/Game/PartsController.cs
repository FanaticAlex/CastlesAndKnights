using Carcassone.ApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    internal class PartsController
    {
        public Dictionary<string, GameObject> _partToGameObject = new Dictionary<string, GameObject>();
        public Dictionary<string, GameObject> _ownedPartsChips = new Dictionary<string, GameObject>();
        public Dictionary<string, GameObject> _ownedPartsFlags = new Dictionary<string, GameObject>();

        public List<ObjectPart> PartsCache = new List<ObjectPart>();

        /// <summary>
        /// Расставляет флаги на захваченных объектах
        /// </summary>
        public void UpdatePartsOwnersUI()
        {
            var newParts = GameManager.Instance.RoomService.GetActiveParts();
            var changedParts = GetChangedParts(PartsCache, newParts);
            PartsCache = newParts;

            foreach (var part in changedParts)
            {
                TryCreateChip(part, "Knight");
                TryCreateFlag(part);
            }
        }

        public List<ObjectPart> GetChangedParts(List<ObjectPart> oldParts, List<ObjectPart> newParts)
        {
            var changed = new List<ObjectPart>();
            foreach (var newPart in newParts)
            {
                var oldPart = oldParts.FirstOrDefault(p => p.PartId == newPart.PartId);
                var isChanged =
                    (oldPart == null) ||
                    (oldPart.Chip?.OwnerName != newPart.Chip?.OwnerName) ||
                    (oldPart.Flag?.OwnerName != newPart.Flag?.OwnerName);

                if (isChanged)
                    changed.Add(newPart);
            }

            return changed;
        }

        private void TryCreateFlag(ObjectPart part)
        {
            // TEST
            /*var partGO = _partToGameObject[part.PartId];
            partGO.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            partGO.SetActive(true);
            if (part.IsOwned)
            {
                var color = Color.magenta;
                partGO.GetComponent<Renderer>().material.color = color;
            }*/

            if (part.Flag == null)
                return;

            // заменяем фишку на флаг
            if (_ownedPartsChips.Keys.Contains(part.PartId))
                GameObject.Destroy(_ownedPartsChips[part.PartId]);

            if (_ownedPartsFlags.Keys.Contains(part.PartId)) // такой флаг уже есть
                return;

            var player = GameManager.Instance.RoomService.GetPlayer(part.Flag.OwnerName);
            var flagPrefab = Constants.Flags[player.Color];
            var flagObject = GameObject.Instantiate(flagPrefab);
            var partGameObject = _partToGameObject[part.PartId];
            flagObject.transform.parent = partGameObject.transform.parent;
            flagObject.transform.localPosition = partGameObject.transform.localPosition + new Vector3(0, 0, -1.3f);
            _ownedPartsFlags[part.PartId] = flagObject;
        }

        private void TryCreateChip(ObjectPart part, string type)
        {
            if (part.Chip == null)
                return;

            if (_ownedPartsChips.Keys.Contains(part.PartId))
                return;

            var chipPrefab = Constants.Chip[type];
            var chipObject = GameObject.Instantiate(chipPrefab);

            var player = GameManager.Instance.RoomService.GetPlayer(part.Chip.OwnerName);
            chipObject.GetComponent<Renderer>().material.color = Constants.Colors[player.Color];
            var partGameObject = _partToGameObject[part.PartId];
            chipObject.transform.parent = partGameObject.transform.parent;
            chipObject.transform.localPosition = partGameObject.transform.localPosition + new Vector3(0, 0, -1.3f);
            _ownedPartsChips.Add(part.PartId, chipObject);
        }
    }
}

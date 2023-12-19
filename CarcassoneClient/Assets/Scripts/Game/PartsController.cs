using Carcassone.ApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    internal class PartsController
    {
        public Dictionary<string, GameObject> _partToGameObject = new();
        private readonly Dictionary<string, GameObject> _ownedPartsChips = new();
        private readonly Dictionary<string, GameObject> _ownedPartsFlags = new();

        /// <summary>
        /// Расставляет флаги на захваченных объектах
        /// </summary>
        public void ShowFlagsAndChips()
        {
            var players = GameManager.Instance.RoomService.GetPlayers();
            var activeParts = GameManager.Instance.RoomService.GetActiveParts();

            foreach (var part in activeParts)
            {
                TryCreateChip(part, players);
                TryCreateFlag(part, players);
            }
        }

        private void TryCreateFlag(ObjectPart part, List<BasePlayer> players)
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

            var player = players.FirstOrDefault(pl => pl.Name == part.Flag.OwnerName);

            var flagPrefab = Constants.Flag;
            var flagObject = GameObject.Instantiate(flagPrefab);
            flagObject.GetComponent<SpriteRenderer>().color = Constants.Colors[player.Color];
            var partGameObject = _partToGameObject[part.PartId];
            flagObject.transform.parent = partGameObject.transform.parent;
            flagObject.transform.localPosition = partGameObject.transform.localPosition + new Vector3(0, 0, -1.3f);
            flagObject.transform.rotation = Quaternion.identity;
            _ownedPartsFlags[part.PartId] = flagObject;
        }

        private void TryCreateChip(ObjectPart part, List<BasePlayer> players)
        {
            if (part.Chip == null)
                return;

            if (_ownedPartsChips.Keys.Contains(part.PartId))
                return;

            var player = players.FirstOrDefault(pl => pl.Name == part.Chip.OwnerName);

            var chipPrefab = Constants.Chip;
            var chipObject = GameObject.Instantiate(chipPrefab);
            chipObject.GetComponent<SpriteRenderer>().color = Constants.Colors[player.Color];
            chipObject.GetComponent<Renderer>().material.color = Constants.Colors[player.Color];
            var partGameObject = _partToGameObject[part.PartId];
            chipObject.transform.parent = partGameObject.transform.parent;
            chipObject.transform.localPosition = partGameObject.transform.localPosition + new Vector3(0, 0, -1.3f);
            chipObject.transform.rotation = Quaternion.identity;
            _ownedPartsChips.Add(part.PartId, chipObject);
        }
    }
}

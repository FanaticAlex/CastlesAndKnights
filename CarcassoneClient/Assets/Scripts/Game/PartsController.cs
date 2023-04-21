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

        /// <summary>
        /// Расставляет флаги на захваченных объектах
        /// </summary>
        public void UpdatePartsOwnersUI()
        {
            var roads = GameManager.Instance.RoomService.GetRoads();
            foreach (var road in roads)
                SetOwnersToRoadParts(road);

            var castles = GameManager.Instance.RoomService.GetCastles();
            foreach (var castle in castles)
                SetOwnersToCastleParts(castle);

            var cornfields = GameManager.Instance.RoomService.GetCornfields();
            foreach (var cornfield in cornfields)
                SetOwnersToCornfieldParts(cornfield);

            var churches = GameManager.Instance.RoomService.GetChurches();
            foreach (var church in churches)
                SetOwnersToChurch(church);
        }

        /// <summary>
        /// Заменяет фишки на флаги
        /// </summary>
        /// <param name="road"></param>
        private void SetOwnersToRoadParts(Road road)
        {
            foreach (var partId in road.PartsIds)
            {
                TryCreateChip(partId, "Thief");
                TryCreateFlag(partId);
            }
        }

        private void SetOwnersToChurch(Church church)
        {
            // если церковь никому не принадлежит
            var baseChurchPart = GameManager.Instance.RoomService.GetObjectPart(church.BaseChurchPartId);
            if ((baseChurchPart.Chip == null) && (baseChurchPart.Flag == null))
                return;

            if (church.IsFinished)
            {
                if (_ownedPartsFlags.Keys.Contains(baseChurchPart.PartId))
                    return;

                // заменяем на флаг
                if (_ownedPartsChips.Keys.Contains(baseChurchPart.PartId))
                    GameObject.Destroy(_ownedPartsChips[baseChurchPart.PartId]);

                var player = GameManager.Instance.RoomService.GetPlayer(baseChurchPart.Flag.OwnerName);
                var flagPrefab = Constants.Flags[player.Color];
                var flagObject = GameObject.Instantiate(flagPrefab);
                flagObject.transform.position = _partToGameObject[baseChurchPart.PartId].transform.position + new Vector3(0, 0, -1.3f);
                _ownedPartsFlags.Add(baseChurchPart.PartId, flagObject);
            }
            else
            {
                if (_ownedPartsChips.Keys.Contains(baseChurchPart.PartId))
                    return;

                var chipPrefab = Constants.Chip["Priest"];
                var chipObject = GameObject.Instantiate(chipPrefab);
                var player = GameManager.Instance.RoomService.GetPlayer(baseChurchPart.Chip.OwnerName);
                chipObject.GetComponent<Renderer>().material.color = Constants.Colors[player.Color];
                var partGameObject = _partToGameObject[baseChurchPart.PartId];
                chipObject.transform.position = partGameObject.transform.position + new Vector3(0, 0, -1.3f);
                _ownedPartsChips.Add(baseChurchPart.PartId, chipObject);
            }
        }

        private void SetOwnersToCastleParts(Castle castle)
        {
            foreach (var partId in castle.PartsIds)
            {
                TryCreateChip(partId, "Knight");
                TryCreateFlag(partId);
            }
        }

        private void SetOwnersToCornfieldParts(Cornfield cornfield)
        {
            foreach (var partId in cornfield.PartsIds)
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

                TryCreateChip(partId, "Peasant");
            }
        }

        private void TryCreateFlag(string partId)
        {
            var part = GameManager.Instance.RoomService.GetObjectPart(partId);
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
            flagObject.transform.position = _partToGameObject[part.PartId].transform.position + new Vector3(0, 0, -1.3f);
            _ownedPartsFlags[part.PartId] = flagObject;
        }

        private void TryCreateChip(string partId, string type)
        {
            var part = GameManager.Instance.RoomService.GetObjectPart(partId);
            if (part.Chip == null)
                return;

            if (_ownedPartsChips.Keys.Contains(part.PartId))
                return;

            var chipPrefab = Constants.Chip[type];
            var chipObject = GameObject.Instantiate(chipPrefab);

            var player = GameManager.Instance.RoomService.GetPlayer(part.Chip.OwnerName);
            chipObject.GetComponent<Renderer>().material.color = Constants.Colors[player.Color];
            var partGameObject = _partToGameObject[part.PartId];
            chipObject.transform.position = partGameObject.transform.position + new Vector3(0, 0, -1.3f);
            _ownedPartsChips.Add(part.PartId, chipObject);
        }
    }
}

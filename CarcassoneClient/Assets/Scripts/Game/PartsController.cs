using Carcassone.ApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            foreach (var part in road.Parts)
            {
                TryCreateChip(part, "Thief");
                TryCreateFlag(part);
            }
        }

        private void SetOwnersToChurch(Church church)
        {
            // если церковь никому не принадлежит
            if ((church.BaseChurchPart.Chip == null) && (church.BaseChurchPart.Flag == null))
                return;

            if (church.IsFinished)
            {
                if (_ownedPartsFlags.Keys.Contains(church.BaseChurchPart.PartId))
                    return;

                // заменяем на флаг
                if (_ownedPartsChips.Keys.Contains(church.BaseChurchPart.PartId))
                    GameObject.Destroy(_ownedPartsChips[church.BaseChurchPart.PartId]);

                var player = GameManager.Instance.RoomService.GetPlayer(church.BaseChurchPart.Flag.OwnerName);
                var flagPrefab = Constants.Flags[player.Color];
                var flagObject = GameObject.Instantiate(flagPrefab);
                flagObject.transform.position = _partToGameObject[church.BaseChurchPart.PartId].transform.position + new Vector3(0, 0, -1.3f);
                _ownedPartsFlags.Add(church.BaseChurchPart.PartId, flagObject);
            }
            else
            {
                if (_ownedPartsChips.Keys.Contains(church.BaseChurchPart.PartId))
                    return;

                var chipPrefab = Constants.Chip["Priest"];
                var chipObject = GameObject.Instantiate(chipPrefab);
                var player = GameManager.Instance.RoomService.GetPlayer(church.BaseChurchPart.Chip.OwnerName);
                chipObject.GetComponent<Renderer>().material.color = Constants.Colors[player.Color];
                var partGameObject = _partToGameObject[church.BaseChurchPart.PartId];
                chipObject.transform.position = partGameObject.transform.position + new Vector3(0, 0, -1.3f);
                _ownedPartsChips.Add(church.BaseChurchPart.PartId, chipObject);
            }
        }

        private void SetOwnersToCastleParts(Castle castle)
        {
            foreach (var part in castle.Parts)
            {
                TryCreateChip(part, "Knight");
                TryCreateFlag(part);
            }
        }

        private void SetOwnersToCornfieldParts(Cornfield cornfield)
        {
            foreach (var part in cornfield.Parts)
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

                TryCreateChip(part, "Peasant");
            }
        }

        private void TryCreateFlag(ObjectPart part)
        {
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
            chipObject.transform.position = partGameObject.transform.position + new Vector3(0, 0, -1.3f);
            _ownedPartsChips.Add(part.PartId, chipObject);
        }
    }
}

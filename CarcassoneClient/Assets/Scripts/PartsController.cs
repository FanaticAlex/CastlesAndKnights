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
            var roads = RoomService.Instance.Client.RoadsAsync(RoomService.Instance.RoomId).Result;
            foreach (var road in roads)
            {
                SetOwnersToRoadParts(road);
            }

            var castles = RoomService.Instance.Client.CastlesAsync(RoomService.Instance.RoomId).Result;
            foreach (var castle in castles)
            {
                SetOwnersToCastleParts(castle);
            }

            var cornfields = RoomService.Instance.Client.CornfieldsAsync(RoomService.Instance.RoomId).Result;
            foreach (var cornfield in cornfields)
            {
                SetOwnersToCornfieldParts(cornfield);
            }

            var churches = RoomService.Instance.Client.ChurchesAsync(RoomService.Instance.RoomId).Result;
            foreach (var church in churches)
            {
                SetOwnersToChurch(church);
            }
        }

        /// <summary>
        /// Заменяет фишки на флаги
        /// </summary>
        /// <param name="road"></param>
        private void SetOwnersToRoadParts(Road road)
        {
            foreach (var part in road.Parts)
            {
                if (part.Chip == null && part.Flag == null)
                    continue;

                if (part.Chip != null)
                {
                    if (!_ownedPartsChips.Keys.Contains(part.PartId))
                    {
                        var ownerName = part.Chip.Owner.Name;
                        var chipPrefab = Constants.Chip["Thief"];
                        var chipObject = GameObject.Instantiate(chipPrefab);
                        chipObject.GetComponent<Renderer>().material.color = Constants.Colors[part.Chip.Owner.Color];
                        var partGameObject = _partToGameObject[part.PartId];
                        chipObject.transform.position = partGameObject.transform.position + new Vector3(0, 0, -1.3f);
                        _ownedPartsChips.Add(part.PartId, chipObject);
                    }

                    continue;
                }

                if (part.Flag != null)
                {
                    if (!_ownedPartsFlags.Keys.Contains(part.PartId))
                    {
                        // заменяем на флаг
                        if (_ownedPartsChips.Keys.Contains(part.PartId))
                        {
                            GameObject.Destroy(_ownedPartsChips[part.PartId]);
                        }

                        var flagPrefab = Constants.Flags[part.Flag.Owner.Color];
                        var flagObject = GameObject.Instantiate(flagPrefab);
                        flagObject.transform.position = _partToGameObject[part.PartId].transform.position + new Vector3(0, 0, -1.3f);
                        _ownedPartsFlags[part.PartId] = flagObject;
                    }
                }
            }
        }

        private void SetOwnersToChurch(Church church)
        {
            if (church.Owner == null)
                return;

            if (church.IsFinished)
            {
                if (_ownedPartsFlags.Keys.Contains(church.BaseChurchPart.PartId))
                    return;

                // заменяем на флаг
                if (_ownedPartsChips.Keys.Contains(church.BaseChurchPart.PartId))
                {
                    GameObject.Destroy(_ownedPartsChips[church.BaseChurchPart.PartId]);
                }

                var flagPrefab = Constants.Flags[church.Owner.Color];
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
                chipObject.GetComponent<Renderer>().material.color = Constants.Colors[church.BaseChurchPart.Chip.Owner.Color];
                var partGameObject = _partToGameObject[church.BaseChurchPart.PartId];
                chipObject.transform.position = partGameObject.transform.position + new Vector3(0, 0, -1.3f);
                _ownedPartsChips.Add(church.BaseChurchPart.PartId, chipObject);
            }
        }

        private void SetOwnersToCastleParts(Castle castle)
        {
            foreach (var part in castle.Parts)
            {
                if (part.Chip == null && part.Flag == null)
                    continue;

                if (part.Chip != null)
                {
                    if (!_ownedPartsChips.Keys.Contains(part.PartId))
                    {
                        var ownerName = part.Chip.Owner.Name;
                        var chipPrefab = Constants.Chip["Knight"];
                        var chipObject = GameObject.Instantiate(chipPrefab);
                        chipObject.GetComponent<Renderer>().material.color = Constants.Colors[part.Chip.Owner.Color];
                        var partGameObject = _partToGameObject[part.PartId];
                        chipObject.transform.position = partGameObject.transform.position + new Vector3(0, 0, -1.3f);
                        _ownedPartsChips.Add(part.PartId, chipObject);
                    }

                    continue;
                }

                if (part.Flag != null)
                {
                    if (!_ownedPartsFlags.Keys.Contains(part.PartId))
                    {
                        // заменяем на флаг
                        if (_ownedPartsChips.Keys.Contains(part.PartId))
                        {
                            GameObject.Destroy(_ownedPartsChips[part.PartId]);
                        }

                        var flagPrefab = Constants.Flags[part.Flag.Owner.Color];
                        var flagObject = GameObject.Instantiate(flagPrefab);
                        flagObject.transform.position = _partToGameObject[part.PartId].transform.position + new Vector3(0, 0, -1.3f);
                        _ownedPartsFlags[part.PartId] = flagObject;
                    }
                }
            }
        }

        private void SetOwnersToCornfieldParts(Cornfield cornfield)
        {
            foreach (var part in cornfield.Parts)
            {
                // TEST
                var partGO = _partToGameObject[part.PartId];
                partGO.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                partGO.SetActive(true);
                if (part.IsOwned)
                {
                    partGO.GetComponent<Renderer>().material.color = Color.magenta;
                }


                if (part.Chip == null)
                    continue;

                if (_ownedPartsChips.Keys.Contains(part.PartId))
                    continue;

                var chipPrefab = Constants.Chip["Peasant"];
                var chipObject = GameObject.Instantiate(chipPrefab);
                chipObject.GetComponent<Renderer>().material.color = Constants.Colors[part.Chip.Owner.Color];
                var partGameObject = _partToGameObject[part.PartId];
                chipObject.transform.position = partGameObject.transform.position + new Vector3(0, 0, -1.3f);
                _ownedPartsChips.Add(part.PartId, chipObject);
            }
        }
    }
}

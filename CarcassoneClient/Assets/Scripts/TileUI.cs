using Carcassone.Core.Players;
using Carcassone.Core.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Assets.Scripts
{
    internal class TileUI
    {
        private GameObject _tile3DBorderGO; // to fix rotation tile 3D border effect

        public GameObject GO {  get; private set; }
        public Dictionary<string, GameObject> _partIdToGameObject = new();
        public GameObject flagObject;
        public GameObject meepleObject;

        public TileUI(Tile tile)
        {
            GO = GetTileGO(tile.TileType);
            GO.GetComponent<BoxCollider>().enabled = false;

            foreach (var part in tile.Parts)
            {
                var partGameObject = GO.transform.Find(part.PartName).gameObject;
                partGameObject.SetActive(false);
                _partIdToGameObject.Add(part.PartName, partGameObject);
            }

            // Border
            _tile3DBorderGO = GetTileBorderGO();
        }

        public void SetActive(bool active)
        {
            GO.SetActive(active);
            _tile3DBorderGO.SetActive(active);
        }

        public void ResetPositionRotation()
        {
            SetPositionRotation(new Point(0, 0), 0);
        }

        public void SetPositionRotation(Point position, int rotation)
        {
            GO.transform.position = new Vector3(position.X, position.Y, -1);
            GO.transform.rotation = Quaternion.Euler(0, 0, -90 * rotation);

            _tile3DBorderGO.transform.position = new Vector3(position.X, position.Y, -1.1f);
        }

        public void HideAllCardMarks()
        {
            foreach (var part in _partIdToGameObject.Values)
                part.SetActive(false);
        }

        public void ShowPartMark(string partName)
        {
            if (_partIdToGameObject.Keys.Contains(partName))
                _partIdToGameObject[partName].SetActive(true);
        }

        public void SetFlag(string partName, PlayerColor color)
        {
            if (meepleObject != null)  // заменяем фишку на флаг
                GameObject.Destroy(meepleObject);

            if (flagObject != null) return; // такой флаг уже есть

            var flagPrefab = Constants.Flag;
            flagObject = GameObject.Instantiate(flagPrefab);
            flagObject.GetComponent<SpriteRenderer>().color = Constants.Colors[color];
            var partGameObject = _partIdToGameObject[partName];
            flagObject.transform.parent = partGameObject.transform.parent;
            flagObject.transform.localPosition = partGameObject.transform.localPosition + new Vector3(0, 0, -1.3f);
            flagObject.transform.rotation = Quaternion.identity;
        }

        public void SetMeeple(string partName, PlayerColor color)
        {
            if (meepleObject != null) return; // такой meeple уже есть

            var chipPrefab = Constants.Chip;
            meepleObject = GameObject.Instantiate(chipPrefab);
            meepleObject.GetComponent<SpriteRenderer>().color = Constants.Colors[color];
            meepleObject.GetComponent<Renderer>().material.color = Constants.Colors[color];
            var partGameObject = _partIdToGameObject[partName];
            meepleObject.transform.parent = partGameObject.transform.parent;
            meepleObject.transform.localPosition = partGameObject.transform.localPosition + new Vector3(0, 0, -1.3f);
            meepleObject.transform.rotation = Quaternion.identity;
        }

        public Vector3 GetTilePosition()
        {
            return GO.transform.position;
        }

        private static GameObject GetTileGO(string tileType)
        {
            var prefab = (GameObject)Resources.Load("Tiles/" + tileType, typeof(GameObject));
            if (prefab == null)
                prefab = (GameObject)Resources.Load("Tiles/River/" + tileType, typeof(GameObject));

            if (prefab == null)
                throw new Exception("cant find tile " + tileType);

            return GameObject.Instantiate(prefab) ?? throw new Exception("can't initiate tile GO" + tileType);
        }

        private GameObject GetTileBorderGO()
        {
            GameObject borderPrefab = (GameObject)Resources.Load("Tiles/Tile_border", typeof(GameObject));
            if (borderPrefab == null)
                throw new Exception("cant find border  prefab");

            return GameObject.Instantiate(borderPrefab) ?? throw new Exception("cant initiate border");
        }
    }
}

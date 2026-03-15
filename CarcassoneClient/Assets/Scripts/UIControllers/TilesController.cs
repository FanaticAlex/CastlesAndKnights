using Carcassone.Core;
using Carcassone.Core.Calculation;
using Carcassone.Core.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    /// <summary>
    /// This class is responsible for tiles instantiate and view.
    /// </summary>
    internal class TilesController
    {
        public Dictionary<string, TileUI> TilesUI = new();
        private Dictionary<string, GameObject> _playerToMarkers = new();
        private SoundEffectsPlayer _soundEffectPlayer;
        private ParticleSystem placeCardEffect;
        private GameObject currentCardImageGO;

        public TilesController()
        {
            currentCardImageGO = GameObject.Find("CurrentCardImage");
            placeCardEffect = GameObject.Find("PlaceCardEffectParticles").GetComponent<ParticleSystem>();
            _soundEffectPlayer = GameObject.FindAnyObjectByType<SoundEffectsPlayer>();
        }

        public void PlaceTile(GameMove gameMove, Tile tile)
        {
            var tUI = GetTileUI(tile);
            tUI.SetActive(true);
            tUI.SetPositionRotation(gameMove.Location, gameMove.TileRotation);

            placeCardEffect.transform.position = new Vector3(gameMove.Location.X, gameMove.Location.Y, 0);
            placeCardEffect.Play();

            UpdatePlayerLastMoveMarkerUI(gameMove.TileId, gameMove.PlayerName);
            _soundEffectPlayer.PlayPutCard();
        }

        /// <summary>
        /// Обновление UI текущей карты
        /// </summary>
        public void SetCurrentTileIcon(Tile tile, int rotation)
        {
            if (tile == null)
                return;

            // изображение на панели
            var cardGO = GetTileUI(tile).GO;
            currentCardImageGO.GetComponent<Image>().sprite = cardGO.GetComponent<SpriteRenderer>().sprite;
            currentCardImageGO.transform.localRotation = Quaternion.Euler(0, 0, rotation * -90);
        }

        public void UpdateRemainTilesIcon(int remainCount)
        {
            var cardsRemainText = GameObject.Find("CardsRemain").GetComponent<Text>();
            cardsRemainText.text = $"{remainCount}";
        }

        private void UpdatePlayerLastMoveMarkerUI(string tileId, string playerName)
        {
            var player = GameParameters.Instance.Players.FirstOrDefault(p => p.Name == playerName);
            if (player.Name == null)
                return;

            var playerHaveMark = _playerToMarkers.ContainsKey(player.Name);
            if (!playerHaveMark)
            {
                var marksPrefab = Constants.Marks[player.Color];
                _playerToMarkers[player.Name] = GameObject.Instantiate(marksPrefab);
            }

            var markObject = _playerToMarkers[player.Name];
            var cardPosition = TilesUI[tileId].GetTilePosition();
            markObject.transform.position = cardPosition + new Vector3(0, 0, -1.3f);
        }

        private TileUI GetTileUI(Tile tile)
        {
            if (TilesUI.ContainsKey(tile.Id))
                return TilesUI[tile.Id];
            else
            {
                var newTileUI = new TileUI(tile);
                newTileUI.SetActive(false);
                TilesUI.Add(tile.Id, newTileUI);
                return newTileUI;
            }
        }
    }
}

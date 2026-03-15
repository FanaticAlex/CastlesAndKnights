using Carcassone.Core;
using Carcassone.Core.Calculation;
using Carcassone.Core.Players;
using Carcassone.Core.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class AvailableMoves
    {
        public List<GameMove> _moves = new List<GameMove>();

        public AvailableMoves(List<GameMove> moves)
        {
            _moves = moves;
        }

        public bool CanPut(Point cell)
        {
            return _moves.Select(m => m.Location).Distinct().Contains(cell);
        }

        public IEnumerable<int> GetAvailableRotations(Point cell) 
        {
            return _moves
                .Where(m => m.Location == cell)
                .Select(m => m.TileRotation)
                .Distinct();
        }

        public IEnumerable<GameMove> GetMoves(Point cell, int rotation)
        {
            return _moves
                .Where(m => m.Location == cell)
                .Where(m => m.TileRotation == rotation);
        }
    }

    /// <summary>
    /// Содержит данные об игре и управляет ходами одного игрока, а так же обновлением игры
    /// </summary>
    public class GameBehaviour : MonoBehaviour
    {
        private FieldsController _fieldsController;
        private TilesController _tilesController;
        private ScoreController _scoreController;
        private CameraBehaviour _cameraBehaviour;

        public GameObject SelectPartPanel;
        public GameObject FinalScoreUIPanel;
        public GameObject FinalScoreUIPanelText;
        public GameObject PlayerDetailScorePanel;
        public GameObject ExitAcceptPanel;

        private GameRoom _room;
        private AvailableMoves _availableMoves;

        private int _selectedRotation;
        private Point _selectedCell;
        private string _selectedPart;


        /// <summary>
        /// Инициализация сцены комнаты игры.
        /// - создание игрового поля
        /// - инициализация счета игроков 
        /// </summary>
        void Start()
        {
            _cameraBehaviour = Camera.main.GetComponent<CameraBehaviour>();
            _room = new GameRoom(GameParameters.Instance.Extensions, GameParameters.Instance.Players);
            _room.Finished += _room_Finished;

            _fieldsController = new FieldsController();
            _tilesController = new TilesController();

            var players = _room.GetPlayers();
            _scoreController = new ScoreController(players, FinalScoreUIPanel, FinalScoreUIPanelText, PlayerDetailScorePanel);

            ExitAcceptPanel.SetActive(false);
            SelectPartPanel.SetActive(false);
        }

        /// <summary>
        /// Основоной цикл игры.
        /// Производит ход локального игрока и обновляет UI компоненты игры.
        /// </summary>
        async void Update()
        {
            // TODO: клик по обьекту для просмотра владельцев

            if (_room.IsFinished) return;

            if (Input.GetKey(KeyCode.Escape)) ExitAcceptPanel.SetActive(true);

            if (_cameraBehaviour.State == TouchState.ZoomToCard) return;

            // если наш ход
            var currentPlayer = _room.GetCurrentPlayer();
            if (currentPlayer.IsAIProcessing) return;

            switch (currentPlayer.Info.PlayerType)
            {
                case PlayerType.Human:
                    InitiateSelectCellStage();
                    if (Input.GetMouseButtonUp(0)) TryToSelectCell();
                    break;
                case PlayerType.AI_Easy:
                case PlayerType.AI_Normal:
                case PlayerType.AI_Hard:
                    await Task.Run(() => currentPlayer.ProcessMove(_room));
                    VisualizeTurn(_room.GetMoves().Last());
                    break;
                case PlayerType.NetworkPlayer:
                    // ждать сетевых игроков 
                    break;
            }
        }

        public void VisualizeTurn(GameMove gameMove)
        {
            var time = DateTime.Now;

            var tile = _room.GetTile(gameMove.TileId);
            _tilesController.PlaceTile(gameMove, tile);
            ShowFlagsAndChips(_room.GetAllGameObjects());
            _tilesController.UpdateRemainTilesIcon(_room.GetRemainTilesCount());

            var players = _room.GetPlayers();
            var scores = players.Select(p => _room.GetPlayerScore(p.Info.Name));
            _scoreController.UpdateScore(scores);
            _scoreController.UpdateCurrentPlayerMark(_room.GetCurrentPlayer());

            var position = _tilesController.TilesUI[gameMove.TileId].GetTilePosition();
            _cameraBehaviour.MoveCameraAtCard(position);

            Logger.Info("Время отрисовки хода: " + (DateTime.Now - time).TotalSeconds);
        }

        /// <summary>
        /// Расставляет флаги на захваченных объектах
        /// </summary>
        public void ShowFlagsAndChips(IEnumerable<BaseGameObject> objects)
        {
            foreach (var obj in objects)
            {
                foreach (var part in obj.Parts)
                {
                    if (part.Meeple != null)
                        _tilesController.TilesUI[part.TileId].SetMeeple(part.PartName, part.Meeple.Owner.Info.Color);

                    if (part.Flag != null)
                        _tilesController.TilesUI[part.TileId].SetFlag(part.PartName, part.Flag.Owner.Info.Color);
                }
            }
        }

        public void OnRotateButonClick()
        {
            var rotations = _availableMoves.GetAvailableRotations(_selectedCell).ToList();
            var currentRotationIndex = rotations.IndexOf(_selectedRotation);
            var nextRotationIndex = (currentRotationIndex + 1) % rotations.Count;
            _selectedRotation = rotations[nextRotationIndex];
            var moves = _availableMoves.GetMoves(_selectedCell, _selectedRotation);
            InitiateSelectPartStage(moves);
        }

        public void OnPartSelected()
        {
            // у всех частей обьекта анимацию убираем
            var currentTile = _room.GetCurrentTile();
            _tilesController.TilesUI[currentTile.Id].HideAllCardMarks();

            // у выбранного включаем
            _selectedPart = GetSelectedPart(currentTile);
            _tilesController.TilesUI[currentTile.Id].ShowPartMark(_selectedPart);
        }

        public void OnPutCardCancel()
        {
            // при отмене карта убирается из поля
            var currentTile = _room.GetCurrentTile();
            _tilesController.TilesUI[currentTile.Id].ResetPositionRotation();
            SelectPartPanel.SetActive(false);
        }

        public void OnEndTurnButonClick()
        {
            var currentTile = _room.GetCurrentTile();
            var currentPlayer = _room.GetCurrentPlayer();

            _selectedPart = GetSelectedPart(currentTile);

            var gameMove = new GameMove()
            {
                PlayerName = currentPlayer.Info.Name,
                TileId = currentTile.Id,
                TileRotation = _selectedRotation,
                Location = _selectedCell,
                PartName = _selectedPart
            };
            _room.MakeMove(gameMove);
            VisualizeTurn(gameMove);

            // ход окончен
            _tilesController.TilesUI[currentTile.Id].HideAllCardMarks();
            _selectedPart = null;
            SelectPartPanel.SetActive(false);
        }

        public void OnShowPlayerDetailedScore(Text playerNamePanel)
        {
            var playerName = playerNamePanel.text;
            var score = _room.GetPlayerScore(playerName);
            _scoreController.ShowDetailedScore(playerNamePanel, score);
        }

        public void OnClosePlayerDetailedScore()
        {
            _scoreController.HideDetailedScore();
        }

        public void OnEndGameBtn()
        {
            SceneManager.LoadScene("CreateRoom", LoadSceneMode.Single);
            //GameParameters.Instance.SaveScore();
            //GameParameters.Instance.ResetGame();
        }

        public void OnExitAcceptButtonClick()
        {
            SceneManager.LoadScene("CreateRoom", LoadSceneMode.Single);
            //GameParameters.Instance.ResetGame();
        }

        public void OnExitCancelButtonClick()
        {
            ExitAcceptPanel.SetActive(false);
        }

        private void InitiateSelectCellStage()
        {
            var tile = _room.GetCurrentTile();
            _tilesController.SetCurrentTileIcon(tile, _selectedRotation);
            _fieldsController.ShowLocations(_room.GetCellsStatus());
            _availableMoves = new AvailableMoves(_room.GetAvailableMoves());
        }

        private void TryToSelectCell()
        {
            if (SelectPartPanel.activeSelf) // если мы в состоянии выбора чати то не кликам на поля
                return;

            // находим поле по которому был клик
            var hittedGO = GetHitedGameObject();
            var cellPoint = _fieldsController.GetFieldByGameObject(hittedGO);
            if (cellPoint == null)
            {
                Logger.Info("No cell selected");
                return;
            }
            _selectedCell = cellPoint.Value;

            // можно ли в это поле ставить текущую карту
            if (!_availableMoves.CanPut(_selectedCell))
            {
                Logger.Info("Can't play to this location!");
                return;
            }

            
            var defaultRotation = _availableMoves.GetAvailableRotations(_selectedCell).First();
            var moves = _availableMoves.GetMoves(_selectedCell, defaultRotation);
            InitiateSelectPartStage(moves);
        }

        private void InitiateSelectPartStage(IEnumerable<GameMove> availableMovesBySelectedCell)
        {
            var move = availableMovesBySelectedCell.First();
            var tile = _room.GetTile(move.TileId);
            _tilesController.PlaceTile(move, tile);

            var currentPlayer = _room.GetCurrentPlayer();
            if (currentPlayer.Info.MeeplesCount == 0)
            {
                Logger.Info("Player has no chip!");
                OnEndTurnButonClick();
                return;
            }
            
            var position = _tilesController.TilesUI[_room.GetCurrentTile().Id].GetTilePosition();
            _cameraBehaviour.MoveCameraAtCard(position);

            var parts = availableMovesBySelectedCell.Select(m => m.PartName).ToList();
            SetSelectPartPanelToggles(parts);
        }

        private void SetSelectPartPanelToggles(List<string> parts)
        {
            SelectPartPanel.SetActive(true);

            var toggleGroup = SelectPartPanel.transform.Find("ToggleGroup");
            for (int i = 0; i < 10; i++)
            {
                var toggleName = "Toggle" + i;
                var toggle = toggleGroup.Find(toggleName);
                var toggleLabel = toggle.Find("Label");

                if (i < parts.Count())
                {
                    toggle.gameObject.SetActive(true);
                    toggleLabel.GetComponent<Text>().text = parts[i];
                }
                else
                {
                    toggle.gameObject.SetActive(false);
                }
            }

            var defaultToggle = toggleGroup.Find("Toggle0");
            var defaultToggleComponent = defaultToggle.GetComponent<Toggle>();
            defaultToggleComponent.isOn = true;
        }

        private void _room_Finished(object sender, EventArgs e)
        {
            var players = _room.GetPlayers();
            var scores = players.Select(p => _room.GetPlayerScore(p.Info.Name));
            _scoreController.ShowEndGameWindow(scores);
        }

        private string GetSelectedPart(Tile tile)
        {
            if (!SelectPartPanel.activeSelf) return null; // окно не активно потому что нет вариантов для установки фишки

            var toggleGroup = SelectPartPanel.transform.Find("ToggleGroup");

            // установить фишку в части обьекта
            for (int i = 0; i < 10; i++)
            {
                var toggleName = "Toggle" + i;
                var toggle = toggleGroup.Find(toggleName);
                var toggleComponent = toggle.GetComponent<Toggle>();
                var toggleLabel = toggle.Find("Label").GetComponent<Text>();

                if (toggleComponent.isOn)
                    return toggleLabel.text;
            }

            return null;
        }

        private GameObject GetHitedGameObject()
        {
            Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hit, 100.0F);
            return hit.collider?.gameObject;
        }
    }
}

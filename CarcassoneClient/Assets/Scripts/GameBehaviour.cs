using Carcassone.Core;
using Carcassone.Core.Calculation;
using Carcassone.Core.Players;
using Carcassone.Core.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
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
        public GameObject ObjectInfoScorePanel;
        public GameObject ExitAcceptPanel;

        private GameLogic _room;
        private AvailableMoves _availableMoves;

        private int _selectedRotation;
        private Point _selectedCell;
        private string _selectedPart;

        private bool _moveInitiated;


        /// <summary>
        /// Инициализация сцены комнаты игры.
        /// - создание игрового поля
        /// - инициализация счета игроков 
        /// </summary>
        void Start()
        {
            _cameraBehaviour = Camera.main.GetComponent<CameraBehaviour>();
            _room = new GameLogic();

            _fieldsController = new FieldsController();
            _tilesController = new TilesController();

            var players = _room.GetPlayers();
            _scoreController = new ScoreController(players, FinalScoreUIPanel, FinalScoreUIPanelText, PlayerDetailScorePanel, ObjectInfoScorePanel);

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

            if (_room.IsFinished())
            {
                var scores = _room.GetPlayersScores();
                _scoreController.ShowEndGameWindow(scores);
                return;
            }

            if (Input.GetKey(KeyCode.Escape)) ExitAcceptPanel.SetActive(true);

            if (_cameraBehaviour.State == TouchState.ZoomToCard) return;

            // если наш ход
            var currentPlayer = _room.GetCurrentPlayer();
            if (currentPlayer.IsAIProcessing) return;

            switch (currentPlayer.Info.PlayerType)
            {
                case PlayerType.Human:
                    if (!_moveInitiated) InitiatePlayerMove();
                    if (Input.GetMouseButtonUp(0)) OnClick();
                    break;
                case PlayerType.AI_Easy:
                case PlayerType.AI_Normal:
                case PlayerType.AI_Hard:
                    await Task.Run(() => _room.ProcessAIPlayerMove());
                    VisualizeTurn(_room.GetState().GameMoves.Last());
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
            ShowFlagsAndChips(_room.GetState().Calculated);
            _tilesController.UpdateRemainTilesIcon(_room.GetRemainTilesCount());

            var players = _room.GetPlayers();
            var scores = _room.GetPlayersScores();
            _scoreController.UpdateScore(scores);
            _scoreController.UpdateCurrentPlayerMark(_room.GetCurrentPlayer());

            var position = _tilesController.GetTileUI(gameMove.TileId).GetTilePosition();
            _cameraBehaviour.MoveCameraAtCard(position);

            Logger.Info("Время отрисовки хода: " + (DateTime.Now - time).TotalSeconds);

            _moveInitiated = false;
        }

        /// <summary>
        /// Расставляет флаги на захваченных объектах
        /// </summary>
        public void ShowFlagsAndChips(IEnumerable<BaseGameObject> objects)
        {
            foreach (var obj in objects)
            {
                // Todo move this code to core. Make it part of state of object
                var isComplete = (obj as ICompletableGameObject)?.IsComplete(_room.GetState()); 

                foreach (var part in obj.Parts)
                {
                    var owner = _room.GetPlayer(part.GetPartOwner(_room.GetState()));
                    if (owner != null)
                    {
                        var tileUI = _tilesController.GetTileUI(part.Tile.Id);
                        if (isComplete == true)
                            tileUI.SetFlag(part.PartName, owner.Info.Color);
                        else
                            tileUI.SetMeeple(part.PartName, owner.Info.Color);
                    }
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
            _tilesController.GetTileUI(currentTile.Id).HideAllCardMarks();

            // у выбранного включаем
            _selectedPart = GetSelectedPart(currentTile);
            _tilesController.GetTileUI(currentTile.Id).ShowPartMark(_selectedPart);
        }

        public void OnPutCardCancel()
        {
            // при отмене карта убирается из поля
            var currentTile = _room.GetCurrentTile();
            _tilesController.GetTileUI(currentTile.Id).ResetPositionRotation();
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
            _tilesController.GetTileUI(currentTile.Id).HideAllCardMarks();
            _selectedPart = null;
            SelectPartPanel.SetActive(false);
        }

        public void OnShowPlayerDetailedScore(Text playerNamePanel)
        {
            var playerName = playerNamePanel.text;
            var score = _room.GetPlayersScores().FirstOrDefault(s => s.PlayerName == playerName);
            _scoreController.ShowDetailedPlayerScore(playerNamePanel, score);
        }

        public void OnClosePlayerDetailedScore()
        {
            _scoreController.HideDetailedScore();
        }

        public void OnCloseObjectDetails()
        {
            _scoreController.HideObjectInfo();
            _tilesController.HideSelection();
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

        private void InitiatePlayerMove()
        {
            var tile = _room.GetCurrentTile();
            _tilesController.SetCurrentTileIcon(tile, _selectedRotation);
            _availableMoves = new AvailableMoves(_room.GetAvailableMoves(tile.Id));
            _fieldsController.ShowLocations(_room.GetCellsStatus(), _availableMoves);
            _moveInitiated = true;
        }

        private void OnClick()
        {
            var hit = GetRaycastHit();

            var partInfo = _tilesController.GetClickedPart(hit);
            if (partInfo != null)
            {
                _tilesController.HideSelection();

                // set new selection marks
                var obj = _room.GetState().Calculated
                    .FirstOrDefault(o => o.Parts.FirstOrDefault(p => (p.PartId == partInfo.Item1 + partInfo.Item2)) != null);
                foreach (var part in obj.Parts)
                {
                    var tileUI = _tilesController.GetTileUI(part.Tile.Id);
                    tileUI.ShowPartMark(part.PartName);
                }

                _scoreController.ShowObjectInfo(obj.GetType().Name, obj.GetScore(_room.GetState()), obj.GetObjectOwners(_room.GetState()));
                return;
            }

            TryToSelectCell(hit.collider?.gameObject);
        }

        private void TryToSelectCell(GameObject hittedGO)
        {
            if (SelectPartPanel.activeSelf) // если мы в состоянии выбора чаcти то не кликам на поля
                return;

            // находим поле по которому был клик
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

            _selectedRotation = _availableMoves.GetAvailableRotations(_selectedCell).First();
            var moves = _availableMoves.GetMoves(_selectedCell, _selectedRotation);
            InitiateSelectPartStage(moves);
        }

        private void InitiateSelectPartStage(IEnumerable<GameMove> availableMovesBySelectedCell)
        {
            var move = availableMovesBySelectedCell.First();
            var tile = _room.GetTile(move.TileId);
            _tilesController.PlaceTile(move, tile);

            var currentPlayer = _room.GetCurrentPlayer();
            if (currentPlayer.GetMeeplesCount(_room.GetState()) == 0)
            {
                Logger.Info("Player has no chip!");
                OnEndTurnButonClick();
                return;
            }
            
            var position = _tilesController.GetTileUI(_room.GetCurrentTile().Id).GetTilePosition();
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

        private RaycastHit GetRaycastHit()
        {
            // Check if the pointer is over a UI element
            if (EventSystem.current.IsPointerOverGameObject() && (EventSystem.current.currentSelectedGameObject != null))
            {
                return new RaycastHit(); // Exit, don't do raycast
            }

            Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hit, 100.0F);
            return hit;
        }
    }
}

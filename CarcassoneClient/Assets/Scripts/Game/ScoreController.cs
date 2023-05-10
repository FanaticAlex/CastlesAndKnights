using Carcassone.ApiClient;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    /// <summary>
    /// Управляет интерфейсом игры:
    /// Список игроков, счет, количество фишек итд
    /// </summary>
    class ScoreController
    {
        private Dictionary<string, GameObject> _playersScorePanels = new Dictionary<string, GameObject>();

        public GameObject _finalScoreUIPanel;
        public GameObject _finalScoreUIPanelText;
        public GameObject _playerDetailScorePanel;

        /// <summary>
        /// Инициализирует панели игроков
        /// </summary>
        /// <param name="players"></param>
        public ScoreController(
            GameObject finalScoreUIPanel,
            GameObject finalScoreUIPanelText,
            GameObject playerDetailScorePanel)
        {
            var players = GameManager.Instance.RoomService.GetPlayers();

            _finalScoreUIPanel = finalScoreUIPanel;
            _finalScoreUIPanelText = finalScoreUIPanelText;
            _playerDetailScorePanel = playerDetailScorePanel;

            _finalScoreUIPanel.SetActive(false);
            _playerDetailScorePanel.SetActive(false);

            var panels = new List<GameObject>();
            panels.Add(GameObject.Find("PlayerScorePanel_1"));
            panels.Add(GameObject.Find("PlayerScorePanel_2"));
            panels.Add(GameObject.Find("PlayerScorePanel_3"));
            panels.Add(GameObject.Find("PlayerScorePanel_4"));
            panels.Add(GameObject.Find("PlayerScorePanel_5"));

            var remainPlayers = players.ToList();
            foreach (var playerPanel in panels)
            {
                var player = remainPlayers.FirstOrDefault();
                if (player != null)
                {
                    _playersScorePanels.Add(player.Name, playerPanel);

                    Color userColor;
                    ColorUtility.TryParseHtmlString(player.Color, out userColor);
                    playerPanel.transform.Find("ColorPanel").GetComponentInChildren<Image>().color = userColor;

                    remainPlayers.Remove(player);
                }
                else
                {
                    playerPanel.SetActive(false);
                }
            }
        }

        public void ShowDetailedScore(Text playerNamePanel)
        {
            var playerName = playerNamePanel.text;
            if (!_playerDetailScorePanel.activeSelf)
            {
                _playerDetailScorePanel.SetActive(true);
                var textComp = GameObject.Find("DetailedPlayerScore").GetComponent<TMP_Text>();
                textComp.text = "Player score " + playerName + "\r\n";
                var score = GameManager.Instance.RoomService.GetScore(playerName);
                textComp.text += $"Castles: {score.CastlesScore}({score.CastlesCount})\r\n";
                textComp.text += $"Fields: {score.CornfieldsScore}({score.CornfieldsCount})\r\n";
                textComp.text += $"Churches: {score.ChurchesScore}({score.ChurchesCount})\r\n";
                textComp.text += $"Roads: {score.RoadsScore}({score.RoadsCount})\r\n";
            }
        }

        public void HideDetailedScore()
        {
            _playerDetailScorePanel.SetActive(false);
        }

        public void UpdateScore()
        {
            var players = GameManager.Instance.RoomService.GetPlayers();
            var scores = players.Select(p => GameManager.Instance.RoomService.GetScore(p.Name));

            foreach (var score in scores)
            {
                var playerScoreUI = _playersScorePanels[score.PlayerName];

                var nameUI = playerScoreUI.transform.Find("PlayerName").GetComponent<Text>();
                nameUI.text = score.PlayerName;

                var chipUI = playerScoreUI.transform.Find("ChipCount").GetComponent<Text>();
                chipUI.text = "Chip: " + score.ChipCount;

                var overall = SumScore(score).ToString();
                playerScoreUI.transform.Find("InfoText").GetComponentInChildren<Text>().text = overall;
                
            }
        }

        public void UpdateCurrentPlayerMark(BasePlayer currentPlayer)
        {
            foreach(var item in  _playersScorePanels)
            {
                if (item.Key == currentPlayer?.Name)
                    item.Value.transform.Find("SelectedBorder").gameObject.SetActive(true);
                else
                    item.Value.transform.Find("SelectedBorder").gameObject.SetActive(false);
            }
        }

        public void UpdateWaitingSpinners(BasePlayer currentPlayer)
        {
            var isLocalPlayerMove = GameManager.Instance.RoomService.HumanUsers.Contains(currentPlayer?.Name);
            if (isLocalPlayerMove)
            {
                SetSpinnersOff();
            }
            else
            {
                SetPlayerSpinnerOn(currentPlayer);
            }
        }

        private void SetPlayerSpinnerOn(BasePlayer player)
        {
            foreach (var item in _playersScorePanels)
            {
                var spinner = item.Value.transform.Find("Spinner");
                var isPlayerPanel = (item.Key == player?.Name);
                spinner.gameObject.SetActive(isPlayerPanel);
            }
        }

        private void SetSpinnersOff()
        {
            foreach (var item in _playersScorePanels)
            {
                var spinner = item.Value.transform.Find("Spinner");
                spinner.gameObject.SetActive(false);
            }
        }

        public void ShowEndGameWindow()
        {
            _finalScoreUIPanel.SetActive(true);

            ////var scores = GameManager.Instance.RoomService.GetGameScores();

            var players = GameManager.Instance.RoomService.GetPlayers();
            var scores = players.Select(p => GameManager.Instance.RoomService.GetScore(p.Name));

            var winnerScore = scores.Max(x => SumScore(x));
            var winners = scores
                .Where(x => SumScore(x) == winnerScore)
                .Select(x => x.PlayerName)
                .ToList();
            _finalScoreUIPanelText.GetComponent<TMP_Text>().text = $"The winner is {string.Join(",", winners)} \r\n";
            _finalScoreUIPanelText.GetComponent<TMP_Text>().text += "SCORE \r\n";


            foreach (var score in scores)
            {
                _finalScoreUIPanelText.GetComponent<TMP_Text>().text +=
                    $"{score.PlayerName} : {SumScore(score)} \r\n";
            }

            GameManager.Instance.RoomService.Reset();
        }

        private int SumScore(PlayerScore score)
        {
            return score.CastlesScore + score.CornfieldsScore + score.ChurchesScore + score.RoadsScore;
        }
    }
}

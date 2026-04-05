using Carcassone.Core.Calculation;
using Carcassone.Core.Players;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

namespace Assets.Scripts
{
    /// <summary>
    /// Управляет интерфейсом игры:
    /// Список игроков, счет, количество фишек итд
    /// </summary>
    class ScoreController
    {
        private readonly Dictionary<string, GameObject> _playersScorePanels = new();

        public GameObject _finalScoreUIPanel;
        public GameObject _finalScoreUIPanelText;
        public GameObject _playerDetailScorePanel;
        public GameObject _objectInfoPanel;

        /// <summary>
        /// Инициализирует панели игроков
        /// </summary>
        /// <param name="players"></param>
        public ScoreController(
            IEnumerable<GamePlayer> players,
            GameObject finalScoreUIPanel,
            GameObject finalScoreUIPanelText,
            GameObject playerDetailScorePanel,
            GameObject objectInfoPanel)
        {
            _finalScoreUIPanel = finalScoreUIPanel;
            _finalScoreUIPanelText = finalScoreUIPanelText;
            _playerDetailScorePanel = playerDetailScorePanel;
            _objectInfoPanel = objectInfoPanel;

            _finalScoreUIPanel.SetActive(false);
            _playerDetailScorePanel.SetActive(false);
            objectInfoPanel.SetActive(false);

            var panels = new List<GameObject>
            {
                GameObject.Find("PlayerScorePanel_1"),
                GameObject.Find("PlayerScorePanel_2"),
                GameObject.Find("PlayerScorePanel_3"),
                GameObject.Find("PlayerScorePanel_4"),
                GameObject.Find("PlayerScorePanel_5")
            };

            var remainPlayers = players.ToList();
            foreach (var playerPanel in panels)
            {
                var player = remainPlayers.FirstOrDefault();
                if (player != null)
                {
                    _playersScorePanels.Add(player.Info.Name, playerPanel);
                    Color userColor = Constants.Colors[player.Info.Color];
                    playerPanel.transform.Find("ColorPanel").GetComponentInChildren<Image>().color = userColor;

                    remainPlayers.Remove(player);
                }
                else
                {
                    playerPanel.SetActive(false);
                }
            }
        }

        public void ShowDetailedPlayerScore(Text playerNamePanel, PlayerScore score)
        {
            if (!_playerDetailScorePanel.activeSelf)
            {
                _playerDetailScorePanel.SetActive(true);
                var textComp = GameObject.Find("DetailedPlayerScore").GetComponent<TMP_Text>();
                var playerName = playerNamePanel.text;
                textComp.text = "Player score " + playerName + "\r\n";
                foreach(var objInfo in score.scoreObjs)
                {
                    textComp.text += $"{objInfo.ObjectName}: {objInfo.ObjectScore} (count: {objInfo.ObjectCount})\r\n";
                }
            }
        }

        public void HideDetailedScore()
        {
            _playerDetailScorePanel.SetActive(false);
        }

        public void ShowObjectInfo(string objType, int score, IEnumerable<string> owners)
        {
            _objectInfoPanel.SetActive(true);
            var textComp = GameObject.Find("ObjectInfo").GetComponent<TMP_Text>();
            textComp.text = "Object type: " + objType + "\r\n";
            textComp.text += $"score: {score}\r\n";
            textComp.text += $"owners: {string.Join(",", owners)}\r\n";
        }

        public void HideObjectInfo()
        {
            _objectInfoPanel.SetActive(false);
        }

        public void UpdateScore(IEnumerable<PlayerScore> scores)
        {
            foreach (var score in scores)
            {
                var playerScoreUI = _playersScorePanels[score.PlayerName];

                var nameUI = playerScoreUI.transform.Find("PlayerName").GetComponent<Text>();
                nameUI.text = score.PlayerName;

                var chipUI = playerScoreUI.transform.Find("ChipCount").GetComponent<Text>();
                chipUI.text = $"Chip: {score.MeeplesCount}";

                var overall = score.OverallScore.ToString();
                playerScoreUI.transform.Find("InfoText").GetComponentInChildren<Text>().text = $"Score: {overall}";
            }
        }

        public void UpdateCurrentPlayerMark(GamePlayer currentPlayer)
        {
            foreach(var item in  _playersScorePanels)
            {
                if (item.Key == currentPlayer?.Info.Name)
                    item.Value.transform.Find("SelectedBorder").gameObject.SetActive(true);
                else
                    item.Value.transform.Find("SelectedBorder").gameObject.SetActive(false);
            }
        }

        public void ShowEndGameWindow(IEnumerable<PlayerScore> scores)
        {
            _finalScoreUIPanel.SetActive(true);

            var winnerScore = scores.Max(x => x.OverallScore);
            var winners = scores
                .Where(x => x.OverallScore == winnerScore)
                .Select(x => x.PlayerName)
                .ToList();
            _finalScoreUIPanelText.GetComponent<TMP_Text>().text = $"The winner is {string.Join(",", winners)} \r\n";
            _finalScoreUIPanelText.GetComponent<TMP_Text>().text += "SCORE \r\n";


            foreach (var score in scores)
            {
                _finalScoreUIPanelText.GetComponent<TMP_Text>().text +=
                    $"{score.PlayerName} : {score.OverallScore} \r\n";
            }
        }
    }
}

using Carcassone.ApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
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
                textComp.text = "Очки игрока " + playerName + "\r\n";
                var score = GameManager.Instance.RoomService.GetScore(playerName);
                textComp.text += $"Замки: {score.Castles}\r\n";
                textComp.text += $"Поля: {score.Cornfields}\r\n";
                textComp.text += $"Аббатства: {score.Churches}\r\n";
                textComp.text += $"Дороги: {score.Roads}\r\n";
            }
        }

        public void HideDetailedScore()
        {
            _playerDetailScorePanel.SetActive(false);
        }

        public void UpdateScore()
        {
            var players = GameManager.Instance.RoomService.GetPlayers();

            // вычислить очки
            var _playerToScore = new Dictionary<BasePlayer, PlayerScore>();
            foreach (var player in players)
            {
                var score = GameManager.Instance.RoomService.GetScore(player.Name);
                _playerToScore.Add(player, score);
            }

            foreach (var item in _playerToScore)
            {
                var player = item.Key;
                var score = item.Value;
                var playerScoreUI = _playersScorePanels[player.Name];

                var nameUI = playerScoreUI.transform.Find("PlayerName").GetComponent<Text>();
                nameUI.text = player.Name;

                var chipUI = playerScoreUI.transform.Find("ChipCount").GetComponent<Text>();
                chipUI.text = " Фишек:" + score.ChipCount;

                var overall = score.Castles + score.Cornfields + score.Churches + score.Roads;
                var result = overall + " / " +
                             " З:" + score.Castles +
                             " П:" + score.Cornfields +
                             " А:" + score.Churches +
                             " Д:" + score.Roads;

                playerScoreUI.transform.Find("InfoText").GetComponentInChildren<Text>().text = result;
                
            }
        }

        public void ShowEndGameWindow()
        {
            _finalScoreUIPanel.SetActive(true);

            var scores = GameManager.Instance.RoomService.GetGameScores();
            var winnerScore = scores.Max(x => x.FinalScore);
            var winners = scores
                .Where(x => x.FinalScore == winnerScore)
                .Select(x => x.UserName)
                .ToList();
            _finalScoreUIPanelText.GetComponent<TMP_Text>().text = $"The winner is {string.Join(",", winners)} \r\n";
            _finalScoreUIPanelText.GetComponent<TMP_Text>().text += "SCORE \r\n";
            foreach (var score in scores)
            {
                _finalScoreUIPanelText.GetComponent<TMP_Text>().text +=
                    $"{score.UserName} : {score.FinalScore} \r\n";
            }

            GameManager.Instance.RoomService.Reset();
        }
    }
}

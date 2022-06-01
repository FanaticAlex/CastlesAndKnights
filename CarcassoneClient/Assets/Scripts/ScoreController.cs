using Carcassone.ApiClient;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// Инициализирует панели игроков
        /// </summary>
        /// <param name="players"></param>
        public ScoreController(IEnumerable<Player> players)
        {
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
                    playerPanel.GetComponentInChildren<Image>().color = userColor;

                    remainPlayers.Remove(player);
                }
                else
                {
                    playerPanel.SetActive(false);
                }
            }
        }

        public void UpdateScore(Dictionary<Player, PlayerScore> allScore)
        {
            foreach(var item in allScore)
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
    }
}

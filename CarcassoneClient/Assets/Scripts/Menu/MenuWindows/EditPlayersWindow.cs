using Carcassone.Core.Players;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Menu
{
    public class EditPlayersWindow : BaseMenuWindow
    {
        public override MenuWindowType MenuPanelType => MenuWindowType.EditPlayers;

        public GameObject NewPlayerPanel;
        public GameObject PlayerTypeGO;
        public GameObject PlayerNameGO;
        public Transform PlayersList {  get; set; }

        public override void Enable()
        {
            base.Enable();

            PlayersList = GameObject.Find("PlayersList1")?.transform?.Find("Viewport")?.Find("Content");
            Assert.IsNotNull(PlayersList);
            Assert.IsNotNull(NewPlayerPanel);

            UpdatePlayerList(PlayersList);
        }

        public void OnBackBtnClick()
        {
            NewPlayerPanel.SetActive(false);
            MenuManager.SwitchToMenuPanel(MenuWindowType.SetupRoom);
        }

        public void OnCancelBtnClick()
        {
            NewPlayerPanel.SetActive(false);
        }

        public void OnNewPlayerBtnClick()
        {
            ShowNewPlayerPanel();
        }

        public void OnNewPlayerCreateAcceptedClick()
        {
            var typeStr = PlayerTypeGO.GetComponent<TMP_Dropdown>().captionText.text;
            var newPlayerType = PlayerTypeHelper.ToPlayerType(typeStr);
            var newPlayerName = PlayerNameGO.GetComponent<TMP_InputField>().text;

            GameManager.Instance.AddPlayer(newPlayerName, newPlayerType);
            NewPlayerPanel.SetActive(false);

            UpdatePlayerList(PlayersList);
        }

        private void ShowNewPlayerPanel()
        {
            var playerTypes = PlayerTypeHelper.GetStrings();
            var list = new List<TMP_Dropdown.OptionData>();
            PlayerTypeGO.GetComponent<TMP_Dropdown>().options.Clear();
            foreach (var playerType in playerTypes)
            {
                var option = new TMP_Dropdown.OptionData(playerType);
                list.Add(option);
            }
            PlayerTypeGO.GetComponent<TMP_Dropdown>().AddOptions(list);

            NewPlayerPanel.SetActive(true);
        }

        private void UpdatePlayerList(Transform playersListGO)
        {
            var playersList = GameManager.Instance.Players;

            // Remove Players
            foreach (Transform child in playersListGO)
                Destroy(child.gameObject);

            // Add Players
            for (var i = 0; i < playersList.Count; i++)
            {
                var player = playersList[i];
                var rowPrefab = (GameObject)Resources.Load("UI/PlayersListRow", typeof(GameObject));
                var row = GameObject.Instantiate(rowPrefab);
                row.transform.Find("NameText").GetComponent<Text>().text = player.Name;
                row.transform.Find("DeleteBtn").GetComponentInChildren<Button>().onClick.AddListener(delegate { OnDeletePlayerBtn(player.Name); });
                row.transform.Find("PlayerType").GetComponent<Text>().text = PlayerTypeHelper.ToString(player.PlayerType);
                row.transform.parent = playersListGO;
                row.transform.localScale = Vector3.one;
                var rowHight = row.GetComponent<RectTransform>().rect.height;
                row.transform.localPosition = new Vector3(0, -rowHight * i, 0);
            }
        }

        public void OnDeletePlayerBtn(string name)
        {
            GameManager.Instance.DeletePlayer(name);
            UpdatePlayerList(PlayersList);

            NewPlayerPanel.SetActive(false);
        }
    }
}

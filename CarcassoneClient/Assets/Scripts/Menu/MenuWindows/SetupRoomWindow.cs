using Carcassone.Core.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Menu
{
    /// <summary>
    /// Контроллер окна создания новой игры.
    /// * создание сетевой игры
    /// * подключение к сетевой игре
    /// * создание локальной игры
    /// </summary>
    internal class SetupRoomWindow : BaseMenuWindow
    {
        public GameObject AddPlayerBtn;
        public GameObject StartGameBtn;
        public GameObject NewPlayerPanel;

        public GameObject PlayerNameGO;

        public Transform PlayersListGO { get; set; }

        public override MenuWindowType MenuPanelType => MenuWindowType.SetupRoom;

        public override void Enable()
        {
            base.Enable();
            Assert.IsNotNull(AddPlayerBtn);
            Assert.IsNotNull(StartGameBtn);
            Assert.IsNotNull(NewPlayerPanel);
            Assert.IsNotNull(PlayerNameGO);

            PlayersListGO = GameObject.Find("PlayersList")?.transform?.Find("Viewport")?.Find("Content");
            Assert.IsNotNull(PlayersListGO);

            // добавляем себя
            if (GameManager.Instance.Room.PlayersPool.GamePlayers.Count == 0)
            {
                var defaultUser = GameManager.Instance.GetDefaultPlayer();
                GameManager.Instance.Room.PlayersPool.AddPlayer(defaultUser.Name, defaultUser.PlayerType);
            }
            
            UpdateUI();
        }

        public void UpdateUI()
        {
            UpdatePlayerList(PlayersListGO);

            var playersCount = GameManager.Instance.Room.PlayersPool.GamePlayers.Count();
            var humansPlayers = GameManager.Instance.Room.PlayersPool.GamePlayers.Where(p => p.PlayerType == PlayerType.Human).Count();
            StartGameBtn.GetComponent<Button>().interactable = (playersCount > 1) && (humansPlayers > 0);
            AddPlayerBtn.GetComponent<Button>().interactable = (playersCount < 5);
        }

        public void OnEditPlayersBtnClick()
        {
            MenuManager.SwitchToMenuPanel(MenuWindowType.EditPlayers);
        }

        // панель добавления игрока
        public void OnShowAddPlayerPanelBtnClick()
        {
            if (GameManager.Instance.Room.PlayersPool.GamePlayers.Count() >= 5)
                throw new Exception("Cant create player, 5 players max");

            GetSoundEffectsPlayer().PlayClick();
            ShowNewPlayerPanel();
        }

        // добавляем нового игрока
        public void OnAddPlayerBtnClick()
        {
            GetSoundEffectsPlayer().PlayClick();
            NewPlayerPanel.SetActive(false);

            if (GameManager.Instance.Room.PlayersPool.GamePlayers.Count() >= 5)
                throw new Exception("Cant create player, 5 players max");

            var playerName = PlayerNameGO.GetComponent<TMP_Dropdown>().captionText.text;
            var player = GameManager.Instance.GetPlayer(playerName);
            GameManager.Instance.Room.PlayersPool.AddPlayer(player.Name, player.PlayerType);

            UpdateUI();
        }

        public void OnAddPlayerCancelBtnClick()
        {
            GetSoundEffectsPlayer().PlayClick();
            NewPlayerPanel.SetActive(false);
        }

        public void OnStartGameBtnClick()
        {
            GetSoundEffectsPlayer().PlayClick();
            GameManager.Instance.Room.Start();
            SceneManager.LoadScene("RoomScene");
        }

        public void OnBackBtnClick()
        {
            GetSoundEffectsPlayer().PlayClick();
            MenuManager.SwitchToMenuPanel(MenuWindowType.Activities);
        }

        public void OnDeletePlayerBtn(string name)
        {
            GetSoundEffectsPlayer().PlayClick();
            GameManager.Instance.Room.PlayersPool.DeletePlayer(name);
            UpdateUI();
        }

        private void UpdatePlayerList(Transform playersListGO)
        {
            var playersList = GameManager.Instance.Room.PlayersPool.GamePlayers;

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

        private void ShowNewPlayerPanel()
        {
            var activePlayersNames = GameManager.Instance.Room.PlayersPool.GamePlayers.Select(p => p.Name);
            var freePlayers = PlayersManager.Load().Where(p => !activePlayersNames.Contains(p.Name));
            var list = new List<TMP_Dropdown.OptionData>();
            PlayerNameGO.GetComponent<TMP_Dropdown>().options.Clear();
            foreach (var player in freePlayers)
            {
                var option = new TMP_Dropdown.OptionData(player.Name);
                list.Add(option);
            }
            PlayerNameGO.GetComponent<TMP_Dropdown>().AddOptions(list);

            NewPlayerPanel.SetActive(true);
        }
    }
}

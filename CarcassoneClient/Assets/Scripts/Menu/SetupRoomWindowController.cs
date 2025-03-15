using Carcassone.Core.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
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
    internal class SetupRoomWindowController : BaseMenuWindowController
    {
        public GameObject AddPlayerBtn;
        public GameObject StartGameBtn;
        public GameObject NewPlayerPanel;

        public GameObject PlayerNameGO;
        public GameObject PlayerTypeGO;

        private float _timer;
        private readonly float _delta = 1.0f;

        private readonly Dictionary<string, GameObject> _rows = new();

        public override MenuWindowType MenuPanelType => MenuWindowType.SetupRoom;

        public override void Enable()
        {
            base.Enable();
            Assert.IsNotNull(AddPlayerBtn);
            Assert.IsNotNull(StartGameBtn);
            Assert.IsNotNull(NewPlayerPanel);
            Assert.IsNotNull(PlayerNameGO);
            Assert.IsNotNull(PlayerTypeGO);

            AddPlayerBtn.GetComponent<Button>().interactable = true;
            StartGameBtn.GetComponent<Button>().interactable = false;
            InitAddPlayerPanel();
            ClearPlayersList();

            // добавляем себя
            GameManager.Instance.RoomService.AddPlayer(UserManager.Instance.User.Name, PlayerType.Human);
            UpdatePlayerListFromServer();
        }

        // обновляем интерфейс если есть игроки подключенные по сети
        void Update()
        {
            _timer += Time.deltaTime;
            if (_timer > _delta)
            {
                //if (!MenuManager.IAmGameMaster) // для подключившихся игроков
                {
                    // ждем пока игра стартует
                    //var room = GameManager.Instance.RoomService.GetRoom();
                    //if (room.IsStarted)
                    //    SceneManager.LoadScene("RoomScene");
                }

                UpdatePlayerListFromServer();
                _timer = 0;
            }
        }

        public void OnShowAddPlayerPanelBtnClick()
        {
            if (_rows.Count >= 5)
                throw new Exception("Cant create player, 5 players max");

            NewPlayerPanel.SetActive(true);

            PlayerNameGO.GetComponent<TMP_InputField>().text = GenratePlayerName();
            PlayerTypeGO.GetComponent<TMP_Dropdown>().SetValueWithoutNotify(0); // вручную можно добавлять только игроков компьютеров
        }

        // добавляем нового игрока
        public void OnAddPlayerBtnClick()
        {
            NewPlayerPanel.SetActive(false);

            if (_rows.Count >= 5)
                return;

            var newName = PlayerNameGO.GetComponent<TMP_InputField>().text;
            var newType = PlayerTypeHelper.ToPlayerType(PlayerTypeGO.GetComponent<TMP_Dropdown>().captionText.text);
            GameManager.Instance.RoomService.AddPlayer(newName, newType);
        }

        public void OnAddPlayerCancelBtnClick()
        {
            NewPlayerPanel.SetActive(false);
        }

        public void OnDeletePlayerBtn(string name)
        {
            GameManager.Instance.RoomService.DeletePlayer(name);
        }

        public void OnStartGameBtnClick()
        {
            GameManager.Instance.RoomService.Start();
            SceneManager.LoadScene("RoomScene");
        }

        public void OnBackBtnClick()
        {
            MenuManager.SwitchToMenuPanel(MenuWindowType.Profile);
        }

        private void UpdatePlayerListFromServer()
        {
            Transform playersListGO = GetPlayersList();

            var playersList = GameManager.Instance.RoomService.GetPlayers();
            RemoveDeletedPlayers(playersList);
            for (var i = 0; i < playersList.Count; i++)
            {
                var player = playersList[i];
                if (!_rows.Keys.Contains(player.Name)) // Add row
                {
                    var rowPrefab = (GameObject)Resources.Load("UI/PlayersListRow", typeof(GameObject));
                    var row = GameObject.Instantiate(rowPrefab);
                    row.transform.Find("NameText").GetComponent<Text>().text = player.Name;
                    row.transform.Find("DeleteBtn").GetComponentInChildren<Button>().onClick.AddListener(delegate { OnDeletePlayerBtn(player.Name); });
                    
                    if (player.PlayerType == PlayerType.Human)
                        row.transform.Find("PlayerType").GetComponent<Text>().text = "human";
                    if (player.PlayerType == PlayerType.AI_Easy) 
                        row.transform.Find("PlayerType").GetComponent<Text>().text = "AI_easy";
                    if (player.PlayerType == PlayerType.AI_Normal)
                        row.transform.Find("PlayerType").GetComponent<Text>().text = "AI_normal";
                    if (player.PlayerType == PlayerType.AI_Hard)
                        row.transform.Find("PlayerType").GetComponent<Text>().text = "AI_hard";

                    row.transform.parent = playersListGO;
                    row.transform.localScale = Vector3.one;
                    var rowHight = row.GetComponent<RectTransform>().rect.height;
                    row.transform.localPosition = new Vector3(0, - rowHight * i, 0);

                    _rows.Add(player.Name, row);
                }
            }

            var canStart = (playersList.Count() > 1);
            StartGameBtn.GetComponent<Button>().interactable = canStart;
        }

        private void RemoveDeletedPlayers(List<BasePlayer> playersList)
        {
            var names = playersList.Select(p => p.Name).ToList();
            foreach (var row in _rows.ToList())
            {
                if (!names.Contains(row.Key))
                {
                    GameObject.Destroy(row.Value);
                    _rows.Remove(row.Key);
                }
            }
        }

        private static Transform GetPlayersList()
        {
            var playersListGO = GameObject.Find("PlayersList")?.transform?.Find("Viewport")?.Find("Content");
            if (playersListGO == null)
                throw new Exception("PlayersList GameObject Not Found");
            
            return playersListGO;
        }

        private static void ClearPlayersList()
        {
            var playersListGO = GetPlayersList();
            foreach (Transform child in playersListGO.transform)
                GameObject.Destroy(child.gameObject);
        }

        private void InitAddPlayerPanel()
        {
            PlayerTypeGO.GetComponent<TMP_Dropdown>().options.Clear();
            var option1 = new TMP_Dropdown.OptionData(PlayerTypeHelper.ToString(PlayerType.AI_Easy));
            var option2 = new TMP_Dropdown.OptionData(PlayerTypeHelper.ToString(PlayerType.AI_Normal));
            var option3 = new TMP_Dropdown.OptionData(PlayerTypeHelper.ToString(PlayerType.AI_Hard));
            var list = new List<TMP_Dropdown.OptionData> { option1, option2, option3 };
            PlayerTypeGO.GetComponent<TMP_Dropdown>().AddOptions(list);

            NewPlayerPanel.SetActive(false);
        }

        private string GenratePlayerName()
        {
            var playersNames = GameManager.Instance.RoomService.GetPlayers().Select(p => p.Name);
            for (var i = 0; i < 5; i++)
            {
                var name = $"Player_{i}";
                if (!playersNames.Contains(name))
                    return name;
            }

            throw new Exception("No free names left");
        }
    }
}

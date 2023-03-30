using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Menu
{
    /// <summary>
    /// Контроллер окна создания новой игры.
    /// *Список игроков
    /// *добавить AI
    /// *начать игру
    /// </summary>
    internal class SetupRoomWindowController : BaseMenuWindowController
    {
        public GameObject AddAIPlayerBtn;
        public GameObject StartGameBtn;

        private float _timer;
        private float _delta = 0.5f;

        public override MenuWindowType MenuPanelType => MenuWindowType.SetupRoom;

        public override void Enable()
        {
            base.Enable();

            if (MenuManager.IAmGameMaster)
            {
                // если мы создаем игру
                AddAIPlayerBtn.SetActive(true);
                StartGameBtn.SetActive(true);
            }
            else
            {
                // если мы подключились
                AddAIPlayerBtn.SetActive(false);
                StartGameBtn.SetActive(false);
                MenuManager.IsWaitingForStart = true;
            }

            InitPlayersListGO();
        }

        void Update()
        {
            _timer += Time.deltaTime;
            if (_timer > _delta)
            {
                WaitingForStart();
                UpdatePlayerList();
                _timer = 0;
            }
        }

        public void OnAddAIBtnClick()
        {
            GameManager.Instance.RoomService.AddAI();
        }

        public void OnDeletePlayerBtn(string name)
        {
            //RemoteRoom.DeletePlayer(name);
        }

        private void WaitingForStart()
        {
            if (MenuManager.IsWaitingForStart)
            {
                // тут мы подключились к игре и ждем пока она стартает
                var room = GameManager.Instance.RoomService.GetRoom();
                if (room.IsStarted)
                    SceneManager.LoadScene("RoomScene");
            }
        }

        public void OnStartGameBtnClick()
        {
            GameManager.Instance.RoomService.Start();
            SceneManager.LoadScene("RoomScene");
        }

        public void OnBackBtnClick()
        {
            MenuManager.SwitchToMenuPanel(MenuWindowType.Profile);
            MenuManager.IsWaitingForStart = false;
        }

        private void UpdatePlayerList()
        {
            Transform playersListGO = InitPlayersListGO();

            var pos = 0.0f;
            var playersList = GameManager.Instance.RoomService.GetPlayers();
            foreach (var player in playersList)
            {
                var rowPrefab = (GameObject)Resources.Load("UI/PlayersListRow", typeof(GameObject));
                var row = GameObject.Instantiate(rowPrefab);
                row.transform.Find("NameText").GetComponent<Text>().text = player.Name;
                row.transform.Find("DeleteBtn").GetComponentInChildren<Button>().onClick.AddListener(delegate { OnDeletePlayerBtn(player.Name); });

                row.transform.parent = playersListGO;
                row.transform.localScale = Vector3.one;
                row.transform.localPosition = new Vector3(0, pos, 0);

                var rowHight = row.GetComponent<RectTransform>().rect.height;

                pos -= rowHight;
            }
        }

        private static Transform InitPlayersListGO()
        {
            var playersListGO = GameObject.Find("PlayersList")?.transform?.Find("Viewport")?.Find("Content");
            if (playersListGO == null)
                throw new Exception("PlayersList GameObject Not Found");

            foreach (Transform child in playersListGO.transform)
                GameObject.Destroy(child.gameObject);

            return playersListGO;
        }
    }
}

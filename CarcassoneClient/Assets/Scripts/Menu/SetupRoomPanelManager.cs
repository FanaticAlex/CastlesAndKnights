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
    /// *Список игроков
    /// *добавить AI
    /// *начать игру
    /// </summary>
    internal class SetupRoomPanelManager : BaseMenuManager
    {
        // SetupRoomPanel
        public GameObject AddAIPlayerBtn;
        public GameObject StartGameBtn;

        private float _timer;
        private float _delta = 3f;

        public override MenuPanel MenuPanel => MenuPanel.SetupRoom;

        public override void Enable()
        {
            base.Enable();

            if (MainMenu.IAmGameMaster)
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
                MainMenu.IsWaitingForStart = true;
            }
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
            if (MainMenu.IsWaitingForStart)
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
            MainMenu.SwitchToMenuPanel(MenuPanel.Profile);
            MainMenu.IsWaitingForStart = false;
        }

        private void UpdatePlayerList()
        {
            var listUi = GameObject.Find("PlayersList")?.transform?.Find("Viewport")?.Find("Content");
            if (listUi == null)
                return;

            foreach (Transform child in listUi.transform)
                GameObject.Destroy(child.gameObject);

            var pos = 0;
            var playersList = GameManager.Instance.RoomService.GetPlayers();
            foreach (var player in playersList)
            {
                var rowPrefab = (GameObject)Resources.Load("Additional/PlayersListRow", typeof(GameObject));
                var row = GameObject.Instantiate(rowPrefab);
                row.transform.Find("NameText").GetComponent<Text>().text = player.Name;
                row.transform.Find("DeleteBtn").GetComponentInChildren<Button>().onClick.AddListener(delegate { OnDeletePlayerBtn(player.Name); });

                row.transform.parent = listUi;
                row.transform.localScale = Vector3.one;
                row.transform.localPosition = new Vector3(0, pos, 0);
                pos -= 30;
            }
        }
    }
}

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
    public class ChooseRoomPanelManager : BaseMenuManager
    {
        private float _timer;
        private float _delta = 3f;

        void Update()
        {
            _timer += Time.deltaTime;
            if (_timer > _delta)
            {
                UpdateRoomsList();
                _timer = 0;
            }
        }

        public override MenuPanel MenuPanel => MenuPanel.ChooseRoom;

        public void OnBackBtnClick()
        {
            MainMenu.IsWaitingForStart = false;
            MainMenu.SwitchToMenuPanel(MenuPanel.Profile);
        }

        public void OnConnectToGameBtnClick(string roomId)
        {
            var user = GameManager.Instance.RoomService.User;
            GameManager.Instance.RoomService.Connect(roomId);
            GameManager.Instance.RoomService.AddHuman(user.Login);

            // мы подключились к игре, но добавлять игроков и начинать игру мы не можем
            MainMenu.SwitchToMenuPanel(MenuPanel.SetupRoom);
            MainMenu.IAmGameMaster = false;
        }

        private void UpdateRoomsList()
        {
            var listUi = GameObject.Find("GamesList")?.transform?.Find("Viewport")?.Find("Content");
            if (listUi == null)
                return;

            foreach (Transform child in listUi.transform)
                GameObject.Destroy(child.gameObject);

            var pos = 0;
            var roomsList = GameManager.Instance.RoomService.GetRoomsIds();
            foreach (var roomId in roomsList)
            {
                var rowPrefab = (GameObject)Resources.Load("Additional/RoomsListRow", typeof(GameObject));
                var row = GameObject.Instantiate(rowPrefab);
                row.transform.Find("NameText").GetComponent<Text>().text = roomId;
                row.transform.Find("ConnectBtn").GetComponentInChildren<Button>().onClick.AddListener(delegate { OnConnectToGameBtnClick(roomId); });
                //row.transform.Find("DeleteBtn").GetComponentInChildren<Button>().onClick.AddListener(delegate { OnDeleteRoomBtn(roomId); });
                row.transform.parent = listUi;
                row.transform.localPosition = new Vector3(0, pos, 0);
                pos -= 30;
            }
        }
    }
}

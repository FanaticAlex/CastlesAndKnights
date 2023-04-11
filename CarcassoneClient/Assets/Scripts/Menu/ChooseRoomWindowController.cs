using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Menu
{
    /// <summary>
    /// Контроллер окна поиска игровой комнаты.
    /// </summary>
    public class ChooseRoomWindowController : BaseMenuWindowController
    {
        private float _timer;

        void Update()
        {
            if (_timer <= 0)
            {
                UpdateRoomsList();
                _timer = 3f;
            }

            _timer -= Time.deltaTime;
        }

        public override MenuWindowType MenuPanelType => MenuWindowType.ChooseRoom;

        public void OnBackBtnClick()
        {
            MenuManager.IsWaitingForStart = false;
            MenuManager.SwitchToMenuPanel(MenuWindowType.Profile);
        }

        public void OnConnectToGameBtnClick(string roomId)
        {
            var user = GameManager.Instance.RoomService.User;
            GameManager.Instance.RoomService.Connect(roomId);
            GameManager.Instance.RoomService.AddHuman(user);

            // мы подключились к игре, но добавлять игроков и начинать игру мы не можем
            MenuManager.SwitchToMenuPanel(MenuWindowType.SetupRoom);
            MenuManager.IAmGameMaster = false;
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
                var rowPrefab = (GameObject)Resources.Load("UI/RoomsListRow", typeof(GameObject));
                var row = GameObject.Instantiate(rowPrefab);
                row.transform.Find("NameText").GetComponent<Text>().text = roomId;
                row.transform.Find("ConnectBtn").GetComponentInChildren<Button>().onClick.AddListener(delegate { OnConnectToGameBtnClick(roomId); });
                row.transform.parent = listUi;
                row.transform.localScale = Vector3.one;
                row.transform.localPosition = new Vector3(0, pos, 0);
                pos -= 30;
            }
        }
    }
}

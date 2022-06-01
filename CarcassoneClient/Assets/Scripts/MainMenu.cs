using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum MenuPanel
{
    Login,
    ChooseRoom,
    SetupRoom
}

/// <summary>
/// Скрипт обработки событий меню
/// </summary>
public class MainMenu : MonoBehaviour
{
    private MenuPanel _currentPanel;
    private float _timer;
    float _delta = 3f;

    public GameObject LoginPanelObj;
    public GameObject ChooseRoomPanelObj;
    public GameObject SetupRoomPanelObj;

    public GameObject LoginBtn;
    public GameObject ErrorText;

    void Start()
    {
        // Инициализация меню
        LoginPanelObj.SetActive(true);
        ChooseRoomPanelObj.SetActive(false);
        SetupRoomPanelObj.SetActive(false);
        ErrorText.SetActive(false);
        LoginBtn.SetActive(false);
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > _delta)
        {
            CheckConnection();
            UpdateRoomsList();
            UpdatePlayerList();
            _timer = 0;
        }
    }

    /// <summary>
    /// Обработчик конопки "логин"
    /// </summary>
    public void OnLoginBtnClick()
    {
        var login = GameObject.Find("Login").GetComponent<TMP_InputField>().text;
        var password = GameObject.Find("Password").GetComponent<TMP_InputField>().text;
        try
        {
            var user = RoomService.Instance.Client.LoginAsync(login, password).Result;
            RoomService.Instance.User = user;
            SwitchToMenuPanel(MenuPanel.ChooseRoom);
        }
        catch
        {
            ErrorText.GetComponent<TMP_Text>().text = "Login or password is incorrect";
            ErrorText.SetActive(true);
        }
    }

    /// <summary>
    /// Обработчик кнопки "назад"
    /// </summary>
    public void OnBackBtnClick()
    {
        SwitchToMenuPanel(_currentPanel - 1);
    }

    /// <summary>ы
    /// Обработчик кнопки "новая игра"
    /// </summary>
    public void OnNewGameBtnClick()
    {
        var room = RoomService.Instance.Client.CreateAsync().Result;
        RoomService.Instance.RoomId = room.Id;
        RoomService.Instance.Client.AddHumanAsync(RoomService.Instance.RoomId, RoomService.Instance.User.Login);
        SwitchToMenuPanel(MenuPanel.SetupRoom);
    }

    public void OnStartGameBtnClick()
    {
        RoomService.Instance.Client.StartAsync(RoomService.Instance.RoomId).Wait();
        SceneManager.LoadScene("RoomScene");
    }

    public void OnDeletePlayerBtn(string name)
    {
        //RemoteRoom.DeletePlayer(name);
    }

    public void OnAddAIBtnClick()
    {
        RoomService.Instance.Client.AddAIAsync(RoomService.Instance.RoomId).Wait();
    }

    private void UpdateRoomsList()
    {
        var listUi = GameObject.Find("GamesList")?.transform?.Find("Viewport")?.Find("Content");
        if (listUi == null)
            return;

        foreach (Transform child in listUi.transform)
            GameObject.Destroy(child.gameObject);

        var pos = 0;
        var roomsList = RoomService.Instance.Client.ListAsync().Result;
        foreach (var roomId in roomsList)
        {
            var rowPrefab = (GameObject)Resources.Load("Additional/RoomsListRow", typeof(GameObject));
            var row = GameObject.Instantiate(rowPrefab);
            row.transform.Find("NameText").GetComponent<Text>().text = roomId;
            //row.transform.Find("ConnectBtn").GetComponentInChildren<Button>().onClick.AddListener(delegate { OnEnterRoomBtn(roomId); });
            //row.transform.Find("DeleteBtn").GetComponentInChildren<Button>().onClick.AddListener(delegate { OnDeleteRoomBtn(roomId); });
            row.transform.parent = listUi;
            row.transform.localPosition = new Vector3(0, pos, 0);
            pos -= 30;
        }
    }

    private void UpdatePlayerList()
    {
        var listUi = GameObject.Find("PlayersList")?.transform?.Find("Viewport")?.Find("Content");
        if (listUi == null)
            return;

        foreach (Transform child in listUi.transform)
            GameObject.Destroy(child.gameObject);

        var pos = 0;
        var playersList = RoomService.Instance.Client.List2Async(RoomService.Instance.RoomId).Result;
        foreach (var player in playersList)
        {
            var rowPrefab = (GameObject)Resources.Load("Additional/PlayersListRow", typeof(GameObject));
            var row = GameObject.Instantiate(rowPrefab);
            row.transform.Find("NameText").GetComponent<Text>().text = player.Name;
            row.transform.Find("DeleteBtn").GetComponentInChildren<Button>().onClick.AddListener(delegate { OnDeletePlayerBtn(player.Name); });

            row.transform.parent = listUi;
            row.transform.localPosition = new Vector3(0, pos, 0);
            pos -= 30;
        }
    }

    private void SwitchToMenuPanel(MenuPanel menu)
    {
        if (menu == MenuPanel.Login)
        {
            _currentPanel = MenuPanel.Login;
            LoginPanelObj.SetActive(true);
            ChooseRoomPanelObj.SetActive(false);
            SetupRoomPanelObj.SetActive(false);
        }

        if (menu == MenuPanel.ChooseRoom)
        {
            _currentPanel = MenuPanel.ChooseRoom;
            LoginPanelObj.SetActive(false);
            ChooseRoomPanelObj.SetActive(true);
            SetupRoomPanelObj.SetActive(false);
        }

        if (menu == MenuPanel.SetupRoom)
        {
            _currentPanel = MenuPanel.SetupRoom;
            LoginPanelObj.SetActive(false);
            ChooseRoomPanelObj.SetActive(false);
            SetupRoomPanelObj.SetActive(true);
        }
    }

    private void CheckConnection()
    {
        try
        {
            var result = RoomService.Instance.Client.ListAsync().Result;
            ErrorText.SetActive(false);
            LoginBtn.SetActive(true);
        }
        catch
        {
            ErrorText.GetComponent<TMP_Text>().text = "Server is not available";
            ErrorText.SetActive(true);
            LoginBtn.SetActive(false);
        }
    }
}
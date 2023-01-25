using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Menu
{
    /// <summary>
    /// Панель логина игрока:
    /// *логин
    /// *пароль
    ///     Войти
    /// оффлайн игра
    /// </summary>
    public class LoginPanelManager : BaseMenuManager
    {
        // LoginPanel
        public GameObject LoginBtn;
        public GameObject ErrorText;

        private float _timer;
        private float _delta = 3f;
        private bool _stopCheckConnection;

        void Update()
        {
            _timer += Time.deltaTime;
            if (_timer > _delta)
            {
                CheckConnection();
                _timer = 0;
            }
        }

        public override MenuPanel MenuPanel => MenuPanel.Login;

        public override void Enable()
        {
            base.Enable();

            ErrorText.SetActive(false);
            LoginBtn.SetActive(false);
        }

        public void OnLoginBtnClick()
        {
            var login = GameObject.Find("Login").GetComponent<TMP_InputField>().text;
            var password = GameObject.Find("Password").GetComponent<TMP_InputField>().text;
            try
            {
                GameManager.Instance.SetOnlineMode();
                GameManager.Instance.RoomService.Login(login, password);
            }
            catch
            {
                ErrorText.GetComponent<TMP_Text>().text = "Login or password is incorrect";
                ErrorText.SetActive(true);
                return;
            }

            MainMenu.SwitchToMenuPanel(MenuPanel.Profile);
        }

        public void OnOfflineGameBtnClick()
        {
            _stopCheckConnection = true;
            GameManager.Instance.SetOfflineMode();
            MainMenu.SwitchToMenuPanel(MenuPanel.SetupRoom);
        }

        private void CheckConnection()
        {
            if (_stopCheckConnection)
            {
                ErrorText.SetActive(false);
                return;
            }

            try
            {
                var result = GameManager.Instance.RoomService.GetRoomsIds();
                ErrorText.SetActive(false);
                LoginBtn.SetActive(true);
            }
            catch (Exception e)
            {
                ErrorText.GetComponent<TMP_Text>().text = "Server is not available";
                ErrorText.SetActive(true);
                LoginBtn.SetActive(false);
                Debug.LogException(e);
            }
        }
    }
}

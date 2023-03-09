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
    /// Контроллер окна логина
    /// </summary>
    public class LoginWindowController : BaseMenuWindowController
    {
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

        public override MenuWindowType MenuPanelType => MenuWindowType.Login;

        public override void Enable()
        {
            base.Enable();
            ErrorText.SetActive(false);
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
            catch (Exception e)
            {
                ErrorText.GetComponent<TMP_Text>().text = $"Error: {e.Message}";
                ErrorText.SetActive(true);
                return;
            }

            MenuManager.SwitchToMenuPanel(MenuWindowType.Profile);
        }

        public void OnOfflineGameBtnClick()
        {
            _stopCheckConnection = true;
            GameManager.Instance.SetOfflineMode();
            MenuManager.SwitchToMenuPanel(MenuWindowType.Profile);
        }

        public void OnExitBtnClick()
        {
            Application.Quit();
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
                ErrorText.GetComponent<TMP_Text>().text = "";
                ErrorText.SetActive(false);
            }
            catch (AggregateException e)
            {
                ErrorText.GetComponent<TMP_Text>().text = $"Error: {e.InnerException.Message}";
                ErrorText.SetActive(true);
                Debug.LogException(e);
            }
            catch (Exception e)
            {
                ErrorText.GetComponent<TMP_Text>().text = $"Error: {e.Message}";
                ErrorText.SetActive(true);
                Debug.LogException(e);
            }
        }
    }
}

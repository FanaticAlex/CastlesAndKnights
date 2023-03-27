using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Menu
{
    /// <summary>
    /// Контроллер окна логина
    /// </summary>
    public class LoginWindowController : BaseMenuWindowController
    {
        public GameObject ErrorText;

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

            // делаем несколько попыток, если не получилось выдаем ошибку
            var maxTryCount = 10;
            var tryNumber = 1;
            while (true)
            {
                var errorMessage = string.Empty;
                if (TryLogin(login, password, out errorMessage))
                {
                    MenuManager.SwitchToMenuPanel(MenuWindowType.Profile);
                    ErrorText.GetComponent<TMP_Text>().text = string.Empty;
                    ErrorText.SetActive(false);
                    break;
                }

                if (tryNumber >= maxTryCount)
                {
                    ErrorText.GetComponent<TMP_Text>().text = $"Error: {errorMessage}";
                    ErrorText.SetActive(true);
                    break;
                }

                tryNumber++;
            }
        }

        private bool TryLogin(string login, string password, out string message)
        {
            try
            {
                GameManager.Instance.SetOnlineMode();
                GameManager.Instance.RoomService.Login(login, password);
                message = String.Empty;
                return true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                message = e.InnerException?.Message ?? e.Message;
                return false;
            }
        }

        public void OnOfflineGameBtnClick()
        {
            GameManager.Instance.SetOfflineMode();
            MenuManager.SwitchToMenuPanel(MenuWindowType.Profile);
        }

        public void OnExitBtnClick()
        {
            Application.Quit();
        }

        public void OnRegisterBtnClick()
        {
            Application.OpenURL("http://192.168.1.65/Identity/Account/Register");
        }
    }
}

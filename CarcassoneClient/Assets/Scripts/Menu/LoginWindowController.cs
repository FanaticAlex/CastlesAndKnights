using Assets.Scripts.Game;
using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Menu
{
    public class LoginWindowController : BaseMenuWindowController
    {
        public override MenuWindowType MenuPanelType => MenuWindowType.Login;

        public override void Enable()
        {
            base.Enable();
            ConnectToServer();
        }

        private static void ConnectToServer()
        {
            try
            {
                var data = CarcassonePrefs.GetSavedAuthData();
                GameManager.Instance.SetOnlineMode();
                GameManager.Instance.RoomService.Login(data);
                MenuManager.SwitchToMenuPanel(MenuWindowType.Profile);
            }
            catch (Exception e)
            {
                Logger.Info(e.Message); // token expired
            }
        }

        public void OnLoginBtnClick()
        {
            var login = GameObject.Find("Login").GetComponent<TMP_InputField>().text.Trim();
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
                    break;
                }

                if (tryNumber >= maxTryCount)
                {
                    Logger.Info($"Error: {errorMessage}");
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
                message = e.InnerException?.Message ?? e.Message;
                return false;
            }
        }

        public void OnOfflineGameBtnClick()
        {
            GameManager.Instance.SetOfflineMode();
            MenuManager.SwitchToMenuPanel(MenuWindowType.SetupRoom);
        }

        public void OnExitBtnClick()
        {
            Application.Quit();
        }

        public void OnRegisterBtnClick()
        {
            Application.OpenURL("http://192.168.1.65/Identity/Account/Register");
        }

        public void OnEyeBtnClick()
        {
            var passwordField = GameObject.Find("Password").GetComponent<TMP_InputField>();
            if (passwordField.contentType == TMP_InputField.ContentType.Password)
                passwordField.contentType = TMP_InputField.ContentType.Standard;
            else
                passwordField.contentType = TMP_InputField.ContentType.Password;

            passwordField.Select();
        }
    }
}

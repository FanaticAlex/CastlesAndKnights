using Assets.Scripts.Game;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Menu
{
    /// <summary>
    /// Контроллер окна информации об игроке
    /// </summary>
    internal class ProfileWindowController : BaseMenuWindowController
    {
        public GameObject UserName;
        public GameObject UserInfo;

        public override void Enable()
        {
            base.Enable();

            var user = new User();
            var userInfo = user.GetUserInfo();
            UserName.GetComponent<TMP_Text>().text = user.Name;
            UserInfo.GetComponent<TMP_Text>().text = 
                $"GamesCount: {userInfo.GamesCount} \r\n" +
                $"WinCount: {userInfo.WinCount}";
        }

        public override MenuWindowType MenuPanelType => MenuWindowType.Profile;

        public void OnFindGameBtnClick()
        {
            MenuManager.SwitchToMenuPanel(MenuWindowType.ChooseRoom);
        }

        public void OnSingleGameBtnClick()
        {
            GameManager.Instance.SetOfflineMode();
            MenuManager.SwitchToMenuPanel(MenuWindowType.SetupRoom);
        }

        public void OnLogoutBtnClick()
        {
            CarcassonePrefs.DeleteSavedAuthData();
            MenuManager.SwitchToMenuPanel(MenuWindowType.Login);
        }
    }
}

using Assets.Scripts.Game;
using Carcassone.Core.Players;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Menu
{
    /// <summary>
    /// Контроллер окна информации об игроке
    /// </summary>
    internal class ProfileWindow : BaseMenuWindow
    {
        public GameObject UserName;
        public GameObject UserInfo;

        public override void Enable()
        {
            base.Enable();

            var user = new Player();
            UserName.GetComponent<TMP_Text>().text = user.Name;
            UserInfo.GetComponent<TMP_Text>().text = 
                $"GamesCount: {user.GamesCount} \r\n" +
                $"WinCount: {user.WinCount}";
        }

        public override MenuWindowType MenuPanelType => MenuWindowType.Profile;

        public void OnFindGameBtnClick()
        {
            MenuManager.SwitchToMenuPanel(MenuWindowType.ChooseRoom);
        }

        public void OnSingleGameBtnClick()
        {
            MenuManager.SwitchToMenuPanel(MenuWindowType.SetupRoom);
        }

        public void OnLogoutBtnClick()
        {
            CarcassonePrefs.DeleteSavedAuthData();
            MenuManager.SwitchToMenuPanel(MenuWindowType.Login);
        }
    }
}

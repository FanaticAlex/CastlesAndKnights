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
        public GameObject Statistic;

        public override void Enable()
        {
            base.Enable();

            var user = GameManager.Instance.RoomService.HumanUser;
            var statistic = GameManager.Instance.RoomService.GetUserStatistic(user);
            UserName.GetComponent<TMP_Text>().text = user;
            Statistic.GetComponent<TMP_Text>().text = 
                $"GamesCount: {statistic.GamesCount} \r\n" +
                $"WinCount: {statistic.WinCount}";
        }

        public override MenuWindowType MenuPanelType => MenuWindowType.Profile;

        public void OnFindGameBtnClick()
        {
            MenuManager.SwitchToMenuPanel(MenuWindowType.ChooseRoom);
        }

        public void OnNewGameBtnClick()
        {
            GameManager.Instance.RoomService.Create();
            MenuManager.IAmGameMaster = true;
            MenuManager.SwitchToMenuPanel(MenuWindowType.SetupRoom);
        }

        public void OnLogoutBtnClick()
        {
            CarcassonePrefs.DeleteSavedAuthData();
            MenuManager.SwitchToMenuPanel(MenuWindowType.Login);
        }
    }
}

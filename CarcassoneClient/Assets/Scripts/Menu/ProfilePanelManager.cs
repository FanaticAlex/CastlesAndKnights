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
    /// Информация об игроке
    /// *новая игра
    /// *подключиться к игре
    /// </summary>
    internal class ProfilePanelManager : BaseMenuManager
    {
        // ProfilePanel
        public GameObject UserName;
        public GameObject Statistic;

        public override void Enable()
        {
            base.Enable();

            var user = GameManager.Instance.RoomService.User;
            UserName.GetComponent<TMP_Text>().text = user.Login;
            Statistic.GetComponent<TMP_Text>().text = $"Raiting: {user.Raiting}";
        }

        public override MenuPanel MenuPanel => MenuPanel.Profile;

        public void OnFindGameBtnClick()
        {
            MainMenu.SwitchToMenuPanel(MenuPanel.ChooseRoom);
        }

        public void OnNewGameBtnClick()
        {
            GameManager.Instance.RoomService.Create();
            var user = GameManager.Instance.RoomService.User;
            GameManager.Instance.RoomService.AddHuman(user.Login);

            MainMenu.IAmGameMaster = true;
            MainMenu.SwitchToMenuPanel(MenuPanel.SetupRoom);
        }

        public void OnBackBtnClick()
        {
            MainMenu.SwitchToMenuPanel(MenuPanel.Login);
            MainMenu.IsWaitingForStart = false;
        }
    }
}

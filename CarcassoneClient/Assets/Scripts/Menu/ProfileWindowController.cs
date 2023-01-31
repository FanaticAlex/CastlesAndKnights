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
    /// Контроллер окна информации об игроке
    /// *новая игра
    /// *подключиться к игре
    /// </summary>
    internal class ProfileWindowController : BaseMenuWindowController
    {
        public GameObject UserName;
        public GameObject Statistic;

        public override void Enable()
        {
            base.Enable();

            var user = GameManager.Instance.RoomService.User;
            UserName.GetComponent<TMP_Text>().text = user.Login;
            Statistic.GetComponent<TMP_Text>().text = $"Raiting: {user.Raiting}";
        }

        public override MenuWindowType MenuPanelType => MenuWindowType.Profile;

        public void OnFindGameBtnClick()
        {
            MenuManager.SwitchToMenuPanel(MenuWindowType.ChooseRoom);
        }

        public void OnNewGameBtnClick()
        {
            GameManager.Instance.RoomService.Create();
            var user = GameManager.Instance.RoomService.User;
            GameManager.Instance.RoomService.AddHuman(user.Login);

            MenuManager.IAmGameMaster = true;
            MenuManager.SwitchToMenuPanel(MenuWindowType.SetupRoom);
        }

        public void OnBackBtnClick()
        {
            MenuManager.SwitchToMenuPanel(MenuWindowType.Login);
            MenuManager.IsWaitingForStart = false;
        }
    }
}

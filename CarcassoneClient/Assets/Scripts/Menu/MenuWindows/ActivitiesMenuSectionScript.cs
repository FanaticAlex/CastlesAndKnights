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
    internal class ActivitiesMenuSectionScript : BaseMenuWindow
    {
        public override void Enable()
        {
            base.Enable();
        }

        public override MenuWindowType MenuPanelType => MenuWindowType.Activities;

        public void OnSingleGameBtnClick()
        {
            MenuManager.SwitchToMenuPanel(MenuWindowType.SetupRoom);
        }

        public void OnExitBtnClick()
        {
            Application.Quit();
        }
    }
}

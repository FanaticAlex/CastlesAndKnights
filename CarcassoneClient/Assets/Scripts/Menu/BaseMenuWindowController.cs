using UnityEngine;

namespace Assets.Scripts.Menu
{

    /// <summary>
    /// Базовый класс скрипта окна игрового меню
    /// </summary>
    public class BaseMenuWindowController : MonoBehaviour
    {
        /// <summary>
        /// Тип панели
        /// </summary>
        public virtual MenuWindowType MenuPanelType { get; }

        /// <summary>
        /// Кнопка назад, есть во всех типах окон
        /// </summary>
        public GameObject BackBtn;

        public virtual void Disable()
        {
            gameObject.SetActive(false);
        }

        public virtual void Enable()
        {
            gameObject.SetActive(true);
        }

        public bool IsEnabled() => gameObject.activeSelf;
    }
}

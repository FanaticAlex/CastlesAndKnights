using UnityEngine;

namespace Assets.Scripts.Menu
{

    /// <summary>
    /// MenuWindow that can be enabled and disabled.
    /// </summary>
    public class BaseMenuWindow : MonoBehaviour
    {
        /// <summary>
        /// Тип панели
        /// </summary>
        public virtual MenuWindowType MenuPanelType { get; }

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Menu
{
    public enum MenuPanel
    {
        Login,
        Profile,
        ChooseRoom,
        SetupRoom
    }

    public class BaseMenuManager : MonoBehaviour
    {
        public virtual MenuPanel MenuPanel { get; }

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

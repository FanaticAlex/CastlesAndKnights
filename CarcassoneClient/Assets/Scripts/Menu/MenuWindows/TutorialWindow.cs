using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Menu
{
    internal class TutorialWindow : BaseMenuWindow
    {
        public List<Sprite> TutorialSprites;
        public GameObject Slide;

        public override MenuWindowType MenuPanelType => MenuWindowType.Tutorial;

        private int _currentSlideIndex;

        public override void Enable()
        {
            base.Enable();
            _currentSlideIndex = 0;
            SetSlide(_currentSlideIndex);
        }

        public void OnPrevBtnClick()
        {
            _currentSlideIndex--;
            if (_currentSlideIndex < 0 ) { _currentSlideIndex = 0; }
            SetSlide(_currentSlideIndex);
        }

        public void OnNextBtnClick()
        {
            _currentSlideIndex++;
            if (_currentSlideIndex >= TutorialSprites.Count) { _currentSlideIndex = TutorialSprites.Count - 1; }
            SetSlide(_currentSlideIndex);
        }

        public void OnBackBtnClick()
        {
            MenuManager.SwitchToMenuPanel(MenuWindowType.Login);
        }

        private void SetSlide(int index)
        {
            Slide.GetComponent<Image>().sprite = TutorialSprites[index];
        }
    }
}

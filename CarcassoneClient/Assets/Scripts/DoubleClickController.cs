using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Отслеживает даблклики
    /// </summary>
    internal class DoubleClickController
    {
        private float _timer1 = 0;
        private bool _rememberedButtonClick;
        private Vector3 _rememberedCoursorPosition;

        public bool IsDoubleClick()
        {
            // first click setup timer and remember click
            if (Input.GetMouseButtonUp(0) && _timer1 <= 0)
            {
                _timer1 = 0.3f; // time to make doubleclick
                _rememberedButtonClick = true;
                _rememberedCoursorPosition = Input.mousePosition;
                return false;
            }

            // second click is doubleclick
            if (_rememberedButtonClick && Input.GetMouseButtonUp(0) && _timer1 > 0)
            {
                var isNear = Vector3.Magnitude(Input.mousePosition - _rememberedCoursorPosition) < 30;
                if (isNear)
                {
                    _timer1 = 0;
                    _rememberedButtonClick = false;
                    _rememberedCoursorPosition = Vector3.zero;
                    return true;
                }
                else
                {
                    Logger.Info("Not near second click!");
                }
            }

            // just waiting second click
            if (_timer1 > 0)
            {
                _timer1 -= Time.deltaTime;
            }
            else // timer dropped
            {
                _rememberedButtonClick = false;
                _rememberedCoursorPosition = Vector3.zero;
            }

            return false;
        }
    }
}

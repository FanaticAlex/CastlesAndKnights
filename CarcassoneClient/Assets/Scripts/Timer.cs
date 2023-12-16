using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    /// <summary>
    /// Отсчитывает интервалы времени.
    /// </summary>
    internal class Timer
    {
        private float _timer;
        private float _interval;

        public Timer(float interval)
        {
            _interval = interval;
        }

        public event Func<object, EventArgs, Task> Elapsed;

        public async Task Update(float delta)
        {
            _timer -= delta;
            if (_timer <= 0.0f) // длительные операции, запросы к серверу
            {
                _timer = _interval;
                await OnElapsed();
            }
        }

        public async Task OnElapsed()
        {
            var handler = Elapsed;
            if (handler == null)
                return;

            var invocationList = handler.GetInvocationList();
            Task[] handlerTasks = new Task[invocationList.Length];
            for (int i = 0; i < invocationList.Length; i++)
            {
                handlerTasks[i] = ((Func<object, EventArgs, Task>)invocationList[i])(this, EventArgs.Empty);
            }

            await Task.WhenAll(handlerTasks);
        }
    }
}

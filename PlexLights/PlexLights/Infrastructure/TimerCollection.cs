using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace PlexLights.Infrastructure
{
    public class TimerCollection
    {
        private readonly ConcurrentDictionary<string, TimerAction> _timers;

        public TimerCollection()
        {
            _timers = new ConcurrentDictionary<string, TimerAction>();
        }

        public int Count => _timers.Count;

        public void AddOrUpdate(string key, Action action, TimeSpan duration)
        {
            if (_timers.ContainsKey(key))
            {
                _timers.Remove(key, out TimerAction timer);
                timer.Dispose();
            }

            _timers[key] = GetTimer(key, action, duration);
        }

        public void Clear()
        {
            foreach (var item in _timers)
            {
                item.Value.Dispose();
            }

            _timers.Clear();
        }

        private TimerAction GetTimer(string key, Action action, TimeSpan duration)
        {
            var timer = new TimerAction(action, duration);
            timer.TimerDone += (s, e) =>
            {
                ((TimerAction)s).Dispose();
                _timers.Remove(key, out _);
            };

            return timer;
        }

        private class TimerAction : IDisposable
        {
            public event EventHandler TimerDone;

            private readonly Timer _timer;

            public TimerAction(Func<Task> action, TimeSpan duration)
            {
                _timer = new Timer
                {
                    Interval = duration.TotalMilliseconds,
                    AutoReset = false
                };

                _timer.Elapsed += (s, e) =>
                {
                    action();
                    OnDone(EventArgs.Empty);
                };

                _timer.Start();
            }

            public TimerAction(Action action, TimeSpan duration)
            {
                _timer = new Timer
                {
                    Interval = duration.TotalMilliseconds,
                    AutoReset = false
                };

                _timer.Elapsed += (s, e) =>
                {
                    action();
                    OnDone(EventArgs.Empty);
                };

                _timer.Start();
            }

            public TimerAction(Action action) : this(action, TimeSpan.FromSeconds(10))
            {
            }

            private void OnDone(EventArgs e)
            {
                TimerDone?.Invoke(this, e);
            }

            public void Dispose()
            {
                try
                {
                    _timer.Dispose();
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e.Message);
                }
            }
        }
    }
}
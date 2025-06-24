using MoyuTokei.Common;
using MoyuTokei.Common.Events;
using MoyuTokei.Services.Interfaces;
using Prism.Events;
using System;
using System.Windows.Threading;

namespace MoyuTokei.Services
{
    public sealed class TokeiService : ITokeiService
    {
        private readonly IEventAggregator _eventAggregator;

        private readonly DispatcherTimer _timer;
        private readonly TimeSpan _interval = TimeSpan.FromSeconds(1);

        private TimeSpan _moyuDuration, _jixueDuration;
        private TokeiMode _currentMode;

        public event EventHandler<TimerTickedEventArgs>? TimerTicked;

        public TokeiMode CurrentMode => _currentMode;

        public TokeiService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            _timer = new DispatcherTimer(DispatcherPriority.Send) { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (_, _) => AddSecond();
        }

        private void AddSecond()
        {
            if (_currentMode is TokeiMode.Moyu)
            {
                _moyuDuration += _interval;
            }
            else
            {
                _jixueDuration += _interval;
            }

            RaiseTimerTickedEvent();
        }

        private void RaiseModeChangedEvent()
        {
            _eventAggregator.GetEvent<TokeiModeChangedEvent>().Publish(new(CurrentMode));
        }

        private void RaiseTimerTickedEvent()
        {
            TimerTicked?.Invoke(this, new(_currentMode switch
            {
                TokeiMode.Moyu => _moyuDuration,
                _ => _jixueDuration
            }, _currentMode));
        }

        private void SwitchTo(TokeiMode mode)
        {
            if (_currentMode == mode)
            {
                return;
            }

            _timer.Stop();
            _currentMode = mode;
            _timer.Start();

            RaiseModeChangedEvent();
            RaiseTimerTickedEvent();
        }

        public void Pause()
        {
            _timer.Stop();
        }

        public void Resume()
        {
            _timer.Start();
        }

        public void Reset()
        {
            _moyuDuration = TimeSpan.Zero;
            _jixueDuration = TimeSpan.Zero;

            RaiseTimerTickedEvent();
        }

        public void SwitchToJixue() => SwitchTo(TokeiMode.Jixue);

        public void SwitchToMoyu() => SwitchTo(TokeiMode.Moyu);
    }
}

using MoyuTokei.Common;
using MoyuTokei.Common.Events;
using System;

namespace MoyuTokei.Services.Interfaces
{
    public interface ITokeiService
    {
        void SwitchToMoyu();

        void SwitchToJixue();

        void Pause();

        void Resume();

        void Reset();

        event EventHandler<TimerTickedEventArgs> TimerTicked;

        TokeiMode CurrentMode { get; }
    }
}

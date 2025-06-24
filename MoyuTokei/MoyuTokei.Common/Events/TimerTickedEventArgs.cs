namespace MoyuTokei.Common.Events
{
    public class TimerTickedEventArgs(TimeSpan duration, TokeiMode tokeiMode) : EventArgs
    {
        public TimeSpan Duration { get; init; } = duration;

        public TokeiMode TokeiMode { get; init; } = tokeiMode;
    }
}
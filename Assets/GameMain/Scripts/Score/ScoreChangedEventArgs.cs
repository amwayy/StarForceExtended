using GameFramework;
using GameFramework.Event;

namespace StarForce {
    public class ScoreChangedEventArgs : GameEventArgs {
        public static readonly int EventId = typeof(ScoreChangedEventArgs).GetHashCode();

        public override int Id => EventId;

        public int Score { get; private set; }

        public static ScoreChangedEventArgs Create(int score) {
            ScoreChangedEventArgs eventArgs = ReferencePool.Acquire<ScoreChangedEventArgs>();
            eventArgs.Score = score;
            return eventArgs;
        }

        public override void Clear() {
            Score = 0;
        }
    }
}

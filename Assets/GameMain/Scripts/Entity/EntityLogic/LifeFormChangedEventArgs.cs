using GameFramework;
using GameFramework.Event;

namespace StarForce {
    public class LifeFormChangedEventArgs : GameEventArgs {
        public static readonly int EventId = typeof(LifeFormChangedEventArgs).GetHashCode();

        public override int Id => EventId;

        public int LifeCount { get; private set; }

        public static LifeFormChangedEventArgs Create(int score) {
            LifeFormChangedEventArgs eventArgs = ReferencePool.Acquire<LifeFormChangedEventArgs>();
            eventArgs.LifeCount = score;
            return eventArgs;
        }

        public override void Clear() {
            LifeCount = 0;
        }
    }
}

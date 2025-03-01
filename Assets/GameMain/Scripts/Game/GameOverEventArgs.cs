using GameFramework;
using GameFramework.Event;

namespace StarForce {
    public class GameOverEventArgs : GameEventArgs {
        public static readonly int EventId = typeof(GameOverEventArgs).GetHashCode();
        public override int Id => EventId;

        public static GameOverEventArgs Create() {
            return ReferencePool.Acquire<GameOverEventArgs>();
        }

        public override void Clear() { }
    }
}

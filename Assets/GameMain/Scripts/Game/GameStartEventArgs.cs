using GameFramework;
using GameFramework.Event;

namespace StarForce {
    public class GameStartEventArgs : GameEventArgs {
        public static readonly int EventId = typeof(GameStartEventArgs).GetHashCode();
        public override int Id => EventId;

        public static GameStartEventArgs Create() {
            return ReferencePool.Acquire<GameStartEventArgs>();
        }

        public override void Clear() { }
    }
}

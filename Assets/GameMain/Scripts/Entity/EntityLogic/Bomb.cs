using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce {
    /// <summary>
    /// Õ¨µ¯¡£
    /// </summary>
    public class Bomb : Drop {

        [SerializeField]
        private DropData m_BombData = null;

#if UNITY_2017_3_OR_NEWER
        protected override void OnShow(object userData)
#else
        protected internal override void OnShow(object userData)
#endif
        {
            base.OnShow(userData);

            m_BombData = userData as DropData;
            if (m_BombData == null) {
                Log.Error("Bomb data is invalid.");
                return;
            }
        }

        protected override void ApplyEffect(MyAircraft myAircraft) {
            base.ApplyEffect(myAircraft);

            Asteroid[] asteroids = FindObjectsByType<Asteroid>(FindObjectsSortMode.None);

            foreach (Asteroid asteroid in asteroids) {
                asteroid.ApplyDamage(this, asteroid.GetHP());
            }
        }

    }
}

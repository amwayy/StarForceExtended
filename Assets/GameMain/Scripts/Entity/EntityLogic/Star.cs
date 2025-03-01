using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce {
    /// <summary>
    /// ÐÇÐÇ¡£
    /// </summary>
    public class Star : Drop {

        [SerializeField]
        private DropData m_StarData = null;

#if UNITY_2017_3_OR_NEWER
        protected override void OnShow(object userData)
#else
        protected internal override void OnShow(object userData)
#endif
        {
            base.OnShow(userData);

            m_StarData = userData as DropData;
            if (m_StarData == null) {
                Log.Error("Star data is invalid.");
                return;
            }
        }

        protected override void ApplyEffect(MyAircraft myAircraft) {
            base.ApplyEffect(myAircraft);

            myAircraft.AttachWeapon(30000);
        }

    }
}

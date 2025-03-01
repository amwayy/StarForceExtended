using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce {
    /// <summary>
    /// …¡µÁ°£
    /// </summary>
    public class Thunder : Drop {

        [SerializeField]
        private DropData m_ThunderData = null;

#if UNITY_2017_3_OR_NEWER
        protected override void OnShow(object userData)
#else
        protected internal override void OnShow(object userData)
#endif
        {
            base.OnShow(userData);

            m_ThunderData = userData as DropData;
            if (m_ThunderData == null) {
                Log.Error("Thunder data is invalid.");
                return;
            }
        }

        protected override void ApplyEffect(MyAircraft myAircraft) {
            base.ApplyEffect(myAircraft);

            myAircraft.BoostSpeed(m_ThunderData.EffectValue);
        }

    }
}

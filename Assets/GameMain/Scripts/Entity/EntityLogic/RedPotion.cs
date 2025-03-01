using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce {
    /// <summary>
    /// ÑªÆ¿¡£
    /// </summary>
    public class RedPotion : Drop {

        [SerializeField]
        private DropData m_RedPotionData = null;

#if UNITY_2017_3_OR_NEWER
        protected override void OnShow(object userData)
#else
        protected internal override void OnShow(object userData)
#endif
        {
            base.OnShow(userData);

            m_RedPotionData = userData as DropData;
            if (m_RedPotionData == null) {
                Log.Error("Red Potion data is invalid.");
                return;
            }
        }

        protected override void ApplyEffect(MyAircraft myAircraft) {
            base.ApplyEffect(myAircraft);

            myAircraft.ApplyHeal((int)m_RedPotionData.EffectValue);
        }

    }
}

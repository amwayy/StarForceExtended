using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce {
    /// <summary>
    /// ÐÄ¡£
    /// </summary>
    public class Heart : Drop {

        [SerializeField]
        private DropData m_HeartData = null;

#if UNITY_2017_3_OR_NEWER
        protected override void OnShow(object userData)
#else
        protected internal override void OnShow(object userData)
#endif
        {
            base.OnShow(userData);

            m_HeartData = userData as DropData;
            if (m_HeartData == null) {
                Log.Error("Heart data is invalid.");
                return;
            }
        }

        protected override void ApplyEffect(MyAircraft myAircraft) {
            base.ApplyEffect(myAircraft);

            myAircraft.AddLifeCount();

            GameEntry.Event.Fire(this, LifeFormChangedEventArgs.Create(myAircraft.GetLifeCount()));
        }

    }
}

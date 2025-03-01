using GameFramework.Entity;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce {
    /// <summary>
    /// µôÂäµÀ¾ß¡£
    /// </summary>
    public abstract class Drop : Entity {

        [SerializeField]
        private DropData m_DropData = null;

#if UNITY_2017_3_OR_NEWER
        protected override void OnShow(object userData)
#else
        protected internal override void OnShow(object userData)
#endif
        {
            base.OnShow(userData);

            m_DropData = userData as DropData;
            if (m_DropData == null) {
                Log.Error("Drop data is invalid.");
                return;
            }
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#else
        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#endif
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            CachedTransform.Translate(Vector3.back * m_DropData.Speed * elapseSeconds, Space.World);
        }

        private void OnTriggerEnter(Collider other) {
            Entity entity = other.gameObject.GetComponent<Entity>();
            if (entity == null) {
                return;
            }

            MyAircraft myAircraft = entity as MyAircraft;
            if (myAircraft == null) {
                return;
            }

            ApplyEffect(myAircraft);

            GameEntry.Entity.HideEntity(this);
        }

        protected virtual void ApplyEffect(MyAircraft myAircraft) {

        }

    }
}

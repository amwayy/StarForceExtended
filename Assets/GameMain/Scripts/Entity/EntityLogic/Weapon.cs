//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    /// <summary>
    /// 武器类。
    /// </summary>
    public class Weapon : Entity
    {
        private const string AttachPoint = "Weapon Point";

        [SerializeField]
        private WeaponData m_WeaponData = null;

        private float m_NextAttackTime = 0f;
        private Aircraft m_OwnerAircraft = null;
        private float m_SpreadFireAngleMax = 120f;
        private float m_SpreadFireAngleInterval = 15f;
        private float m_LineFireAngleInterval = .5f;


#if UNITY_2017_3_OR_NEWER
        protected override void OnInit(object userData)
#else
        protected internal override void OnInit(object userData)
#endif
        {
            base.OnInit(userData);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnShow(object userData)
#else
        protected internal override void OnShow(object userData)
#endif
        {
            base.OnShow(userData);

            m_WeaponData = userData as WeaponData;
            if (m_WeaponData == null)
            {
                Log.Error("Weapon data is invalid.");
                return;
            }

            GameEntry.Entity.AttachEntity(Entity, m_WeaponData.OwnerId, AttachPoint);

            m_OwnerAircraft = GetOwnerAircraft();

            if (m_OwnerAircraft == null) {
                Log.Error("Owner Aircraft is invalid.");
                return;
            }

            m_NextAttackTime = m_OwnerAircraft.GetWeaponNextAttackTime();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
#else
        protected internal override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
#endif
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);

            Name = Utility.Text.Format("Weapon of {0}", parentEntity.Name);
            CachedTransform.localPosition = Vector3.zero;
        }

        public void TryAttack()
        {
            if (Time.time < m_NextAttackTime)
            {
                return;
            }

            int weaponIndex = m_OwnerAircraft.GetWeaponIndex(this);
            int weaponCount = m_OwnerAircraft.GetWeaponCount();

            Vector3 bulletSpawnPosition = CalculateBulletSpawnPosition(weaponIndex, weaponCount);
            Quaternion bulletRotation = CalculateBulletRotation(weaponIndex, weaponCount);

            m_NextAttackTime = Time.time + m_WeaponData.AttackInterval;
            GameEntry.Entity.ShowBullet(new BulletData(GameEntry.Entity.GenerateSerialId(), m_WeaponData.BulletId, m_WeaponData.OwnerId, m_WeaponData.OwnerCamp, m_WeaponData.Attack, m_WeaponData.BulletSpeed)
            {
                Position = bulletSpawnPosition,
                Rotation = bulletRotation,
            });
            GameEntry.Sound.PlaySound(m_WeaponData.BulletSoundId);
        }

        private Quaternion CalculateBulletRotation(int weaponIndex, int weaponCount) {
            if (weaponCount < 3) {
                return Quaternion.identity;
            }

            // 散射最大角设为120°
            float interval = Mathf.Min(m_SpreadFireAngleInterval, m_SpreadFireAngleMax / (weaponCount - 1));
            Vector3 rotationVector = new Vector3(0, (weaponIndex - (weaponCount - 1) / 2) * interval, 0);
            return Quaternion.Euler(rotationVector);
        }

        private Vector3 CalculateBulletSpawnPosition(int weaponIndex, int weaponCount) {
            if (weaponCount < 3) {
                return CachedTransform.position + new Vector3((weaponIndex - (weaponCount - 1) / 2) * m_LineFireAngleInterval, 0, 0);
            }

            return CachedTransform.position;
        }

        public float GetNextAttackTime() {
            return m_NextAttackTime;
        }

        private Aircraft GetOwnerAircraft() {
            return GameEntry.Entity.GetEntity(m_WeaponData.OwnerId)?.Logic as Aircraft;
        }

    }
}

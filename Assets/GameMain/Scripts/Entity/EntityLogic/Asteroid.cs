//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.DataTable;
using UnityEngine;
using UnityEngine.UIElements;
using UnityGameFramework.Runtime;
using System;
using UnityEngine.EventSystems;

namespace StarForce
{
    /// <summary>
    /// 小行星类。
    /// </summary>
    public class Asteroid : TargetableObject
    {
        [SerializeField]
        private AsteroidData m_AsteroidData = null;

        private Vector3 m_RotateSphere = Vector3.zero;
        private ProcedureMain m_ProcedureMain = null;
        private GameBase m_CurrentGame = null;

#if UNITY_2017_3_OR_NEWER
        protected override void OnInit(object userData)
#else
        protected internal override void OnInit(object userData)
#endif
        {
            base.OnInit(userData);

            m_ProcedureMain = GameEntry.Procedure.GetProcedure<ProcedureMain>() as ProcedureMain;
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnShow(object userData)
#else
        protected internal override void OnShow(object userData)
#endif
        {
            base.OnShow(userData);

            m_AsteroidData = userData as AsteroidData;
            if (m_AsteroidData == null)
            {
                Log.Error("Asteroid data is invalid.");
                return;
            }

            m_RotateSphere = UnityEngine.Random.insideUnitSphere;

            m_CurrentGame = m_ProcedureMain.GetCurrentGame();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#else
        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#endif
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            Vector3 moveDirection = Vector3.back;
            if (!m_CurrentGame.GameOver) {
                Vector3 playerShipPosition = m_CurrentGame.GetMyAircraft().CachedTransform.position;
                moveDirection = (playerShipPosition - CachedTransform.position).normalized;
            }

            CachedTransform.Translate(moveDirection * m_AsteroidData.Speed * elapseSeconds, Space.World);
            CachedTransform.Rotate(m_RotateSphere * m_AsteroidData.AngularSpeed * elapseSeconds, Space.Self);
        }

        protected override void OnDead(Entity attacker)
        {
            base.OnDead(attacker);

            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), m_AsteroidData.DeadEffectId)
            {
                Position = CachedTransform.localPosition,
            });
            GameEntry.Sound.PlaySound(m_AsteroidData.DeadSoundId);

            // 击杀后加分
            GameEntry.Score.ModifyScore(1);

            // 掉落道具
            DropUtility.RandomDrop(CachedTransform.position);
        }


        public override ImpactData GetImpactData()
        {
            return new ImpactData(m_AsteroidData.Camp, m_AsteroidData.HP, m_AsteroidData.Attack, 0);
        }
    }
}

using GameFramework.DataTable;
using System;
using UnityEngine;

namespace StarForce {
    [Serializable]
    public class DropData : EntityData {

        [SerializeField]
        private Type m_DropType;

        [SerializeField]
        private float m_Speed = 0f;

        [SerializeField]
        private float m_EffectValue = 0;

        [SerializeField]
        private int m_DeadEffectId = 0;

        [SerializeField]
        private int m_DeadSoundId = 0;

        public DropData(int entityId, int typeId)
            : base(entityId, typeId) {
            IDataTable<DRDrop> dtDrop = GameEntry.DataTable.GetDataTable<DRDrop>();
            DRDrop drDrop = dtDrop.GetDataRow(TypeId);

            m_DropType = Type.GetType("StarForce." + drDrop.DropType);
            m_Speed = drDrop.Speed;
            m_EffectValue = drDrop.EffectValue;
            m_DeadEffectId = drDrop.DeadEffectId;
            m_DeadSoundId = drDrop.DeadSoundId;
        }

        public Type DropType {
            get { return m_DropType; }
        }

        public float Speed {
            get { return m_Speed; }
        }

        public float EffectValue {
            get { return m_EffectValue; }
        }

        public int DeadEffectId {
            get { return m_DeadEffectId; }
        }

        public int DeadSoundId {
            get { return m_DeadSoundId; }
        }
    }
}

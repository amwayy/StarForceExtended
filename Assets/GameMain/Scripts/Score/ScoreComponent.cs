using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce {
    public class ScoreComponent : GameFrameworkComponent {
        private int m_Score = 0;
        private int m_Record = 0;

        private const string RECORD = "Record";

        public int Score {
            get {
                return m_Score;
            }
        }

        public int Record {
            get {
                return m_Record;
            }
        }

        private void Start() {
            m_Record = GameEntry.Setting.GetInt(RECORD, 0);

            GameEntry.Event.Subscribe(GameStartEventArgs.EventId, OnGameStart);
        }

        //private void OnDestroy() {
        //    GameEntry.Event.Unsubscribe(GameStartEventArgs.EventId, OnGameStart);
        //}

        private void OnGameStart(object sender, GameEventArgs e) {
            ModifyScore(-m_Score); 
        }

        public void ModifyScore(int modifyAmount) {
            m_Score += modifyAmount;

            if (m_Score > m_Record) { 
                m_Record = m_Score;

                GameEntry.Setting.SetInt(RECORD, m_Record);
                GameEntry.Setting.Save();
            }

            GameEntry.Event.Fire(this, ScoreChangedEventArgs.Create(m_Score));
        }
    }
}

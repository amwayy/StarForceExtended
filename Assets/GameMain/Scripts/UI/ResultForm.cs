using GameFramework.Event;
using StarForce;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;


namespace StarForce {
    public class ResultForm : UGuiForm {

        [SerializeField] private Text m_ScoreText;
        [SerializeField] private Text m_RecordText;

        private ProcedureMain m_ProcedureMain = null;

        protected override void OnOpen(object userData) {
            base.OnOpen(userData);

            m_ScoreText.text = GameEntry.Score.Score.ToString();
            m_RecordText.text = GameEntry.Score.Record.ToString();

            m_ProcedureMain = (ProcedureMain)userData;
            if (m_ProcedureMain == null) {
                Log.Warning("ProcedureMain is invalid when open ResultForm.");
                return;
            }
        }

        protected override void OnClose(bool isShutdown, object userData) {
            m_ProcedureMain = null;

            base.OnClose(isShutdown, userData);
        }

        public void OnRetryButtonClick() {
            m_ProcedureMain.Retry();

            Close();
        }

        public void OnMenuButtonClick() {
            m_ProcedureMain.GotoMenu();
        }
    }
}
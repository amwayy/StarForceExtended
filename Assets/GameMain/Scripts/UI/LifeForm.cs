using GameFramework.Event;
using UnityEngine;
using UnityEngine.UI;


namespace StarForce {
    public class LifeForm : UGuiForm {

        [SerializeField] private Text m_lifeCountText;

        protected override void OnOpen(object userData) {
            base.OnOpen(userData);

            GameEntry.Event.Subscribe(LifeFormChangedEventArgs.EventId, OnLifeCountChanged);
            GameEntry.Event.Subscribe(GameOverEventArgs.EventId, OnGameOver);
        }

        protected override void OnClose(bool isShutdown, object userData) {
            GameEntry.Event.Unsubscribe(LifeFormChangedEventArgs.EventId, OnLifeCountChanged);
            GameEntry.Event.Unsubscribe(GameOverEventArgs.EventId, OnGameOver);

            base.OnClose(isShutdown, userData);
        }

        private void OnLifeCountChanged(object sender, GameEventArgs e) {
            LifeFormChangedEventArgs args = (LifeFormChangedEventArgs)e;
            UpdateLifeCount(args.LifeCount);
        }

        private void OnGameOver(object sender, GameEventArgs e) {
            Close();
        }

        private void UpdateLifeCount(int lifeCount) {
            m_lifeCountText.text = $"{lifeCount}";
        }
    }
}

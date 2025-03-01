using GameFramework.Event;
using UnityEngine;
using UnityEngine.UI;


namespace StarForce {
    public class ScoreForm : UGuiForm {
        [SerializeField] private Text m_scoreText;

        protected override void OnOpen(object userData) {
            base.OnOpen(userData);

            GameEntry.Event.Subscribe(ScoreChangedEventArgs.EventId, OnScoreChanged);
            GameEntry.Event.Subscribe(GameOverEventArgs.EventId, OnGameOver);

            UpdateScore(GameEntry.Score.Score);
        }

        protected override void OnClose(bool isShutdown, object userData) {
            GameEntry.Event.Unsubscribe(ScoreChangedEventArgs.EventId, OnScoreChanged);
            GameEntry.Event.Unsubscribe(GameOverEventArgs.EventId, OnGameOver);

            base.OnClose(isShutdown, userData);
        }

        private void OnScoreChanged(object sender, GameEventArgs e) {
            ScoreChangedEventArgs args = (ScoreChangedEventArgs)e;
            UpdateScore(args.Score);
        }

        private void OnGameOver(object sender, GameEventArgs e) {
            Close();
        }

        private void UpdateScore(int score) {
            m_scoreText.text = $"{score}";
        }
    }
}

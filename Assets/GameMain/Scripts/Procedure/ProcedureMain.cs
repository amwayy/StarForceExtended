﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace StarForce
{
    public class ProcedureMain : ProcedureBase
    {
        private const float GameOverDelayedSeconds = 0f;

        private readonly Dictionary<GameMode, GameBase> m_Games = new Dictionary<GameMode, GameBase>();
        private GameBase m_CurrentGame = null;
        private bool m_GotoMenu = false;
        private float m_GotoMenuDelaySeconds = 0f;

        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        public void GotoMenu() {
            m_GotoMenuDelaySeconds = 0;
            m_GotoMenu = true;
        }

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);

            m_Games.Add(GameMode.Survival, new SurvivalGame());
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);

            m_Games.Clear();
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            m_GotoMenu = false;
            GameMode gameMode = (GameMode)procedureOwner.GetData<VarByte>("GameMode").Value;
            m_CurrentGame = m_Games[gameMode];
            m_CurrentGame.Initialize();

            GameEntry.UI.OpenUIForm(UIFormId.ScoreForm, this);
            GameEntry.UI.OpenUIForm(UIFormId.LifeForm, this);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            if (m_CurrentGame != null)
            {
                m_CurrentGame.Shutdown();
                m_CurrentGame = null;
            }

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_CurrentGame != null && !m_CurrentGame.GameOver)
            {
                m_CurrentGame.Update(elapseSeconds, realElapseSeconds);
                return;
            }

            if (m_CurrentGame != null && m_CurrentGame.GameOver && !GameEntry.UI.HasUIForm(UIFormId.ResultForm)) {
                GameEntry.UI.OpenUIForm(UIFormId.ResultForm, this);
            }

            //if (!m_GotoMenu) {
            //    m_GotoMenu = true;
            //    m_GotoMenuDelaySeconds = 0;
            //}

            if (m_GotoMenu) {
                if (GameEntry.UI.HasUIForm(UIFormId.ResultForm)) {
                    UGuiForm resultForm = GameEntry.UI.GetUIForm(UIFormId.ResultForm);
                    GameEntry.UI.CloseUIForm(resultForm);
                }

                m_GotoMenuDelaySeconds += elapseSeconds;
                if (m_GotoMenuDelaySeconds >= GameOverDelayedSeconds) {
                    procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.Menu"));
                    ChangeState<ProcedureChangeScene>(procedureOwner);
                }
            }
        }

        public void Retry() {
            if (m_CurrentGame != null) {
                m_CurrentGame.Shutdown();
                m_CurrentGame = null;
            }
            m_Games.Remove(GameMode.Survival);

            GameEntry.Entity.HideAllLoadedEntities();
            GameEntry.Entity.HideAllLoadingEntities();

            m_Games.Add(GameMode.Survival, new SurvivalGame());

            m_CurrentGame = m_Games[GameMode.Survival];
            m_CurrentGame.Initialize();
        }

        public GameBase GetCurrentGame() {
            return m_CurrentGame;
        }
    }
}

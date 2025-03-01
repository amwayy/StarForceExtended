//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.DataTable;
using UnityEngine;

namespace StarForce
{
    public class SurvivalGame : GameBase
    {
        private float m_SpawnInterval = 1f;
        private float m_MinSpawnInterval = .1f;
        private float m_AsteroidUpgradeInterval = 5f;
        private float m_NextSpawnTime = 0f;
        private float m_NextAsteroidUpgradeTime = 0f;
        private float m_SpawnIntervalDecreaseAmount = .05f;
        private int m_AsteroidHPIncreaseAmount = 5;
        private int m_AsteroidAdditionalHP = 0;

        public override GameMode GameMode
        {
            get
            {
                return GameMode.Survival;
            }
        }

        public override void Initialize() {
            base.Initialize();

            // 重开时重置生成间隙
            m_NextSpawnTime = 0f;
            m_NextAsteroidUpgradeTime = Time.time + m_AsteroidUpgradeInterval;
            m_SpawnInterval = 1f;
            m_AsteroidAdditionalHP = 0;
        }

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            base.Update(elapseSeconds, realElapseSeconds);

            if (Time.time >= m_NextAsteroidUpgradeTime) {
                m_NextAsteroidUpgradeTime = Time.time + m_AsteroidUpgradeInterval;

                m_SpawnInterval = Mathf.Max(m_MinSpawnInterval, m_SpawnInterval - m_SpawnIntervalDecreaseAmount);

                m_AsteroidAdditionalHP += m_AsteroidHPIncreaseAmount;
            }


            if (Time.time >= m_NextSpawnTime)
            {
                m_NextSpawnTime = Time.time + m_SpawnInterval;

                IDataTable<DRAsteroid> dtAsteroid = GameEntry.DataTable.GetDataTable<DRAsteroid>();
                float randomPositionX = SceneBackground.EnemySpawnBoundary.bounds.min.x + SceneBackground.EnemySpawnBoundary.bounds.size.x * (float)Utility.Random.GetRandomDouble();
                float randomPositionZ = SceneBackground.EnemySpawnBoundary.bounds.min.z + SceneBackground.EnemySpawnBoundary.bounds.size.z * (float)Utility.Random.GetRandomDouble();
                GameEntry.Entity.ShowAsteroid(new AsteroidData(GameEntry.Entity.GenerateSerialId(), 60000 + Utility.Random.GetRandom(dtAsteroid.Count), m_AsteroidAdditionalHP) {
                    Position = new Vector3(randomPositionX, 0f, randomPositionZ),
                });
            }
        }
    }
}

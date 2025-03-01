using System;
using System.Collections.Generic;
using System.Linq;
using GameFramework.DataTable;
using UnityEngine;

namespace StarForce {
    /// <summary>
    /// 道具掉落工具类。
    /// </summary>
    public static class DropUtility {

        private static Dictionary<int, float> s_probabilityWeightTable = new Dictionary<int, float>();

        private static int[] s_DropItemIds = { 80001 };
        //private static float s_DropProbability = .5f;
        private static float s_DropProbability = .1f;

        public static void InitializeDropTable() {
            s_probabilityWeightTable.Clear();
            IDataTable<DRDrop> dtDrop = GameEntry.DataTable.GetDataTable<DRDrop>();

            foreach (DRDrop drop in dtDrop.GetAllDataRows()) {
                s_probabilityWeightTable[drop.Id] = drop.ProbabilityWeight;
            }

            //foreach (int id in s_DropItemIds) {
            //    s_probabilityWeightTable[id] = dtDrop[id].ProbabilityWeight;
            //}
        }


        public static void RandomDrop(Vector3 dropPosition) {
            if (UnityEngine.Random.value >= s_DropProbability)
                return;

            int dropEntityId = GameEntry.Entity.GenerateSerialId();

            int selectedItemId = GetWeightedRandomDrop();

            DropData dropData = new DropData(dropEntityId, selectedItemId) {
                Position = dropPosition,
            };

            Type dropType = dropData.DropType;
            if (dropType == null) {
                Debug.LogError($"未知掉落物类型 ID: {selectedItemId}");
                return;
            }

            GameEntry.Entity.ShowDrop(dropType, dropData);
        }

        private static int GetWeightedRandomDrop() {
            float totalWeight = s_probabilityWeightTable.Values.Sum();
            float randomPoint = UnityEngine.Random.value * totalWeight;

            float currentWeight = 0f;
            foreach (var item in s_probabilityWeightTable) {
                currentWeight += item.Value;
                if (randomPoint <= currentWeight) {
                    return item.Key;
                }
            }

            return s_probabilityWeightTable.Keys.First();
        }

    }
}

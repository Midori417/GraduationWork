// PilotFactory

using UnityEngine;

namespace Factory
{
    /// <summary>
    /// パイロットを生成する工場
    /// </summary>
    public class PilotFactory
    {
        private static readonly string[] names =
{
            "HumanPilot",
            "StopCpuPilot",
            "EasyCpuPilot"
        };

        /// <summary>
        /// パイロットを生成
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static BasePilot Create(PilotType type)
        {
            if (type == PilotType.Max)
            {
                // 範囲外
                return null;
            }

            string name = names[(int)type];
            // リソースからプレハブを取得
            BasePilot prefab = Resources.Load<BasePilot>($"Prefabs/Pilot/pfb_{name}");

            if (prefab == null)
            {
                Debug.LogError(name + "は存在しません");
                return null;
            }

            // 生成
            return GameObject.Instantiate(prefab);
        }
    }
}
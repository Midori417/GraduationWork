// MsFactory.cs

using UnityEngine;

namespace Factory
{

    /// <summary>
    /// 機体を生成する工場
    /// </summary>
    public class MsFactory
    {
        private static readonly string[] names =
        {
            "Gundam"
        };

        /// <summary>
        /// 機体を生成
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static BaseMs Create(MsType type)
        {
            if(type == MsType.Max)
            {
                // 範囲外
                return null;
            }
            string name = names[(int)type];
            // リソースからプレハブを取得
            BaseMs prefab = Resources.Load<BaseMs>($"Prefabs/Ms/pfb_{name}");

            if (prefab== null)
            {
                Debug.LogError(name + "は存在しません");
                return null;
            }

            // 生成
            return GameObject.Instantiate(prefab);
        }
    }
}
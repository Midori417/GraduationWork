// MapFactory.cs

using UnityEngine;

namespace Factory
{
    /// <summary>
    /// マップを生成する工場
    /// </summary>
    public class MapFactory
    {
        private static readonly string[] names =
        {
            "Side7"
        };

        /// <summary>
        /// マップを生成
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static MapManager Create(MapType type)
        {
            if (type == MapType.Max)
            {
                // 範囲外
                return null;
            }

            string name = names[(int)type];
            // リソースからプレハブを取得
            MapManager prefab = Resources.Load<MapManager>($"Prefabs/Map/pfb_{name}");

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

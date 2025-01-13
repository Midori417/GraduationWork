using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField, Header("リスポーン位置")]
    private Transform[] resPoes;

    /// <summary>
    /// リス地取得
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Vector2 GetResPos(int index)
    {
        if(index > resPoes.Length)
        {
            return Vector2.zero;
        }
        return new Vector2(resPoes[index].position.x, resPoes[index].position.z);
    }
}

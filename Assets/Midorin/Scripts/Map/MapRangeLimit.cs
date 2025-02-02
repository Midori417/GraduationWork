﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マップ範囲制限をオブジェクトを生成する
/// </summary>
public class MapRangeLimit : MonoBehaviour
{
    [SerializeField, Header("マップサイズ")]
    private Vector2 _mapSize = Vector2.one;

    [SerializeField, Header("壁の設計図")]
    private GameObject _pfbWall = null;

    /// <summary>
    /// Updateの前に呼び出される
    /// </summary>
    void Start()
    {
        Vector2 mapSizeHarf = _mapSize / 2;
        {
            Vector2 pos = new Vector2(0, mapSizeHarf.y);
            Vector2 size = new Vector2(_mapSize.x, 1);
            CreateMapWall(pos, size);
        }
        {
            Vector2 pos = new Vector2(0, -mapSizeHarf.y);
            Vector2 size = new Vector2(_mapSize.x, 1);
            CreateMapWall(pos, size);
        }
        {
            Vector2 pos = new Vector2(mapSizeHarf.x, 0);
            Vector2 size = new Vector2(1, _mapSize.y);
            CreateMapWall(pos, size);
        }
        {
            Vector2 pos = new Vector2(-mapSizeHarf.x, 0);
            Vector2 size = new Vector2(1, _mapSize.y);
            CreateMapWall(pos, size);
        }
    }

    /// <summary>
    /// 壁を作成
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="size"></param>
    void CreateMapWall(Vector2 pos, Vector2 size)
    {
        if (!_pfbWall)
        {
            Debug.LogError("壁の設計図が登録されていない");
            return;
        }
        GameObject obj = Instantiate(_pfbWall);
        obj.transform.position = new Vector3(pos.x, 0.5f, pos.y);
        obj.transform.localScale = new Vector3(size.x, 1, size.y);

        obj.transform.SetParent(transform);
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻撃判定の基底クラス
/// </summary>
public class BaseAttackCollision : MonoBehaviour
{
    [SerializeField, Header("与えるダメージ")]
    private float _atk = 0;

    [SerializeField, Header("ダウン値")]
    private float _down = 0;

    // 所属チーム
    private Team _team = Team.None;

    #region プロパティ

    public float atk
    {
        get => _atk;
        set => _atk = value;
    }
    public float down
    {
        get => _down;
        set => _down = value;
    }

    public Team team
    {
        protected get => _team;
        set => _team = value;
    }

    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム全体管理クラス
/// </summary>
public class GameManager : SingletonBehavior<GameManager>
{
    // バトル情報
    private BattleInfo _battleInfo;

    // バトル情報
    public BattleInfo battleInfo
    {
        get => _battleInfo;
        set => _battleInfo = value;
    }
}

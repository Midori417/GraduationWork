using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム全体管理クラス
/// </summary>
public class GameManager : MonoBehaviour
{
    // シングルトンクラスにする
    public static GameManager instance
    {
        get;
        private set;
    }

    // バトル情報
    public BattleInfo battleInfo
    {
        get;
        private set;
    }

    // チームのコストの最大値
    public static readonly int teamCostMax = 6000;

    /// <summary>
    /// 生成時のイベント
    /// </summary>
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    /// <summary>
    /// バトル情報を設定
    /// </summary>
    /// <param name="_battleInfo"></param>
    public void SetBattleInfo(BattleInfo _battleInfo)
    {
        battleInfo = _battleInfo;
    }
}

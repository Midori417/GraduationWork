using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム全体管理クラス
/// </summary>
public class GameManager : SingletonBehavior<GameManager>
{
    // バトル情報
    public BattleInfo _battleInfo
    {
        get;
        private set;
    }

    // チームのコストの最大値
    public static readonly int teamCostMax = 6000;

    [SerializeField, Header("機体のプレハブ")]
    private List<BaseMs> _pfbMs;

    [SerializeField, Header("人間パイロットプレハブ")]
    private HumanPilot _pfbHumanPilot;

    [SerializeField, Header("コンピュータパイロットプレハブ")]
    private CpuPilot _pfbCpuPilot;

    /// <summary>
    /// バトル情報を設定
    /// </summary>
    /// <param name="_battleInfo"></param>
    public void SetBattleInfo(BattleInfo _battleInfo)
    {
        this._battleInfo = _battleInfo;
    }

    /// <summary>
    /// 機体設計図を取得
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public BaseMs GetMsPrefab(int index)
    {
        if (_pfbMs.Count - 1 < index)
        {
            return null; 
        }
        return _pfbMs[index];
    }

    /// <summary>
    /// パイロット設計図を取得
    /// </summary>
    /// <param name="index"></param>
    /// <returns>
    /// true 人間
    /// false CPU
    /// </returns>
    public BasePilot GetPilotPrefab(bool inputType)
    {
        if(inputType)
        {
            return _pfbHumanPilot;
        }
        else
        {
            return _pfbCpuPilot;
        }
    }
}

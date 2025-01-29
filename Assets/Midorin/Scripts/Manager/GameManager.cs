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

    [SerializeField, Header("機体のプレハブ")]
    private List<BaseMs> _pfb_ms;

    [SerializeField, Header("人間パイロットプレハブ")]
    private HumanPilot pfb_humanPilot;

    [SerializeField, Header("コンピュータパイロットプレハブ")]
    private CpuPilot pfb_cpuPilot;

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

    /// <summary>
    /// 機体設計図を取得
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public BaseMs GetMsPrefab(int index)
    {
        if (_pfb_ms.Count - 1 < index)
        {
            return null; 
        }
        return _pfb_ms[index];
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
            return pfb_humanPilot;
        }
        else
        {
            return pfb_cpuPilot;
        }
    }
}

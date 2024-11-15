using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// バトル管理クラス
/// </summary>
public class BttleManager : MonoBehaviour
{
    // 赤チームのコスト
    int teamRedCost = new int();

    // 青チームのコスト
    int teamBlueCost = new int();

    [SerializeField, Header("戦うパイロット")]
    private List<BasePilot> pilots;

    /// <summary>
    /// スタートイベント
    /// </summary>
    private void Start()
    {
        // バトル情報を取得
        BattleInfo battleInfo = GameManager.instance.battleInfo;
        teamRedCost = battleInfo.teamRedCost;
        teamBlueCost = battleInfo.teamBlueCost;
    }
}

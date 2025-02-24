using System;
using System.Collections.Generic;

/// <summary>
/// チーム
/// </summary>
public enum Team
{
    None=0,
    Red=1,
    Blue=2,
}

/// <summary>
/// パイロット情報
/// </summary>
[Serializable]
public struct PilotInfo
{
    // チームID
    public Team teamId;

    // プレイヤータイプ
    public PilotType playerType;

    // 仕様機体
    public MsType useMs;
}

/// <summary>
/// バトル情報
/// </summary>
[Serializable]
public struct BattleInfo
{
    // チームのコストの最大値
    public static readonly int teamCostMax = 6000;

    // 赤チームのコスト
    public int teamRedCost;

    // 青チームのコスト
    public int teamBlueCost;

    // 制限時間
    public int time;

    public MapType mapType;

    // パイロット情報配列
    public List<PilotInfo> pilotsInfo;
}

// 勝敗
public enum Victory
{
    None,
    Win,
    Lose,
    Draw
}

// ターゲットタイプ
public enum TargetType
{
    Red,
    Green,
    Yellow,
    LookOn
}
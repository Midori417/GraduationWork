using System;
using System.Collections.Generic;
using UnityEngine;

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
/// プレイヤーのタイプ
/// </summary>
public enum PlayerType
{
    Human,
    Cpu,
}

/// <summary>
/// 機体リスト
/// </summary>
public enum MsList
{
    Gundam,
    None,
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
    public PlayerType playerType;

    // 仕様機体
    public MsList useMs;
}

/// <summary>
/// バトル情報
/// </summary>
[Serializable]
public struct BattleInfo
{
    // 赤チームのコスト
    public int teamRedCost;

    // 青チームのコスト
    public int teamBlueCost;

    // 制限時間
    public int time;

    // パイロット情報配列
    public List<PilotInfo> pilotsInfo;
}

// 勝敗
public enum Victory
{
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
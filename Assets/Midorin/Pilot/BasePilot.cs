using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// パイロットの基底クラス
/// </summary>
public class BasePilot : MonoBehaviour
{
    [SerializeField, Header("自身の機体")]
    private BaseMS myMs;

    [SerializeField, Header("自身のカメラ")]
    private GameObject myCamera;

    [SerializeField, Header("自身のUI")]
    private UIManager myUImanager;

    [SerializeField, Header("相手チームのパイロット")]
    private BasePilot[] enemyPilots;

    [SerializeField, Header("ターゲットパイロット")]
    private BasePilot targetPilot;

    // 自分のチームのコスト
    private int myTeamCost;

    // 相手チームのコスト
    private int enemyTeamCost;

    [System.Serializable]
    public struct MsInput
    {
        [Header("移動入力")]
        public Vector2 moveAxis;

        [Header("ジャンプ入力")]
        public bool jumpBtn;

        [Header("ダッシュ入力")]
        public bool dashBtn;
    }

    /// <summary>
    /// チームのコストを設定
    /// 生成されたときにバトルマネージャで呼び出す
    /// </summary>
    /// <param name="_myTeamCost">自チーム</param>
    /// <param name="_enemyTeamCost">相手チーム</param>
    public void SetTeamCost(int _myTeamCost, int _enemyTeamCost)
    {
        myTeamCost = _myTeamCost;
        enemyTeamCost = _enemyTeamCost;
    }
}

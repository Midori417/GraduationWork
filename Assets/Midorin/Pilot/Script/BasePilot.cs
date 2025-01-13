using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// パイロットの基底クラス
/// </summary>
public class BasePilot : MonoBehaviour
{
    [SerializeField, Header("自身のカメラ")]
    private CameraManager _myCameraManager;

    [SerializeField, Header("自身の機体")]
    private BaseMs _myMs;

    // 敵チームのパイロット
    private List<BasePilot> enemyTeamPilots = new List<BasePilot>();

    [SerializeField, Header("ターゲットパイロット")]
    private BasePilot targetPilot;

    public BaseMs myMs
    { get { return _myMs; } }

    public BaseMs targetMs
    { get { return targetPilot.myMs; } }

    /// <summary>
    /// 機体を設定
    /// BattleManagerで呼び出す
    /// </summary>
    /// <param name="myMs"></param>
    public void SetMyMs(BaseMs myMs)
    {
        _myMs = myMs;
    }

    /// <summary>
    /// エネミーチームに追加
    /// </summary>
    /// <param name="enemyPilot"></param>
    public void SetEnemyPilot(BasePilot enemyPilot)
    {
        enemyTeamPilots.Add(enemyPilot);
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    public virtual void Initialize()
    {
        _myMs.SetMyCamera(_myCameraManager.mainCamera.transform);

        targetPilot = enemyTeamPilots[0];
        _myMs.SetTargetMs(targetMs);

        _myMs.Initialize();
    }
}

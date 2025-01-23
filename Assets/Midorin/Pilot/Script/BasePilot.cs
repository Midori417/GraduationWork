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

    // 自身の機体
    public BaseMs myMs
    { get; private set; }

    [SerializeField, Header("ターゲットパイロット")]
    private BasePilot targetPilot;
    // ターゲット機体
    public BaseMs targetMs
    { get { return targetPilot.myMs; } }

    // 味方チームのパイロット
    private BasePilot teamPilots;

    // 敵チームのパイロット
    private List<BasePilot> enemyTeamPilots = new List<BasePilot>();

    [SerializeField, Header("処理をするか")]
    private bool _isProsess;
    protected bool isProsess
    { get { return _isProsess; } }

    // 復活位置
    private List<Transform> removePos = new List<Transform>();

    // trueなら機体のリムーブ可能
    private bool isRemoveMs = false;

    //[SerializeField, Header("復活時間")]
    private float removeTime = 5;

    #region 処理コントロール関数

    /// <summary>
    /// 処理を開始
    /// </summary>
    virtual public void StartProsess()
    {
        _isProsess = true;
    }

    /// <summary>
    /// 処理を停止
    /// </summary>
    virtual public void StopProsess()
    {
        _isProsess = false;
    }

    #endregion

    /// <summary>
    /// 初期化処理
    /// </summary>
    public virtual void Initialize()
    {
        myMs.SetMyCamera(_myCameraManager.mainCamera.transform);

        targetPilot = enemyTeamPilots[0];
        myMs.SetTargetMs(targetMs);

        myMs.Initialize();

        // カメラの初期設定
        List<Transform> enemyMses = new List<Transform>();
        foreach (BasePilot pilot in enemyTeamPilots)
        {
            enemyMses.Add(pilot.myMs.center);
        }

        _myCameraManager.Initialize(myMs.center, enemyMses);
    }

    /// <summary>
    /// 破壊された機体の処理
    /// </summary>
    protected void DestoryMsProsess()
    {
        if (myMs.hp <= 0)
        {
            if (!isRemoveMs)
            {
                isRemoveMs = true;
                Invoke("RemoveMs", removeTime);
            }
        }
    }

    /// <summary>
    /// 機体を復元
    /// </summary>
    private void RemoveMs()
    {
        // 復活位置
        if (removePos.Count > 0)
        {
            int index = Random.Range(0, removePos.Count);
            myMs.transform.SetPositionAndRotation(removePos[index].position, removePos[index].rotation);
        }

        myMs.Remove();
        isRemoveMs = false;
        myMs.gameObject.SetActive(true);
    }

    /// <summary>
    /// 機体を設定
    /// BattleManagerで呼び出す
    /// </summary>
    /// <param name="myMs"></param>
    public void SetMyMs(BaseMs _myMs)
    {
        myMs = _myMs;
    }

    /// <summary>
    /// 自チームのパイロットを設定
    /// </summary>
    /// <param name="_teamPilot"></param>
    public void SetTeamPilot(BasePilot _teamPilot)
    {
        teamPilots = _teamPilot;
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
    /// 復活位置を設定
    /// </summary>
    /// <param name="transforms"></param>
    public void SetRemovePos(List<Transform> transforms)
    {
        removePos = transforms;
    }
}

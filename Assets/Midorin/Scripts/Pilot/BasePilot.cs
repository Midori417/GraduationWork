using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// パイロットの基底クラス
/// </summary>
public class BasePilot : BaseGameObject
{
    private Team _team;

    [SerializeField, Header("カメラマネージャー")]
    private CameraManager _cameraManager;

    // 自身の操る機体
    private BaseMs _myMs;

    // ターゲットパイロット
    private BasePilot _targetPilot;
    private List<BasePilot> _enemyPilots;
    private int _taregetIndex = 0;

    public Team team
    {
        get => _team; 
        set => _team = value;
    }
    public BaseMs myMs
    {
        get { return _myMs; }
        set { _myMs = value; }
    }
    public List<BasePilot> enemyPilots
    {
        set => _enemyPilots = value;
    }
    protected CameraManager cameraManager => _cameraManager;

    /// <summary>
    /// 初期化
    /// </summary>
    public virtual void Initialize()
    {
        // 機体を子供に設定
        _myMs.transform.parent = transform;
        _myMs.myCamera = _cameraManager.mainCamera.transform;
        _cameraManager.myMs = _myMs.center;
        
        List<Transform> _enemyMs = new List<Transform>();
        foreach(BasePilot pilot in _enemyPilots)
        {
            _enemyMs.Add(pilot.myMs.center);
        }
        // 最初のターゲットを設定
        _targetPilot = _enemyPilots[0];
        _cameraManager.target = _targetPilot.myMs.center;
        _myMs.targetMs = _targetPilot.myMs;

        // 初期化
        _myMs.Initialize();
    }

    /// <summary>
    /// ターゲットチェンジ
    /// パイロットで呼んでもらう
    /// </summary>
    public void TargetChange()
    {
        if (_enemyPilots.Count - 1 == _taregetIndex)
        {
            _taregetIndex = 0;
        }
        _taregetIndex++;
        _targetPilot = _enemyPilots[_taregetIndex];

        // ターゲットを設定
        _cameraManager.target = _targetPilot.myMs.center;
        _myMs.targetMs = _targetPilot.myMs;
    }

    /// <summary>
    /// オブジェクトの処理を開始
    /// </summary>
    public override void Play()
    {
        base.Play();
        myMs.Play();
    }

    /// <summary>
    /// オブジェクトの処理を止める
    /// </summary>
    public override void Stop()
    {
        base.Stop();
        myMs.Stop();
    }
}

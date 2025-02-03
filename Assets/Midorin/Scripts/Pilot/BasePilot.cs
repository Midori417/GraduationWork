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
        _cameraManager.enemys = _enemyMs;
        // 最初のターゲットを設定
        _targetPilot = _enemyPilots[0];

        // 初期化
        _myMs.Initialize();
    }
}

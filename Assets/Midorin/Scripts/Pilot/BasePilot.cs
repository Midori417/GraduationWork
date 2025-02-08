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

    // チームパイロット
    private BasePilot _teamPilot;

    // ターゲットパイロット
    private BasePilot _targetPilot;
    private List<BasePilot> _enemyPilots;
    protected GameInput msInput = new GameInput();
    private int _targetIndex = 0;

    #region プロパティ

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
    public BasePilot teamPilot
    {
       protected get => _teamPilot;
        set => _teamPilot = value;
    }
    public BasePilot targetPilot => _targetPilot;
    public BaseMs targetMs
    {
        get
        {
            if (targetPilot)
            {
                return targetPilot.myMs;
            }
            return null;
        }
    }

    public List<BasePilot> enemyPilots
    {
        protected get => _enemyPilots;
        set => _enemyPilots = value;
    }
    protected CameraManager cameraManager => _cameraManager;

    // trueなら既にコストダウンされた
    private bool _isCostDown = false;

    // 復活処理作動時間
    private GameTimer _responTimer = new GameTimer(3);

    // trueならリスポーンする
    private bool _respon = false;

    private Vector3 _responPos = Vector3.zero;

    #endregion

    /// <summary>
    /// 機体が破壊されたときの処理
    /// </summary>
    protected virtual void MsUpdate()
    {
        // 機体が破壊された
        if (myMs.hp <= 0)
        {
            // コストダウンが行われてなければ行う
            if (!_isCostDown)
            {

                _isCostDown = true;
                BattleManager mana = BattleManager.I;
                mana.CostDown(_myMs.cost, _team);
                // まだコストがあれば復活処理をする
                if (mana.GetTeamCost(team) > 0)
                {
                    _responTimer.ResetTimer();
                    _responPos = mana.GetResponPos();
                    _respon = true;
                }
            }
            // 復活処理が行われる
            if (_respon)
            {
                // 破壊が終わっていたらタイマー作動
                if (myMs.isDestroy)
                {
                    if (_responTimer.UpdateTimer())
                    {
                        _myMs.Respon();
                        _myMs.transform.position = _responPos;
                        _respon = false;
                        _isCostDown = false;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public virtual void Initialize()
    {
        msInput.Initialize();

        // 機体を子供に設定
        _myMs.transform.parent = transform;
        _myMs.myCamera = _cameraManager.mainCamera.transform;
        _cameraManager.myMs = _myMs.center;
        _myMs.msInput = msInput;
        _myMs.team = team;

        List<Transform> _enemyMs = new List<Transform>();
        foreach (BasePilot pilot in _enemyPilots)
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
    /// ターゲットの更新
    /// </summary>
    protected void TargetUpdate()
    {
        if (msInput.GetInputDown(GameInputState.TargetChange))
        {
            TargetChange();
        }
        if (targetMs.hp <= 0)
        {
            TargetChange();
        }
    }

    /// <summary>
    /// ターゲットチェンジ
    /// </summary>
    private void TargetChange()
    {
        _targetIndex++;
        if (_targetIndex == _enemyPilots.Count)
        {
            _targetIndex = 0;
        }

        // ターゲットを設定
        if (_enemyPilots[_targetIndex].myMs.hp > 0)
        {
            _targetPilot = _enemyPilots[_targetIndex];
        }
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

    /// <summary>
    /// 勝敗を設定
    /// </summary>
    /// <param name="victory"></param>
    public virtual void SetVitory(Victory victory)
    {

    }
}

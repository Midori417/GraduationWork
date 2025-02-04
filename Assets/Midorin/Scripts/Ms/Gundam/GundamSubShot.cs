using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GundamSubShot : BaseMsAmoParts
{
    enum State
    {
        None,

        // 射撃前
        ShotF,

        // 射撃後
        ShotB,
    }
    StateMachine<State> _stateMachine = new StateMachine<State>();

    [SerializeField, Header("弾Prefab")]
    private BasicBulletMove _pfbBullet = null;

    [SerializeField, Header("マズルフラッシュ")]
    private ParticleSystem _psMuzzle = null;

    [SerializeField, Header("行動を始めてから射撃するまでの時間")]
    private float _shotTime = 0;

    [SerializeField, Header("射撃してから行動を終了するまでの時間")]
    private float _endTime = 0;

    [SerializeField, Header("弾生成位置")]
    private Vector3 _shotPos = Vector3.zero;

    [SerializeField, Header("反動")]
    private float _recoil = 0;

    [SerializeField, Header("インターバル")]
    private GameTimer _interval = new GameTimer();

    [SerializeField, Header("一発の弾がリロードされるまでの時間")]
    private GameTimer _reloadTime = new GameTimer();

    private Transform _target;

    private bool _isNow = false;

    public bool isNow => _isNow;

    /// <summary>
    /// 生成時に呼び出される
    /// </summary>
    private void Awake()
    {
        if(_psMuzzle)
            _psMuzzle.Stop();
        SetUp();
    }

    #region 状態

    /// <summary>
    /// 状態をセットアップ
    /// </summary>
    private void SetUp()
    {
        SetUpNone();
        SetUpShotF();
        SetUpShotB();
        _stateMachine.Setup(State.None);
    }

    /// <summary>
    /// None状態をセットアップ
    /// </summary>
    private void SetUpNone()
    {
        State state = State.None;
        Action<State> enter = (prev) =>
        {
        };
        Action update = () =>
        {
            if (_interval.UpdateTimer())
            {
                if (msInput._subShot && amo > 0)
                {
                    _stateMachine.ChangeState(State.ShotF);
                }
            }
        };
        Action lateUpdate = () =>
        {
        };
        Action<State> exit = (next) =>
        {
        };
        _stateMachine.AddState(state, enter, update, lateUpdate, exit);
    }

    /// <summary>
    /// ShotF状態をセットアップ
    /// </summary>
    private void SetUpShotF()
    {
        State state = State.ShotF;
        GameTimer timer = new GameTimer();
        Action<State> enter = (prev) =>
        {
            rb.AddForce(transform.up * 0.5f, ForceMode.Impulse);
            if (_psMuzzle)
                _psMuzzle.Play();
            _isNow = true;
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            timer.ResetTimer(_shotTime);
            // ターゲットを設定
            _target = targetMs;
            animator.SetInteger("ShotType", 2);
            animator.SetTrigger("Shot");
        };
        Action update = () =>
        {
            if (_target)
            { 
                // ターゲット方向の回転を計算
                Vector3 directionToTarget = Vector3.Scale(_target.position - transform.position, new Vector3(1, 0, 1));
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                transform.rotation = targetRotation;
            }
            // 射撃時間になったら弾を生成
            if (timer.UpdateTimer())
            {
                CreateBullet();
                _stateMachine.ChangeState(State.ShotB);
            }
        };
        Action lateUpdate = () =>
        {
        };
        Action<State> exit = (next) =>
        {
        };
        _stateMachine.AddState(state, enter, update, lateUpdate, exit);
    }

    /// <summary>
    /// ShotB状態をセットアップ
    /// </summary>
    private void SetUpShotB()
    {
        State state = State.ShotB;
        GameTimer timer = new GameTimer();
        Action<State> enter = (prev) =>
        {
            timer.ResetTimer(_endTime);
        };
        Action update = () =>
        {
            // 射撃状態を終了
            if (timer.UpdateTimer())
            {
                _stateMachine.ChangeState(State.None);
            }
        };
        Action lateUpdate = () =>
        {
        };
        Action<State> exit = (next) =>
        {
            rb.useGravity = true;
            _isNow = false;
            _target = null;
            _interval.ResetTimer();
        };
        _stateMachine.AddState(state, enter, update, lateUpdate, exit);
    }

    #endregion

    /// <summary>
    /// 初期化処理
    /// </summary>
    public override void Initalize()
    {
        base.Initalize();
        amo = amoMax;
    }

    /// <summary>
    /// サブ射撃
    /// </summary>
    public void SubShot()
    {
        Reload();
        _stateMachine.UpdateState();
    }

    /// <summary>
    /// リロード処理
    /// </summary>
    private void Reload()
    {
        if (amo <= 0)
        {
            if (_reloadTime.UpdateTimer())
            {
                amo = amoMax;
            }
        }
    }

    /// <summary>
    /// 弾を生成
    /// </summary>
    private void CreateBullet()
    {
        if (!_pfbBullet) return;

        if (_target)
        {
            // ターゲットの方向を計算
            Vector3 directionToTarget = _target.position - mainMs.center.position;
            Quaternion rot = Quaternion.LookRotation(directionToTarget);
            // 弾の生成位置を計算
            Vector3 pos = mainMs.transform.position + rot * _shotPos;

            // 弾を生成
            BasicBulletMove bullet = Instantiate(_pfbBullet, pos, rot);
            bullet.target = _target;
        }
        else
        {
            // 自身の機体のセンターを代入
            Transform center = mainMs.center;
            Quaternion rot = transform.rotation;
            Vector3 pos = center.position + rot * _shotPos;
            BasicBulletMove bullet = Instantiate(_pfbBullet, pos, rot);
        }
        rb.AddForce(-transform.forward * _recoil, ForceMode.Impulse);
        // 弾を減らす
        amo--;
        if (amo <= 0)
        {
            _reloadTime.ResetTimer();
        }
    }
}

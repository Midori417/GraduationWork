using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

/// <summary>
/// ガンダム
/// </summary>
public class Gundam : BaseMs
{
    enum State
    {
        // 通常
        Normal,
        // 着地
        Landing,
    }
    StateMachine<State> _stateMachine = new StateMachine<State>();

    private MsMove _move;
    private GundamMainShot _mainShot;
    private GundamSubShot _subShot;
    [SerializeField, Header("MsDamage")]
    private MsDamage _msDamage;

    [Serializable]
    private struct ActiveObject
    {
        [Header("ビームライフル")]
        public GameObject _beumRifle;

        [Header("バズーカ")]
        public GameObject _bazooka;

        [Header("サーベル")]
        public GameObject _sable;

        [Header("バ―ニア")]
        public GameObject _roketFire;

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            BeumRifleActive(true);
            BazookaActive(false);
            SableActive(false);
            RoketFireActive(false);
        }

        /// <summary>
        /// ビームライフルの切り替え
        /// </summary>
        /// <param name="value"></param>
        public void BeumRifleActive(bool value)
        {
            if (!_beumRifle) return;
            _beumRifle.SetActive(value);
        }

        /// <summary>
        /// バズーカの切り替え
        /// </summary>
        /// <param name="value"></param>
        public void BazookaActive(bool value)
        {
            if (!_bazooka) return;
            _bazooka.SetActive(value);
        }

        /// <summary>
        /// サーベルの切り替え
        /// </summary>
        /// <param name="value"></param>
        public void SableActive(bool value)
        {
            if (!_sable) return;
            _sable.SetActive(value);
        }

        /// <summary>
        /// バーニアの切り替え
        /// </summary>
        /// <param name="value"></param>
        public void RoketFireActive(bool value)
        {
            if (!_roketFire) return;
            _roketFire.SetActive(value);
        }
    }
    [SerializeField, Header("オブジェクト")]
    private ActiveObject _activeObj;

    private int _layer = -1;

    #region イベント関数

    /// <summary>
    /// 生成時に呼び出される
    /// </summary>
    private void Awake()
    {
        _move = GetComponent<MsMove>();
        _mainShot = GetComponent<GundamMainShot>();
        _subShot = GetComponent<GundamSubShot>();
        SetUp();
    }

    /// <summary>
    /// Updateより前に呼び出される
    /// </summary>
    private void Start()
    {
        // レイヤー番号を取得
        _layer = animator.GetLayerIndex("BeumRifleLayer");
    }

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    private void Update()
    {
        if (isStop) return;

        if (DestroyCheck())
        {
            // 破壊された
            return;
        }
        _stateMachine.UpdateState();
        AnimUpdate();
        BoostCharge();
    }

    /// <summary>
    /// Updateの後に呼び出される
    /// </summary>
    private void LateUpdate()
    {
        _stateMachine.LateUpdateState();
    }

    #endregion

    /// <summary>
    /// 初期化する
    /// パイロットに呼び出してもらう
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();
        if (_move)
        {
            _move.SetMainMs(this);
            _move.Initalize();
        }
        if (_mainShot)
        {
            _mainShot.SetMainMs(this);
            _mainShot.Initalize();
        }
        if (_subShot)
        {
            _subShot.SetMainMs(this);
            _subShot.Initalize();
        }
        if(_msDamage)
        {
            _msDamage.SetMainMs(this);
            _msDamage.Initalize();
        }
        _activeObj.Initialize();
    }

    /// <summary>
    /// 常に更新するアニメーション変数
    /// </summary>
    private void AnimUpdate()
    {
        float speed = new Vector2(rb.velocity.x, rb.velocity.z).magnitude;
        animator.SetFloat("Speed", speed);
        animator.SetBool("IsGround", groundCheck.isGround);
    }

    #region 状態

    /// <summary>
    /// 状態のセットアップ
    /// </summary>
    private void SetUp()
    {
        SetUpNormal();
        SetUpLanding();
        _stateMachine.Setup(State.Normal);
    }

    /// <summary>
    /// 通常状態をセットアップ
    /// </summary>
    private void SetUpNormal()
    {
        State state = State.Normal;
        Action<State> enter = (prev) =>
        {
        };
        Action update = () =>
        {
            // 着地
            if (groundCheck.isGround && !groundCheck.oldIsGround)
                _stateMachine.ChangeState(State.Landing);

            Move();
            SubShot();

            bool isRoket = _move.isDash || _move.isJump || _subShot.isNow;
            _activeObj.RoketFireActive(isRoket);
        };
        Action lateUpdate = () =>
        {
            MainShot();
        };
        Action<State> exit = (next) =>
        {
        };
        _stateMachine.AddState(state, enter, update, lateUpdate, exit);
    }

    /// <summary>
    /// 着地状態をセットアップ
    /// </summary>
    private void SetUpLanding()
    {
        State state = State.Landing;
        GameTimer timer = new GameTimer(1);
        Action<State> enter = (prev) =>
        {
            _move.Landing();
            animator.SetTrigger("Landing");
            timer.ResetTimer();
        };
        Action update = () =>
        {
            if (timer.UpdateTimer())
            {
                _stateMachine.ChangeState(State.Normal);
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

    #endregion


    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        if (_subShot.isNow)
            return;
        _move.MoveUpdate();
    }

    /// <summary>
    /// メイン射撃
    /// </summary>
    private void MainShot()
    {
        // バズーカ中は不可
        if (_subShot.isNow)
            return;

        _mainShot.MainShot();
    }

    /// <summary>
    /// サブ射撃
    /// </summary>
    private void SubShot()
    {
        if (_mainShot.isNow) return;
        _subShot.SubShot();
        // オブジェクトの切り替え
        if (_subShot.isNow)
        {
            animator.SetLayerWeight(_layer, 0);
            _activeObj.BazookaActive(true);
            _activeObj.BeumRifleActive(false);
        }
        else
        {
            _activeObj.BazookaActive(false);
            _activeObj.BeumRifleActive(true);
        }
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GundamDamage : BaseMsParts
{
    enum State
    {
        // 通常
        Normal,

        // ダメージ
        Damage,

        // ダウン
        Down,

        // 立ち上がり
        Standing
    }
    StateMachine<State> _stateMachine = new StateMachine<State>();

    // ダメージを受けた方向
    private Vector3 _directionToTarget = Vector3.zero;

    [SerializeField, Header("赤く発行している時間")]
    private GameTimer _redMeshTimer = new GameTimer(0.3f);

    [SerializeField, Header("ダメージ状態時間")]
    private float _damageTime = 0;

    [SerializeField, Header("ダウンから自動的に立ち上がる時間")]
    private float _autoStandingtime = 0;

    [SerializeField, Header("立ち上がり状態時間")]
    private float _standingTime = 0;

    [SerializeField, Header("ダウン状態からの無敵時間")]
    private float _invisibleTime = 0;

    #region プロパティ

    public bool isDamage => _stateMachine.currentState == State.Damage;
    public bool isDown => _stateMachine.currentState == State.Down;
    public bool isStanding => _stateMachine.currentState == State.Standing;

    #endregion

    #region イベント関数

    /// <summary>
    /// 生成時に呼び出される
    /// </summary>
    private void Awake()
    {
        SetUp();
    }

    #endregion

    #region 状態

    /// <summary>
    /// 状態をセットアップ
    /// </summary>
    private void SetUp()
    {
        SetUpNormal();
        SetUpDamage();
        SetUpDown();
        SetUpStanding();
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
    /// ダメージ状態をセットアップ
    /// </summary>
    private void SetUpDamage()
    {
        State state = State.Damage;
        GameTimer timer = new GameTimer();
        Action<State> enter = (prev) =>
        {
            // タイマーをリセット
            {
                timer.ResetTimer(_damageTime);
                _redMeshTimer.ResetTimer();
            }
            rb.velocity = Vector3.zero;
            // 後ろに後退
            rb.AddForce(_directionToTarget * 10.0f, ForceMode.Impulse);
            // 赤く発行
            mainMs.RedMesh();
            // アニメーション
            animator.SetTrigger("Damage");

        };
        Action update = () =>
        {
            if(_redMeshTimer.UpdateTimer())
            {
                mainMs.NormalMesh();
            }
            // 一定時間立てば通常状態にする
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

    /// <summary>
    /// ダウン状態をセットアップ
    /// </summary>
    private void SetUpDown()
    {
        State state = State.Down;
        GameTimer timer = new GameTimer();
        // 地面についてから入力を受け付ける時間
        GameTimer inputTimer = new GameTimer(1);
        Action<State> enter = (prev) =>
        {
            // タイマーをリセット
            {
                inputTimer.ResetTimer();
                timer.ResetTimer(_autoStandingtime);
                _redMeshTimer.ResetTimer();
            }
            // 後方に吹き飛ばされる
            {
                rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
                rb.AddForce(_directionToTarget * 20, ForceMode.Impulse);
            }
            // アニメーション
            animator.SetTrigger("Down");
        };
        Action update = () =>
        {
            if (_redMeshTimer.UpdateTimer())
            {
                mainMs.NormalMesh();
            }
            // 地面についていたらタイマー作動
            if(groundCheck.isGround)
            {
                if(inputTimer.UpdateTimer())
                {
                    if (msInput._move != Vector2.zero)
                    {
                        _stateMachine.ChangeState(State.Standing);
                    }
                }
                if (timer.UpdateTimer())
                {
                    _stateMachine.ChangeState(State.Standing);
                }
            }
        };
        Action lateUpdate = () =>
        {
        };
        Action<State> exit = (next) =>
        {
            // ダウン値をリセット
            mainMs.downValue = 0;
        };
        _stateMachine.AddState(state, enter, update, lateUpdate, exit);
    }

    /// <summary>
    /// 立ち上がり状態をセットアップ
    /// </summary>
    private void SetUpStanding()
    {
        State state = State.Standing;
        GameTimer timer = new GameTimer();
        Action<State> enter = (prev) =>
        {
            // タイマーのリセット
            timer.ResetTimer(_standingTime);
            animator.SetTrigger("Standing");
        };
        Action update = () =>
        {
            if(timer.UpdateTimer())
            {
                mainMs.InvisibleTimer(_invisibleTime);
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
    /// 状態を更新
    /// </summary>
    public void UpdateState()
    {
        _stateMachine.UpdateState();
    }

    /// <summary>
    /// ステートチェンジ
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="downValue"></param>
    /// <param name="bulletPos"></param>
    public void SetState(int damage, int downValue, Vector3 bulletPos)
    {
        rb.useGravity = true;
        _directionToTarget = Vector3.Scale(transform.position - bulletPos, new Vector3(1, 0, 1));
        float dot = Vector3.Dot(_directionToTarget.normalized, transform.forward);
        // ダメージを受けた方向を計算
        if (dot > 0)
        {
            // 背面から
            Quaternion targetRotation = Quaternion.LookRotation(_directionToTarget);
            transform.rotation = targetRotation;
            animator.SetInteger("DamageDirection", 1);
        }
        else
        {
            // 正面からくらった
            Vector3 reverseDirectionToTarget = _directionToTarget * -1.0f;
            Quaternion targetRotation = Quaternion.LookRotation(reverseDirectionToTarget);
            transform.rotation = targetRotation;
            animator.SetInteger("DamageDirection", 0);
        }

        // 破壊されている
        if (mainMs.hp <= 0)
        {
            rb.AddForce(Vector3.up * 20, ForceMode.Impulse);
            rb.AddForce(_directionToTarget * 30, ForceMode.Impulse);
            return;
        }

        // ダメージ
        if(mainMs.downValue < 5)
        {
            _stateMachine.ChangeState(State.Damage);
        }
        else
        {
            _stateMachine.ChangeState(State.Down);
        }
    }
}

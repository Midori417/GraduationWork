using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GundamMelee : BaseMsParts
{
    enum State
    {
        None,

        Move,

        Attack1,

        Attack2,

        Attack3,
    }
    StateMachine<State> _stateMachine = new StateMachine<State>();

    [SerializeField, Header("攻撃判定")]
    private MeleeCollision _attackCollision;

    [SerializeField, Header("攻撃可能距離")]
    private float _attackDistace = 0;

    [SerializeField, Header("移動速度")]
    private float _moveSpeed = 0;

    [SerializeField, Header("攻撃時の移動速度")]
    private float _attackSpeed = 0;

    [Serializable]
    private struct StateTime
    {
        [SerializeField, Header("移動状態")]
        public float _move;

        [SerializeField, Header("攻撃１")]
        public float _attack1;

        [SerializeField, Header("攻撃1の判定")]
        public float _attack1Coll;

        [SerializeField, Header("攻撃2")]
        public float _attack2;

        [SerializeField, Header("攻撃2の判定")]
        public float _attack2Coll;

        [SerializeField, Header("攻撃3")]
        public float _attack3;

        [SerializeField, Header("攻撃3の判定")]
        public float _attack3Coll;
    }
    [SerializeField, Header("状態の時間")]
    private StateTime _stateTime;

    [Serializable]
    private struct AtkDown
    {
        [SerializeField, Header("攻撃")]
        public int _atk;

        [SerializeField, Header("ダウン")]
        public float _down;
    }
    [Serializable]
    private struct AtkDownList
    {
        [SerializeField, Header("攻撃1")]
        public AtkDown _attack1;

        [SerializeField, Header("攻撃2")]
        public AtkDown _attack2;

        [SerializeField, Header("攻撃3")]
        public AtkDown _attack3;
    }
    [SerializeField, Header("攻撃とダウン")]
    private AtkDownList _atkDown;

    // true ならcombo攻撃
    private bool _isCombo = false;

    private Transform _target;

    [SerializeField, Header("インターバル")]
    private GameTimer _intervalTimer = new GameTimer(0);

    [SerializeField, Header("使用ブースト量")]
    private float userBoost = 0;

    private float _interval = 1;

    [SerializeField, Header("サーベル攻撃音")]
    private AudioClip _seSable;

    #region プロパティ

    public bool isNow => _stateMachine.currentState != State.None;

    #endregion

    #region イベント関数

    /// <summary>
    /// 生成時に実行
    /// </summary>
    private void Awake()
    {
        SetUp();
    }

    #endregion

    #region 状態

    /// <summary>
    /// 状態のセットアップ
    /// </summary>
    private void SetUp()
    {
        SetUpNone();
        SetUpMove();
        SetUpAttack1();
        SetUpAttack2();
        SetUpAttack3();
        _stateMachine.SetUp(State.None);
    }

    /// <summary>
    /// None状態のセットアップ
    /// </summary>
    private void SetUpNone()
    {
        State state = State.None;
        GameTimer timer = new GameTimer();
        Action<State> enter = (prev) =>
        {
            if(rb)
            rb.useGravity = true;
        };
        Action update = () =>
        {
            if (!_intervalTimer.UpdateTimer())
                return;

            // 攻撃入力があれば
            if (msInput.GetInputDown(GameInputState.MainAttack))
            {
                // すでに攻撃可能距離ならすぐに攻撃1に移る
                if (mainMs.targetDistance < _attackDistace)
                {
                    _stateMachine.ChangeState(State.Attack1);
                }
                else
                {
                    _stateMachine.ChangeState(State.Move);
                }
            }
        };
        Action<State> exit = (next) =>
        {
            // 赤距離なのでターゲットを設定
            if (mainMs.isRedDistance)
            {
                _target = targetMs.transform;
            }
            rb.useGravity = false;
        };
        _stateMachine.AddState(state, enter, update, exit);
    }

    /// <summary>
    /// 移動状態のセットアップ
    /// </summary>
    private void SetUpMove()
    {
        State state = State.Move;
        GameTimer timer = new GameTimer();
        Action<State> enter = (prev) =>
        {
            timer.ResetTimer(_stateTime._move);
            animator.SetInteger("SableType", 0);
            animator.SetTrigger("Sable");
        };
        Action update = () =>
        {
            // 移動時間か攻撃距離になれば攻撃位置に移る
            if (timer.UpdateTimer() || mainMs.targetDistance < _attackDistace || mainMs.boostRate <=0)
            {
                _stateMachine.ChangeState(State.Attack1);
            }
            else if (!timer.UpdateTimer())
            {
                // ターゲットの方向に向きながら進む
                LookTarget(true);
                rb.velocity = transform.forward * _moveSpeed;
                mainMs.UseBoost(userBoost);
            }
        };
        Action<State> exit = (next) =>
        {
        };
        _stateMachine.AddState(state, enter, update, exit);
    }

    /// <summary>
    /// 攻撃状態のセットアップ
    /// </summary>
    private void SetUpAttack1()
    {
        State state = State.Attack1;
        GameTimer timer = new GameTimer();
        GameTimer attackTimer = new GameTimer();
        Action<State> enter = (prev) =>
        {
            // タイマーのリセット
            {
                timer.ResetTimer(_stateTime._attack1);
                attackTimer.ResetTimer(_stateTime._attack1Coll);
            }
            // アニメーションを設定
            {
                animator.SetInteger("SableType", 1);
                animator.SetTrigger("Sable");
            }
            // ターゲットの方向に向く
            LookTarget(false);
            // 攻撃判定にステータスを与える
            _attackCollision.SetAtkDown(_atkDown._attack1._atk, _atkDown._attack1._down);
            // サウンド
            audio.MainSe(_seSable);
        };
        Action update = () =>
        {
            // 攻撃時間になれば攻撃判定を付与
            if (attackTimer.UpdateTimer())
            {
                _attackCollision.isCollision = true;
            }

            if (timer.UpdateTimer())
            {
                if (_isCombo)
                {
                    _stateMachine.ChangeState(State.Attack2);
                }
                else
                {
                    _stateMachine.ChangeState(State.None);
                }
            }
            else
            {
                mainMs.UseBoost(userBoost);
                rb.velocity = transform.forward * _attackSpeed;

                // 時間内に攻撃入力されたらコンボ入力とみなす
                if (msInput.GetInputDown(GameInputState.MainAttack))
                {
                    _isCombo = true;
                }
            }
        };
        Action<State> exit = (next) =>
        {
            AttackFaild();
        };
        _stateMachine.AddState(state, enter, update, exit);
    }

    /// <summary>
    /// 攻撃2状態のセットアップ
    /// </summary>
    private void SetUpAttack2()
    {
        State state = State.Attack2;
        GameTimer timer = new GameTimer();
        GameTimer attackTimer = new GameTimer();
        Action<State> enter = (prev) =>
        {
            // タイマーのリセット
            {
                timer.ResetTimer(_stateTime._attack2);
                attackTimer.ResetTimer(_stateTime._attack2Coll);
            }
            // アニメーションを設定
            {
                animator.SetInteger("SableType", 2);
                animator.SetTrigger("Sable");
            }
            // ターゲットの方向に向く
            LookTarget(false);
            // 攻撃判定にステータスを与える
            _attackCollision.SetAtkDown(_atkDown._attack2._atk, _atkDown._attack2._down);
            // サウンド
            audio.MainSe(_seSable);
        };
        Action update = () =>
        {
            // 攻撃時間になれば攻撃判定を付与
            if (attackTimer.UpdateTimer())
            {
                _attackCollision.isCollision = true;
            }
            if (timer.UpdateTimer())
            {
                if (_isCombo)
                {
                    _stateMachine.ChangeState(State.Attack3);
                }
                else
                {
                    _stateMachine.ChangeState(State.None);
                }
            }
            else
            {
                rb.velocity = transform.forward * _attackSpeed;
                mainMs.UseBoost(userBoost);

                // 時間内に攻撃入力があればコンボとみなす
                if (msInput.GetInputDown(GameInputState.MainAttack))
                {
                    _isCombo = true;
                }
            }
        };
        Action<State> exit = (next) =>
        {
            AttackFaild();
        };
        _stateMachine.AddState(state, enter, update, exit);
    }

    /// <summary>
    /// 攻撃3状態のセットアップ
    /// </summary>
    private void SetUpAttack3()
    {
        State state = State.Attack3;
        GameTimer timer = new GameTimer();
        GameTimer attackTimer = new GameTimer();
        Action<State> enter = (prev) =>
        {
            // タイマーのリセット
            {
                timer.ResetTimer(_stateTime._attack3);
                attackTimer.ResetTimer(_stateTime._attack3Coll);
            }
            // アニメーションを設定
            {
                animator.SetInteger("SableType", 3);
                animator.SetTrigger("Sable");
            }
            // ターゲットの方向に向く
            LookTarget(false);
            // 攻撃判定にステータスを与える
            _attackCollision.SetAtkDown(_atkDown._attack3._atk, _atkDown._attack3._down);
            // サウンド
            audio.MainSe(_seSable);
        };
        Action update = () =>
        {
            // 攻撃判定
            if (attackTimer.UpdateTimer())
            {
                _attackCollision.isCollision = true;
            }
            if (timer.UpdateTimer())
            {
                _stateMachine.ChangeState(State.None);
            }
            else
            {
                rb.velocity = transform.forward * _attackSpeed;
                mainMs.UseBoost(userBoost);
            }
        };
        Action<State> exit = (next) =>
        {
            AttackFaild();
        };
        _stateMachine.AddState(state, enter, update, exit);
    }

    #endregion

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Initalize()
    {
        base.Initalize();
        if (!_attackCollision.mainMs)
        {
            _attackCollision.mainMs = mainMs;
        }
        _stateMachine.ChangeState(State.None);
        _isCombo = false;
        _target = null;
        _attackCollision.isCollision = false;
        animator.SetInteger("SableType", -1);
    }

    /// <summary>
    /// 状態の更新
    /// </summary>
    public void UpdateState()
    {
        _stateMachine.UpdateState();
    }

    /// <summary>
    /// ターゲットの方向に向く
    /// </summary>
    private void LookTarget(bool isY)
    {
        if (!_target)
        {
            return;
        }
        // ターゲット方向の回転を計算
        Vector3 directionToTarget = Vector3.zero;
        if (isY)
        {
            directionToTarget = Vector3.Scale(_target.position - transform.position, new Vector3(1, 1, 1));
        }
        else
        {
            directionToTarget = Vector3.Scale(_target.position - transform.position, new Vector3(1, 0, 1));
        }

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = targetRotation;
    }

    /// <summary>
    /// 行動終了処理
    /// </summary>
    private void AttackFaild()
    {
        if (!_isCombo)
        {
            _target = null;
            animator.SetInteger("SableType", -1);
        }

        _isCombo = false;
        _attackCollision.isCollision = false;
        _attackCollision.End();
        _intervalTimer.ResetTimer(_interval);
    }
}
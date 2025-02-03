using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Msの移動
/// </summary>
public class MsMove : BaseMsParts
{
    enum State
    {
        // 通常移動
        Normal,

        // ジャンプ
        Jump,

        // ダッシュ
        Dash,
    }
    StateMachine<State> _stateMachine = new StateMachine<State>();

    // 自身のカメラ
    private Transform _myCamera;

    [Serializable]
    private struct MoveValiable
    {
        [Header("移動速度")]
        public float _speed;

        [Header("旋回速度")]
        public float _rotationSpeed;
    }
    [SerializeField, Header("移動パラメータ")]
    private MoveValiable _move;

    [Serializable]
    private struct JumpValiable
    {
        [Header("ジャンプ力")]
        public float _power;

        [Header("移動速度")]
        public float _speed;

        [Header("旋回速度")]
        public float _rotationSpeed;

        [Header("ブーストゲージの消費量")]
        public float useBoost;

        [HideInInspector, Header("ジャンプ中か")]
        public bool isNow;

        [Header("着地時の慣性")]
        public float inertia;
    }
    [SerializeField, Header("ジャンプパラメータ")]
    private JumpValiable _jump;

    [Serializable]
    private struct DashValiable
    {
        [Header("移動速度")]
        public float speed;

        [Header("旋回速度")]
        public float rotationSpeed;

        [Header("ブーストゲージの消費量")]
        public float useBoost;

        [HideInInspector, Header("ダッシュ中か")]
        public bool isNow;
    }
    [SerializeField, Header("ダッシュパラメータ")]
    private DashValiable _dash;

    #region プロパティ

    public bool isJump => _jump.isNow;
    public bool isDash => _dash.isNow;

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

    /// <summary>
    /// 移動処理
    /// </summary>
    public void MoveUpdate()
    {
        _stateMachine.UpdateState();
        // アニメータ変数処理
        animator.SetBool("Jump", isJump);
        animator.SetBool("Dash", isDash);
    }

    #region 状態

    /// <summary>
    /// 状態をセットアップ
    /// </summary>
    private void SetUp()
    {
        SetUpNormal();
        SetUpJump();
        SetUpDash();
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
            if (groundCheck.isGround)
            {
                // 進行方向に回転しながら正面方向に進む
                if (msInput._move != Vector2.zero)
                {
                    Vector3 moveFoward = MoveForward();
                    MoveForwardRot(moveFoward, _move._rotationSpeed);
                    rb.velocity = transform.forward * _move._speed + new Vector3(0, rb.velocity.y, 0);
                }
                else
                {
                    // 移動入力がなくなったら速度をなくす
                    rb.velocity = new Vector3(0, rb.velocity.y, 0);
                }
            }
            if (mainMs.boost01 > 0)
            {
                if (msInput._jump && !isDash)
                {
                    _stateMachine.ChangeState(State.Jump);
                    return;
                }
                if (msInput._dash)
                {
                    _stateMachine.ChangeState(State.Dash);
                    return;
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
    /// ジャンプ状態をセットアップ
    /// </summary>
    private void SetUpJump()
    {
        State state = State.Jump;
        Action<State> enter = (prev) =>
        {
            _jump.isNow = true;
            rb.useGravity = false;
        };
        Action update = () =>
        {
            if (!msInput._jump || mainMs.boost01 <= 0)
            {
                _stateMachine.ChangeState(State.Normal);
                return;
            }
            if (msInput._dash)
            {
                _stateMachine.ChangeState(State.Dash);
                return;
            }
            Vector3 moveFoward = MoveForward();

            // 進行方向に補間しながら回転
            if (moveFoward != Vector3.zero)
            {
                MoveForwardRot(moveFoward, _jump._rotationSpeed);
                rb.velocity = transform.forward * _jump._speed + new Vector3(0, rb.velocity.y, 0);
            }
            rb.velocity = new Vector3(rb.velocity.x, _jump._power, rb.velocity.z);

            // エネルギーの使用
            mainMs.UseBoost(_jump.useBoost);

        };
        Action lateUpdate = () =>
        {
        };
        Action<State> exit = (next) =>
        {
            rb.useGravity = true;
            _jump.isNow = false;
        };
        _stateMachine.AddState(state, enter, update, lateUpdate, exit);
    }

    /// <summary>
    /// ダッシュ状態をセットアップ
    /// </summary>
    private void SetUpDash()
    {
        State state = State.Dash;
        Action<State> enter = (prev) =>
        {
            _dash.isNow = true;
            rb.useGravity = false;
            transform.Translate(0, 1, 0);
        };
        Action update = () =>
        {
            // 入力かブーストがなくなれば通常に戻どす
            if (!msInput._dash || mainMs.boost01 <= 0)
            {
                _stateMachine.ChangeState(State.Normal);
                return;
            }

            Vector3 moveFoward = MoveForward();
            // 進行方向に補間しながら回転
            if (moveFoward != Vector3.zero)
            {
                MoveForwardRot(moveFoward, _dash.rotationSpeed);
            }
            rb.velocity = transform.forward * _dash.speed;

            // エネルギーの使用
            mainMs.UseBoost(_dash.useBoost);
        };
        Action lateUpdate = () =>
        {
        };
        Action<State> exit = (next) =>
        {
            rb.useGravity = true;
            _dash.isNow = false;
        };
        _stateMachine.AddState(state, enter, update, lateUpdate, exit);
    }

    #endregion

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Initalize()
    {
        base.Initalize();
        _myCamera = mainMs.myCamera;
    }

    /// <summary>
    /// 着地処理
    /// </summary>
    public void Landing()
    {
        rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, _jump.inertia);
    }

    /// <summary>
    /// カメラを基準に移動方向を取得
    /// </summary>
    /// <returns></returns>
    private Vector3 MoveForward()
    {
        Vector2 moveAxis = msInput._move;
        // カメラの方向から、X-Z単位ベクトル(正規化)を取得
        Vector3 cameraForward = Vector3.Scale(_myCamera.forward, new Vector3(1, 0, 1));
        Vector3 moveForward = cameraForward * moveAxis.y + _myCamera.right * moveAxis.x;

        return moveForward;
    }

    /// <summary>
    /// 進行方向に回転
    /// </summary>
    private void MoveForwardRot(Vector3 moveForward, float rotSpeed)
    {
        Quaternion rotation = Quaternion.LookRotation(moveForward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotSpeed * Time.deltaTime);
    }
}

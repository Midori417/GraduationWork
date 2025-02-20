using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステップパーツ
/// </summary>
public class MsStep : BaseMsParts
{
    enum State
    {
        None,

        // 入力外し待ち
        NoInputStanby,

        // 入力待ち
        InputStanby,

        // ステップ処理
        Step,
    }
    private StateMachine<State> _stateMachine = new StateMachine<State>();

    enum Dir
    {
        None,
        F,
        B,
        R,
        L
    }
    private Dir dir = Dir.None;

    // カメラ
    private Transform _camera;

    [SerializeField, Header("ステップの速度")]
    private float _speed = 0;

    [SerializeField, Header("入力待ち時間")]
    private GameTimer _inputTime = new GameTimer();

    [SerializeField, Header("ステップの長さ")]
    private float _stepTime = 0;

    private Vector2 _inputAxis = Vector2.zero;

    [SerializeField, Header("ステップサウンド")]
    private AudioClip _seStep;

    #region プロパティ

    public bool isNow => _stateMachine.currentState == State.Step;

    #endregion

    #region イベント関数

    /// <summary>
    /// 生成時に呼び出す
    /// </summary>
    private void Awake()
    {
        SetUp();
    }

    #endregion

    #region 状態関数

    /// <summary>
    /// 状態のセットアップ
    /// </summary>
    private void SetUp()
    {
        SetUpNone();
        SetUpNoInputStanby();
        SetUpInputStanby();
        SetUpStep();
        _stateMachine.SetUp(State.None);
    }

    /// <summary>
    /// None状態をセットアップ
    /// </summary>
    private void SetUpNone()
    {
        State state = State.None;
        Action<State> enter = (prev) =>
        {
            dir = Dir.None;
        };
        Action update = () =>
        {
            dir = InputDirection();
            if (dir != Dir.None)
            {
                _inputTime.ResetTimer();
                _stateMachine.ChangeState(State.NoInputStanby);
            }
            // CPU専用
            if(msInput.GetInputDown(GameInputState.CPUStep))
            {
                _stateMachine.ChangeState(State.Step);
            }
        };
        Action<State> exit = (next) =>
        {
        };
        _stateMachine.AddState(state, enter, update, exit);
    }

    /// <summary>
    /// InputStanby状態をセットアップ
    /// </summary>
    private void SetUpNoInputStanby()
    {
        State state = State.NoInputStanby;
        Action<State> enter = (prev) =>
        {
        };
        Action update = () =>
        {
            if (_inputTime.UpdateTimer())
            {
                _stateMachine.ChangeState(State.None);
            }
            else
            {
                if (InputDirection() == Dir.None)
                {
                    _stateMachine.ChangeState(State.InputStanby);
                }
            }
        };
        Action<State> exit = (next) =>
        {
        };
        _stateMachine.AddState(state, enter, update, exit);
    }

    /// <summary>
    /// InputStanby状態をセットアップ
    /// </summary>
    private void SetUpInputStanby()
    {
        State state = State.InputStanby;
        Action<State> enter = (prev) =>
        {
        };
        Action update = () =>
        {
            if (_inputTime.UpdateTimer())
            {
                _stateMachine.ChangeState(State.None);
            }
            else
            {
                if (dir == InputDirection() && mainMs.boostRate > 0)
                {
                    _stateMachine.ChangeState(State.Step);
                }
            }
        };
        Action<State> exit = (next) =>
        {
        };
        _stateMachine.AddState(state, enter, update, exit);
    }

    /// <summary>
    /// Step状態をセットアップ
    /// </summary>
    private void SetUpStep()
    {
        State state = State.Step;
        GameTimer timer = new GameTimer();
        Vector3 moveFoward = Vector3.zero;

        Action<State> enter = (prev) =>
        {
            timer.ResetTimer(_stepTime);
            mainMs.UseBoost(10, true);
            audio.MainSe(_seStep);
            mainMs.homingCut = true;

            if (!_camera) _camera = mainMs.myCamera;
            Vector2 input = _inputAxis.normalized;
            Vector3 cameraForward = Vector3.Scale(_camera.forward, new Vector3(1, 0, 1));
            moveFoward = cameraForward * input.y + _camera.right * input.x;

            // 入力方向の計算
            Vector3 perendicular = Vector3.Cross(moveFoward, transform.forward);
            float dot = Vector3.Dot(moveFoward, transform.forward);
            if (dot > 0.5f)
            {
                animator.SetInteger("StepType", 0);
            }
            else if (dot < -0.5f)
            {
                animator.SetInteger("StepType", 1);
            }
            else
            {
                if (perendicular.y > 0)
                {
                    animator.SetInteger("StepType", 2);
                }
                else if (perendicular.y < 0)
                {
                    animator.SetInteger("StepType", 3);
                }
            }
            animator.SetTrigger("Step");
        };
        Action update = () =>
        {
            if (timer.UpdateTimer())
            {
                _stateMachine.ChangeState(State.None);
            }
            else
            {
                mainMs.UseBoost(30);
                rb.velocity = moveFoward * _speed;
            }
        };
        Action<State> exit = (next) =>
        {
            mainMs.homingCut = false;
            rb.velocity = Vector3.zero;
        };
        _stateMachine.AddState(state, enter, update, exit);
    }

    #endregion

    /// <summary>
    /// 更新
    /// </summary>
    public void UpdateState()
    {
        _stateMachine.UpdateState();
    }

    /// <summary>
    /// 入力方向の計算
    /// </summary>
    private Dir InputDirection()
    {
        _inputAxis = msInput.GetMoveAxis();

        if (_inputAxis.sqrMagnitude < 0.01f)
        {
            return Dir.None;
        }

        // 基準ベクトル
        Vector2 right = Vector2.right;   // (1, 0)
        Vector2 up = Vector2.up;         // (0, 1)

        // 内積で方向を判定
        float dotRight = Vector2.Dot(_inputAxis, right); // 右向きかどうか
        float dotUp = Vector2.Dot(_inputAxis, up);       // 上向きかどうか
        float cross = _inputAxis.x * up.y - _inputAxis.y * up.x; // 外積で左右判定

        if (Mathf.Abs(dotRight) > Mathf.Abs(dotUp)) // 左右の方が強い
        {
            if (cross > 0)
                return Dir.R;
            else
                return Dir.L;
        }
        else // 上下の方が強い
        {
            if (dotUp > 0)
                return Dir.F;
            else
                return Dir.B;
        }
    }
}
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

        // 入力待ち
        InputStanby,

        // ステップ処理
        Step,
    }
    private StateMachine<State> _stateMachine = new StateMachine<State>();

    // カメラ
    private Transform _camera;

    [SerializeField, Header("入力待ち時間")]
    private float _inputTime = 0;

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
        };
        Action update = () =>
        {
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
        State state = State.None;
        Action<State> enter = (prev) =>
        {
        };
        Action update = () =>
        {
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
        Action<State> enter = (prev) =>
        {
            if (!_camera) _camera = mainMs.myCamera;
            Vector3 cameraForward = Vector3.Scale(_camera.forward, new Vector3(1, 0, 1));
            Vector3 moveFoward = cameraForward * msInput.GetMoveAxis().x + _camera.right * msInput.GetMoveAxis().y;

            // 入力方向の計算
            Vector3 perendicular = Vector3.Cross(moveFoward, transform.forward);
            float dot = Vector3.Dot(moveFoward, transform.forward);
        };
        Action update = () =>
        {
        };
        Action<State> exit = (next) =>
        {
        };
        _stateMachine.AddState(state, enter, update, exit);
    }

    #endregion
}
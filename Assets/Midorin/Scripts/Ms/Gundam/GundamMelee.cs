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
        _stateMachine.Setup(State.None);
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
    /// 移動状態のセットアップ
    /// </summary>
    private void SetUpMove()
    {
        State state = State.Move;
        GameTimer timer = new GameTimer();
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
    /// 攻撃状態のセットアップ
    /// </summary>
    private void SetUpAttack1()
    {
        State state = State.Attack1;
        GameTimer timer = new GameTimer();
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
    /// 攻撃2状態のセットアップ
    /// </summary>
    private void SetUpAttack2()
    {
        State state = State.Attack2;
        GameTimer timer = new GameTimer();
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
    /// 攻撃3状態のセットアップ
    /// </summary>
    private void SetUpAttack3()
    {
        State state = State.Attack3;
        GameTimer timer = new GameTimer();
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

    #endregion

    /// <summary>
    /// 
    /// </summary>
    public void UpdateState()
    {

    }
}
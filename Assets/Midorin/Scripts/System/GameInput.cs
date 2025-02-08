using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public enum GameInputState
{
    Jump,
    Dash,
    MainShot,
    SubShot,
    MainAttack,
    TargetChange,
    Option,
    Destory,

    Max,
}

public class GameInput
{
    private struct InputState
    {
        [Header("前回のフレームで押されていたかの有無")]
        public bool _old;

        [Header("キーが下がったかの有無")]
        public bool _down;

        [Header("キーが上がったかの有無")]
        public bool _up;

        [Header("キーが押されているかの有無")]
        public bool on;
    }

    // 移動軸
    private Vector2 _moveAxis;

    // 入力リスト
    private List<InputState> _inputStateList = new List<InputState>();

    private Gamepad _gamepad;

    /// <summary>
    /// 初期化する
    /// </summary>
    public void Initialize()
    {
        for (int i = 0; i < (int)GameInputState.Max; ++i)
        {
            InputState state = new InputState();
            _inputStateList.Add(state);
        }
    }

    /// <summary>
    /// 入力の更新
    /// 自分で設定する場合は呼び出さないで
    /// </summary>
    public void Update()
    {
        if (_gamepad == null)
        {
            _gamepad = Gamepad.current;
        }
        if (_gamepad == null)
        {
            KeyUpdate();
        }
        else
        {
            GamePadUpdate();
        }
    }

    /// <summary>
    /// キーの更新
    /// </summary>
    private void KeyUpdate()
    {
        _moveAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        SetInput(GameInputState.Jump, KeyCode.Space);
        SetInput(GameInputState.Dash, KeyCode.LeftShift);
        SetInput(GameInputState.MainShot, KeyCode.Mouse0);
        SetInput(GameInputState.SubShot, KeyCode.E);
        SetInput(GameInputState.MainAttack, KeyCode.Mouse1);
        SetInput(GameInputState.TargetChange, KeyCode.Mouse2);
        SetInput(GameInputState.Option, KeyCode.Escape);
        SetInput(GameInputState.Destory, KeyCode.P);
    }

    /// <summary>
    /// ゲームパッドの更新
    /// </summary>
    private void GamePadUpdate()
    {
        _moveAxis = _gamepad.leftStick.ReadValue();

        SetInput(GameInputState.Jump, _gamepad.crossButton.isPressed);
        SetInput(GameInputState.Dash, _gamepad.leftStickButton.isPressed);
        SetInput(GameInputState.MainShot, _gamepad.squareButton.isPressed);
        SetInput(GameInputState.SubShot, _gamepad.rightShoulder.isPressed);
        SetInput(GameInputState.MainAttack, _gamepad.triangleButton.isPressed);
        SetInput(GameInputState.TargetChange, _gamepad.circleButton.isPressed);
        SetInput(GameInputState.Option, _gamepad.selectButton.isPressed);
    }

    /// <summary>
    ///  セットキー
    /// </summary>
    /// <param name="state"></param>
    /// <param name="keyCode"></param>
    private void SetInput(GameInputState state, KeyCode keyCode)
    {
        bool now = Input.GetKey(keyCode);

        int index = (int)state;
        InputState inputState = _inputStateList[index];

        inputState.on = now;
        inputState._down = now && !inputState._old;
        inputState._up = !now && inputState._old;
        inputState._old = now;
        _inputStateList[index] = inputState;
    }

    /// <summary>
    /// boolでセットする場合
    /// </summary>
    /// <param name="state"></param>
    /// <param name="now"></param>
    public void SetInput(GameInputState state, bool now)
    {
        int index = (int)state;
        InputState inputState = _inputStateList[index];

        inputState.on = now;
        inputState._down = now && !inputState._old;
        inputState._up = !now && inputState._old;
        inputState._old = now;
        _inputStateList[index] = inputState;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="moveAxis"></param>
    public void SetMoveAxis(Vector2 moveAxis)
    {
        _moveAxis = moveAxis;
    }

    #region ゲット関数

    public Vector2 GetMoveAxis()
    {
        return _moveAxis;
    }

    /// <summary>
    /// 押し下がったか取得
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public bool GetInputDown(GameInputState state)
    {
        return _inputStateList[(int)state]._down;
    }

    /// <summary>
    /// 押しあがったか取得
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public bool GetInputUp(GameInputState state)
    {
        return _inputStateList[(int)state]._up;
    }

    /// <summary>
    /// 押されているか取得
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public bool GetInput(GameInputState state)
    {
        return _inputStateList[(int)state].on;
    }

    #endregion
}

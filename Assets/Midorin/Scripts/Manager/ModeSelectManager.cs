using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/// <summary>
/// モード選択管理クラス
/// </summary>
public class ModeSelectManager : MonoBehaviour
{
    enum State
    {
        CpuBattle,
        OnlineBattle,
        FreeBattle,
        Option,
        Exit,
    }
    private StateMachine<State> _stateMachine = new StateMachine<State>();

    [SerializeField, Header("モード切替UI")]
    private Image[] _imgModeSelect;

    [SerializeField, Header("ボタン管理オブジェクト")]
    private BtnManager[] _btnObjects;

    [SerializeField, Header("未選択カラー")]
    private Color _noSelectColor;

    private int _modeSelectIndex = 0;

    // trueならボタンが押された
    private bool _isPushBtn = false;

    [SerializeField, Header("BGNオウディオ")]
    private AudioSource _bgmAudio = null;
    [SerializeField, Header("SEオウディオ")]
    private AudioSource _seAudio = null;

    #region イベント関数

    /// <summary>
    /// Update前に呼び出される
    /// </summary>
    private void Start()
    {
        SetUp();
        ObjSwiticng();
    }

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    private void Update()
    {
        if (_isPushBtn) return;
        _stateMachine.UpdateState();
        ModeSwitching();
        BtnSwicthing();
    }

    #endregion

    #region 状態のセットアップ関数

    /// <summary>
    /// 状態のセットアップ
    /// </summary>
    private void SetUp()
    {
        CpuBattleSetUp();
        OnlineBattleSetUp();
        FreeBattleSetUp();
        OptionSetUp();
        ExitSetUp();
        _stateMachine.ChangeState(State.CpuBattle);
    }

    /// <summary>
    /// CpuBattleをセットアップ
    /// </summary>
    private void CpuBattleSetUp()
    {
        State state = State.CpuBattle;
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
    /// OnlineBattleをセットアップ
    /// </summary>
    private void OnlineBattleSetUp()
    {
        State state = State.OnlineBattle;
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
    /// FreeBattleをセットアップ
    /// </summary>
    private void FreeBattleSetUp()
    {
        State state = State.FreeBattle;
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
    /// Optionをセットアップ
    /// </summary>
    private void OptionSetUp()
    {
        State state = State.Option;
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
    /// Exitをセットアップ
    /// </summary>
    private void ExitSetUp()
    {
        State state = State.Exit;
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

    #endregion

    /// <summary>
    /// ステートが切り替わった時の処理
    /// </summary>
    private void ObjSwiticng()
    {
        // モード切替のUIを未選択カラーにする
        foreach (Image img in _imgModeSelect)
        {
            img.color = _noSelectColor;
        }
        // ボタン管理を非表示
        foreach (BtnManager obj in _btnObjects)
        {
            obj.gameObject.SetActive(false);
        }
        State state = _stateMachine.currentState;
        _imgModeSelect[(int)state].color = Color.white;
        _btnObjects[(int)state].gameObject.SetActive(true);
    }

    /// <summary>
    /// モード切替
    /// </summary>
    private void ModeSwitching()
    {
        Gamepad gamepad = Gamepad.current;
        if (gamepad == null)
        {
            Debug.Log("ゲームパッドが接続されていないよ");
            return;
        }

        // R1が押された
        if (gamepad.rightShoulder.wasPressedThisFrame)
        {
            _modeSelectIndex++;
        }
        // L1が押された
        else if (gamepad.leftShoulder.wasPressedThisFrame)
        {
            _modeSelectIndex--;
        }
        _modeSelectIndex = Mathf.Clamp(_modeSelectIndex, 0, (int)State.Exit);

        // 現在のステートと値が受ければ変更
        if ((State)_modeSelectIndex != _stateMachine.currentState)
        {
            _stateMachine.ChangeState((State)_modeSelectIndex);
            ObjSwiticng();
            _btnObjects[_modeSelectIndex].SelectReset();
        }
    }

    /// <summary>
    /// ボタン切り替え
    /// </summary>
    private void BtnSwicthing()
    {
        Gamepad gamepad = Gamepad.current;
        if (gamepad == null)
        {
            Debug.Log("ゲームパッドが接続されていないよ");
            return;
        }
        BtnManager btnManager = _btnObjects[_modeSelectIndex];
        // ×
        if(gamepad.crossButton.wasPressedThisFrame)
        {
            btnManager.SelectBtn();
            _bgmAudio.Stop();
            _seAudio.Play();
        }
        // 十字上
        else if (gamepad.dpad.up.wasPressedThisFrame)
        {
            btnManager.SelectDown();
        }
        // 十字下
        else if (gamepad.dpad.down.wasPressedThisFrame)
        {
            btnManager.SelectAdd();
        }

    }
}

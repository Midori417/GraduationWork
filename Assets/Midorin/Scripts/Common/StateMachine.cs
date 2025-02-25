using System;
using System.Collections.Generic;
using System.Diagnostics;

// ステートマシンを実装するクラス
// Enum型限定テンプレート
[Serializable]
public class StateMachine<T> where T : Enum
{
    // 各状態を表現するクラス
    // Action デリゲートの一種(デリゲートとは関数を変数のように使えるもの?)↵
    // 戻りを持たないvoid型 ↵
    // Genericで引数の方を指定できる ↵
    // パラメータは最大16個 ↵
    private class State
    {
        // 状態の値(enum)
        public T Value;

        // 状態に入るときの処理
        public Action<T> Enter;

        // 状態中に毎フレーム呼び出される処理
        public Action Update;

        // 状態を離れるときの所為
        public Action<T> Exit;

        public Action LateUpdate;
    }

    // 現在の状態を外部で取得
    public T currentState => _currentState;

    // Dictionary C++のstd::mapみたいなもの
    private Dictionary<T, State> _dict = new Dictionary<T, State>()
    {
        [default(T)] = new State()
        {
            Value = default(T),
            Enter = delegate { },
            Update = delegate { },
            Exit = delegate { },
            LateUpdate = delegate { },
        },
    };
    private T _currentState;

    /// <summary>
    /// 最初の状態を設定して
    /// その状態のEnterを実行する
    /// </summary>
    /// <param name="first">設定する状態</param>
    public void SetUp(T first)
    {
        _currentState = first;
        _dict[_currentState].Enter(first);
    }

    /// <summary>
    /// 新しい状態を設定する
    /// </summary>
    /// <param name="value">状態の識別子(enum)</param>
    /// <param name="enter">状態開始時の処理</param>
    /// <param name="update">状態更新ジ時の処理</param>
    /// <param name="exit">状態終了時の処理</param>
    public void AddState(T value, Action<T> enter, Action update, Action lateUpdate, Action<T> exit)
    {
        var state = new State()
        {
            Value = value,
            Enter = enter,
            Update = update,
            LateUpdate = lateUpdate,
            Exit = exit,
        }; 
        _dict[value] = state;
    }

    /// <summary>
    /// 新しい状態を設定する
    /// </summary>
    /// <param name="value">状態の識別子(enum)</param>
    /// <param name="enter">状態開始時の処理</param>
    /// <param name="update">状態更新ジ時の処理</param>
    /// <param name="exit">状態終了時の処理</param>
    public void AddState(T value, Action<T> enter, Action update, Action<T> exit)
    {
        var state = new State()
        {
            Value = value,
            Enter = enter,
            Update = update,
            LateUpdate = delegate { },
            Exit = exit,
        };
        _dict[value] = state;
    }


    /// <summary>
    /// 現在の状態のUpdate処理を実行する
    /// </summary>
    public void UpdateState()
    {
        _dict[_currentState].Update();
    }

    /// <summary>
    /// 現在の状態のLateUpdate処理を実行する
    /// </summary>
    public void LateUpdateState()
    {
        if(_dict[_currentState].LateUpdate == null)
        {
            return;
        }
        _dict[_currentState].LateUpdate();
    }

    /// <summary>
    /// 現在の状態を変更する
    /// 変更前の状態のExit処理を実行する
    /// 変更後の状態のEnter処理を実行する
    /// </summary>
    /// <param name="next">次の状態の識別子</param>
    public void ChangeState(T next)
    {
        if(!_dict.ContainsKey(next))
        {
            // 登録されていないKeyが呼び出されたとき
            return;
        }

        var prev = _currentState;
        _dict[_currentState].Exit(next);
        _currentState = next;
        _dict[_currentState].Enter(prev);
    }
}

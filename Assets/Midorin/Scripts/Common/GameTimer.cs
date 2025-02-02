using System;
using UnityEngine;

// タイマークラス
[Serializable]
public class GameTimer
{
    #region ゲッター

    // 最大時間
    public float max => _maxTime;

    // 現在の時間
    public float current => _currentTime;

    // 残り時間
    public float remain => _maxTime - _currentTime;

    // 経過時間の割合
    public float rate => _currentTime / _maxTime;

    // 残り時間の割合
    public float inverseRate => remain / _maxTime;

    // タイマーが終了しているか
    public bool isEnd => _currentTime >= _maxTime;

    #endregion

    [SerializeField, Header("最大時間")]
    private float _maxTime;

    // 現在の時間
    private float _currentTime;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public GameTimer()
    {
        _maxTime = 1.0f;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="time">最大時間</param>
    /// <param name="isEnd">trueなら現在時刻に最大時間を代入</param>
    public GameTimer(float time, bool isEnd = false)
    {
        _maxTime = time;
        if (isEnd)
        {
            _currentTime = _maxTime;
        }
    }

    /// <summary>
    /// タイマーを更新
    /// </summary>
    /// <param name="scale">時間の進む速さ</param>
    /// <returns>
    /// true    タイマー終了
    /// flase   タイマー継続
    /// </returns>
    public bool UpdateTimer(float scale = 1.0f)
    {
        _currentTime += Time.deltaTime * scale;
        if (isEnd)
        {
            _currentTime = _maxTime;
            return true;
        }
        return false;
    }

    /// <summary>
    /// タイマーをリセット
    /// </summary>
    public void ResetTimer()
    {
        _currentTime = 0.0f;
    }

    /// <summary>
    /// タイマーのリセットと最大時間を再設定
    /// </summary>
    /// <param name="time">最大時間</param>
    public void ResetTimer(float time)
    {
        _maxTime = time;
        _currentTime = 0.0f;
    }

    /// <summary>
    /// タイマーの強制終了
    /// </summary>
    public void ForceEnd()
    {
        _currentTime = _maxTime;
    }

    /// <summary>
    /// タイマーを強制終了させて
    /// 最大時間を再設定
    /// </summary>
    /// <param name="time">最大時間</param>
    public void ForceEnd(float time)
    {
        _maxTime = time;
        _currentTime = _maxTime;
    }
}
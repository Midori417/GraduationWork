using UnityEngine;

/// <summary>
/// ゲームに登場するゲームオブジェクト
/// </summary>
public class BaseGameObject : MonoBehaviour
{
    // trueなら行動を止める
    private bool _isStop = false;

    public bool isStop => _isStop;

    /// <summary>
    /// オブジェクトの処理を止める
    /// </summary>
    public virtual void Stop()
    {
        _isStop = true;
    }

    /// <summary>
    /// オブジェクトの処理を開始
    /// </summary>
    public virtual void Play()
    {
        _isStop = false;
    }
}

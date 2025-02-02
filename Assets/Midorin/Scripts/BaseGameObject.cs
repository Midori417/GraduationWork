using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームに登場するゲームオブジェクト
/// </summary>
public class BaseGameObject : MonoBehaviour
{
    private bool _isStop = false;

    [HideInInspector]
    public bool isStop => _isStop;

    /// <summary>
    /// オブジェクトの処理を止める
    /// </summary>
    public virtual void Stop()
    {
        _isStop = false;
    }

    /// <summary>
    /// オブジェクトの処理を開始
    /// </summary>
    public virtual void Play()
    {
        _isStop = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 機体パーツのベースコンポーネント
/// </summary>
public class BaseMsParts : MonoBehaviour
{
    // メイン機体コンポーネント
    private Gundam _mainMs;

    private Rigidbody _rb;
    private Animator _animator;

    // メイン機体コンポーネント
    protected Gundam mainMs
    { get { return _mainMs; } }

    protected Rigidbody rb
    { get { return _rb; } }
    protected Animator Animator 
    { get { return _animator; } }

    protected Transform targetMs
    { 
        get
        {
            if(!_mainMs)
            {
                Debug.LogError("メイン機体が不明");
                return null;
            }
            if(!_mainMs.targetMs)
            {
                Debug.LogError("ターゲット機体が不明");
                return null;
            }
            return _mainMs.targetMs.transform;
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns>
    /// true    初期化成功
    /// faklse  初期化失敗
    /// </returns>
    public virtual bool Initalize() 
    {
        _mainMs = GetComponent<Gundam>();
        if (!mainMs)
        {
            _mainMs = GetComponentInParent<Gundam>();

            if (!mainMs)
            {
                Debug.LogError("メインコンポーネントが取得できません");
                return false;
            }
        }
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        return true;
    }
}

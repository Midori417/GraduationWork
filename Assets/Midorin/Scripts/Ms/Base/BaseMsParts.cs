using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 機体パーツのベースコンポーネント
/// </summary>
public class BaseMsParts : MonoBehaviour
{
    // メイン機体コンポーネント
    private BaseMs _mainMs;

    protected BaseMs mainMs => _mainMs;
    protected Rigidbody rb
    {
        get 
        {
            if(mainMs)
            {
                return mainMs.rb;
            }

            return null;
        }
    }
    protected Animator animator
    {
        get
        {
            if(mainMs)
            {
                return mainMs.animator;
            }
            return null;
        }
    }
    protected Transform targetMs
    {
        get
        {
            if(mainMs)
            {
                if(mainMs.targetMs)
                {
                    return mainMs.targetMs.center;
                }
            }
            return null;
        }
    }
    protected bool isStop => _mainMs.isStop;
    protected MsInput msInput => _mainMs.msInput;
    protected GroundCheck groundCheck => _mainMs.groundCheck;

    /// <summary>
    /// 初期化
    /// </summary>
    public virtual void Initalize()
    {
    }

    /// <summary>
    /// メインコンポーネントを設定
    /// </summary>
    /// <param name="mainMs"></param>
    public void SetMainMs(BaseMs mainMs)
    {
        _mainMs = mainMs;
    }
}

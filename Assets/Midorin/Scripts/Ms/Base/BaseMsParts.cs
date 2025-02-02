using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 機体パーツのベースコンポーネント
/// </summary>
public class BaseMsParts : BaseGameObject
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
    protected Transform targetMs
    {
        get
        {
            if(mainMs)
            {
                if(mainMs.targetMs)
                {
                    return mainMs.targetMs.transform;
                }
            }
            return null;
        }
    }

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

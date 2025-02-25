using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武装基底コンポーネントの
/// </summary>
public class BaseArmed : BaseGameObject
{
    /// <summary>
    /// 生成時に呼び出す
    /// </summary>
    private void Awake()
    {
        ArmedManager i = ArmedManager.I;
        if (i)
        {
            i.AddArmed(this);
        }
    }

    /// <summary>
    /// 破壊時に呼び出す
    /// </summary>
    private void OnDestroy()
    {
        ArmedManager i = ArmedManager.I;
        if (i)
        {
            i.RemoveArmed(this);
        }
    }
}

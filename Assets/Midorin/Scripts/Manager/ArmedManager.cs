using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武装管理マネージャー
/// </summary>
public class ArmedManager : SingletonBehavior<ArmedManager>
{
    //武装リスト
    private List<BaseArmed> _armedList = new List<BaseArmed>(100);

    /// <summary>
    /// 武装の追加
    /// </summary>
    /// <param name="bullet"></param>
    public void AddArmed(BaseArmed bullet)
    {
        _armedList.Add(bullet);
    }

    /// <summary>
    /// 武装の削除
    /// </summary>
    /// <param name="bullet"></param>
    public void RemoveArmed(BaseArmed bullet)
    {
        _armedList.Remove(bullet);
    }

    /// <summary>
    /// 処理の再生
    /// </summary>
    public void Play()
    {
        foreach (BasicBulletMove armed in _armedList)
        {
            armed.Play();
        }
    }

    /// <summary>
    /// 処理の停止
    /// </summary>
    public void Stop()
    {
        foreach (BasicBulletMove armed in _armedList)
        {
            armed.Stop();
        }
    }
}

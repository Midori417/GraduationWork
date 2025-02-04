using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダメージ検知
/// </summary>
public class MsDamageCollision : BaseMsParts
{
    /// <summary>
    /// 初期化
    /// </summary>
    public override void Initalize()
    {
        base.Initalize();
    }

    /// <summary>
    /// ダメージ
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="downValue"></param>
    /// <param name="bulletPos"></param>
    public bool Damage(int damage, int downValue, Vector3 bulletPos)
    {
        if (mainMs)
        {
            return mainMs.Damage(damage, downValue, bulletPos);
        }
        return false;
    }
}

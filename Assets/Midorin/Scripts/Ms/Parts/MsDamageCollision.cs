using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダメージ検知
/// </summary>
public class MsDamageCollision : BaseMsParts
{

    public Team team
    {
        get
        {
            if(mainMs)
            {
                return mainMs.team;
            }
            return Team.None;
        }
    }

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
    public bool Damage(int damage, float downValue, Vector3 bulletPos, float hitStop = 0)
    {
        if (mainMs)
        {
            return mainMs.Damage(damage, downValue, bulletPos, hitStop);
        }
        return false;
    }
}

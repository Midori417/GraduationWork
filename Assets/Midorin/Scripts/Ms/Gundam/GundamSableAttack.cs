using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 近接攻撃
/// </summary>
public class GundamSableAttack : BaseMsParts
{

    // アニメータータ
    private Animator animator;

    private void Update()
    {
    }

    public override bool Initalize()
    {
        if (!base.Initalize())
        {
            return false;
        }
        if (!animator) animator = GetComponent<Animator>();

        return true;
    }

}

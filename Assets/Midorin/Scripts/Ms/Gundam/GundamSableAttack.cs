using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ߐڍU��
/// </summary>
public class GundamSableAttack : BaseMsParts
{

    // �A�j���[�^�[�^
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

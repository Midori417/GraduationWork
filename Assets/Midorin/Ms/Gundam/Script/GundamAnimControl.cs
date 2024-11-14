using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �K���_���A�j���[�V�����R���g�[���R���|�[�l���g
/// </summary>
public class GundamAnimControl : MonoBehaviour
{
    private Animator anim;

    /// <summary>
    /// �n��A�j���[�V�����̐ݒ�
    /// </summary>
    /// <param name="_speed"></param>
    public void SetMove(bool isMove)
    {
        if (anim)
        {
            anim.SetBool("Move", isMove);
        }
    }

    /// <summary>
    /// �_�b�V����ݒ�
    /// </summary>
    /// <param name="isDash"></param>
    public void SetDash(bool isDash)
    {
        if(anim)
        {
            anim.SetBool("Dash", isDash);
        }
    }

    /// <summary>
    /// �W�����v��ݒ�
    /// </summary>
    /// <param name="isJump"></param>
    public void SetJump(bool isJump)
    {
        if(anim)
        {
            anim.SetBool("Jump", isJump);
        }
    }

    /// <summary>
    /// ���n�A�j���[�V����
    /// </summary>
    public void SetGround()
    {
        if (anim)
        {
            anim.SetTrigger("Ground");
        }
    }
}
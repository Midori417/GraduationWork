using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ガンダムアニメーションコントールコンポーネント
/// </summary>
public class GundamAnimControl : MonoBehaviour
{
    private Animator anim;

    /// <summary>
    /// 地上アニメーションの設定
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
    /// ダッシュを設定
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
    /// ジャンプを設定
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
    /// 着地アニメーション
    /// </summary>
    public void SetGround()
    {
        if (anim)
        {
            anim.SetTrigger("Ground");
        }
    }
}
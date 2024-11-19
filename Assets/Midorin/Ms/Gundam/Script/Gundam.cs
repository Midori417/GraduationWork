using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ガンダム
/// </summary>
public class Gundam : BaseMs
{
    [SerializeField, Header("移動機能")]
    private GundomMove move;

    /// <summary>
    /// Updateの前に実行
    /// </summary>
    private void Start()
    {
        // 必要なコンポーネントを取得する
        // コンポーネントが足りなければスクリプトを停止する
        enabled = GetGundamComponent();
        if (!enabled)
        {
            return;
        }

        Initialize();
    }

    /// <summary>
    /// 毎フレーム実行
    /// </summary>
    private void Update()
    {
        if (!move)
        {
            return;
        }
        BoostGaugeChage();
        UseGravity();

        // 着地処理
        if (!olsIsGround && isGround)
        {
            move.Landing();
        }

        // 地面についている場合
        if (isGround)
        {
            move.Move(pilotInput.moveAxis);
        }
        move.Dash(pilotInput.moveAxis, pilotInput.isDashBtn);
        move.Jump(pilotInput.moveAxis, pilotInput.isJumpBtn);

        AnimationUpdate();
    }

    /// <summary>
    /// ガンダムに必要なコンポーネントを取得する
    /// </summary>
    /// <returns></returns>
    private bool GetGundamComponent()
    {
        if (!GetBaseMsComponent())
        {
            return false;
        }
        move = GetComponent<GundomMove>();
        if (!move)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// ガンダムの機体パラメータの初期化
    /// </summary>
    private void Initialize()
    {
        BoostGaugeInit();
        move.Initalize();
    }

    /// <summary>
    /// 重力が必要か
    /// </summary>
    void UseGravity()
    {
        if (move.isDash || move.isJump)
        {
            rb.useGravity = false;
        }
        else
        {
            rb.useGravity = true;
        }
    }

    /// <summary>
    /// アニメーションの状態を更新
    /// </summary>
    void AnimationUpdate()
    {
        if (!anim)
        {
            return;
        }
        anim.SetBool("Move", move.isMove);
        anim.SetBool("Dash", move.isDash);
        anim.SetBool("Jump", move.isJump);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ガンダム
/// </summary>
public class Gundam : BaseMs
{
    [SerializeField, Header("true動きを止める")]
    private bool isStop = false;

    [SerializeField, Header("移動コンポーネント")]
    private MsMove move;

    // 仮入力
    Vector2 moveAxis;
    bool isJumpBtn;
    bool isDashBtn;

    // ビームライフル攻撃レイヤーインデックス
    int beumRifleLayerIndex = 0;

    #region イベント

    /// <summary>
    /// Updateより前に実行する
    /// </summary>
    private void Start()
    {
        Initialize();
    }

    /// <summary>
    /// 毎フレーム実行
    /// </summary>
    private void Update()
    {
        if (!ComponentCheck())
        {
            Debug.LogError("必要なコンポーネント足りません");
            return;
        }

        // 仮キー入力
        moveAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        isJumpBtn = Input.GetKey(KeyCode.Space);
        isDashBtn = Input.GetKey(KeyCode.LeftShift);

        if(Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("BeumRifleShot");
            animator.SetLayerWeight(beumRifleLayerIndex, 1);
        }

        BoostCharge();

        if (!isStop)
        {
            Move();
        }

        AnimUpdate();
    }

    /// <summary>
    ///  Updateより後に実行
    /// </summary>
    private void LateUpdate()
    {

    }

    #endregion

    /// <summary>
    /// 初期化する
    /// </summary>
    protected override void Initialize()
    {
        base.Initialize();

        ComponentCheck();

        rb.drag = 0.1f;
        move.Initalize();

        beumRifleLayerIndex = animator.GetLayerIndex("BeumRifleLayer");
    }

    /// <summary>
    /// 必要なコンポーネントがあるか
    /// </summary>
    /// <returns></returns>
    protected override bool ComponentCheck()
    {
        if(!base.ComponentCheck()) return false;
        if (!move) return false;

        return true;
    }

    /// <summary>
    /// 動きを止める
    /// </summary>
    public void Stop()
    {
        isStop = true;
    }

    /// <summary>
    /// 動きを再開
    /// </summary>
    public void Go()
    {
        isStop = false;
    }

    /// <summary>
    /// 着地処理
    /// </summary>
    void Landing()
    {
        move.Landing();
        animator.SetTrigger("Landing");
        Stop();
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    void Move()
    {
        // 地面ついているときのみ
        if (groundCheck.isGround)
        {
            move.Move(moveAxis);
        }

        if (!move.isDash)
        {
            move.Jump(moveAxis, isJumpBtn);
        }

        // ダッシュが終わったとき地面についていたら着地する
        if(move.isDash && !isDashBtn && groundCheck.isGround)
        {
            Landing();
        }

        move.Dash(moveAxis, isDashBtn);

        // 着地した瞬間
        if (groundCheck.isGround && !groundCheck.oldIsGround)
        {
            Landing();
        }

        // ダッシュとジャンプ中は重力の影響を受けない
        rb.useGravity = (!move.isDash) && (!move.isJump);
    }

    /// <summary>
    /// アニメータに伝える変数の更新
    /// </summary>
    void AnimUpdate()
    {
        // アニメータ変数処理
        float speed = new Vector2(rb.velocity.x, rb.velocity.z).magnitude;
        animator.SetFloat("Speed", speed);
        animator.SetBool("IsGround", groundCheck.isGround);
        animator.SetBool("Jump", move.isJump);
        animator.SetBool("Dash", move.isDash);
    }

    /// <summary>
    /// ビームライフル攻撃が終わった時の処理
    /// </summary>
    public void BeumRifleShotFailed()
    {
        Debug.Log("A");
        animator.SetLayerWeight(beumRifleLayerIndex, 0);
    }
}

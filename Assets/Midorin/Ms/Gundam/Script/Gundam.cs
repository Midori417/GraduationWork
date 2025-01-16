using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ガンダム
/// </summary>
public class Gundam : BaseMs
{
    private bool isStopMove = false;

    // trueなら立ち上がり可能
    private bool isStandingOk = false;

    [SerializeField, Header("着地してから立ち上がり可能時間")]
    private float standingOkTime = 0;

    [SerializeField, Header("着地してから自動的に立ち上がる時間")]
    private float autoStandingTime = 5;

    [SerializeField, Header("攻撃食らってからの無敵時間")]
    private float invincibleTime;

    // trueならダメージを受ける
    private bool isDamageOk = true;

    [SerializeField, Header("頭の処理")]
    private LockHead lockHead;

    [SerializeField, Header("移動コンポーネント")]
    private MsMove move;

    [SerializeField, Header("ビームライフルコンポーネント")]
    private GundamRifleShot rifleShot;

    [SerializeField, Header("バズーカコンポーネント")]
    private GundamBazookaShot bazookaShot;

    [SerializeField]
    private MsDamageCheck msDamageCheck;

    [SerializeField, Header("ビームライフルオブジェクト")]
    private GameObject beumRifle;

    [SerializeField, Header("バーニアエフェクト")]
    private ParticleSystem eff_roketFire;

    // ビームライフル攻撃レイヤーインデックス
    int beumRifleLayerIndex = 0;

    #region イベント

    /// <summary>
    /// 毎フレーム実行
    /// </summary>
    private void Update()
    {
        if (DestroyCheck())
        {
            // 破壊された
            return;
        }

        BoostCharge();

        if (!isStopMove)
        {
            MoveProsess();
            BeumRifleProcess();
            BazookaProsess();
        }
        else
        {
            rb.useGravity = true;
        }

        DownProsess();
        RoketEffControl();
        AnimationProsess();
    }

    #endregion

    /// <summary>
    /// 初期化する
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();
        ProsessCheck();

        if (msDamageCheck)
            msDamageCheck.Initalize();

        // レイヤー番号を取得
        beumRifleLayerIndex = animator.GetLayerIndex("BeumRifleLayer");
    }

    /// <summary>
    ///処理に必要なものがそろっているかチェック
    /// </summary>
    /// <returns>
    /// false そろっている
    /// true そろっていない
    /// </returns>
    protected override bool ProsessCheck()
    {
        // 必ず必要なもの
        if (!base.ProsessCheck()) return true;
        if (!move)
        {
            Debug.LogError("移動コンポーネントが存在しません");
            return true;
        }
        else move.Initalize();

        // 存在しなくても大丈夫なもの
        if (!lockHead) Debug.LogWarning("LockHead存在しません");
        else lockHead.Initalize();
        if (!rifleShot) Debug.LogWarning("RifleShot存在しません");
        else rifleShot.Initalize();
        if (!bazookaShot) Debug.LogWarning("Bazooka存在しません");
        else bazookaShot.Initalize();

        return false;
    }

    /// <summary>
    /// アニメーションの処理
    /// </summary>
    void AnimationProsess()
    {
        // アニメータ変数処理
        float speed = new Vector2(rb.velocity.x, rb.velocity.z).magnitude;
        animator.SetFloat("Speed", speed);
        animator.SetBool("IsGround", groundCheck.isGround);
        animator.SetBool("Jump", move.isJump);
        animator.SetBool("Dash", move.isDash);
    }

    /// <summary>
    /// ロケットエフェクトコントロール
    /// </summary>
    void RoketEffControl()
    {
        if (!eff_roketFire)
        {
            return;
        }

        if (move.isDash || move.isJump)
        {
            eff_roketFire.Play();
        }
        else
        {
            eff_roketFire.Stop();
        }
    }

    #region ダメージ

    /// <summary>
    /// ダメージを与える
    /// </summary>
    /// <param name="damage"></param>
    public override void Damage(int damage, int _downValue, Vector3 bulletPos)
    {
        if (!isDamageOk)
            return;
        base.Damage(damage, _downValue, bulletPos);

        Vector3 directionToTarget = Vector3.Scale(transform.position - bulletPos, new Vector3(1, 0, 1));
        float dot = Vector3.Dot(directionToTarget.normalized, transform.forward);
        rb.velocity = Vector3.zero;
        if (dot > 0)
        {
            // 背面から
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = targetRotation;
            animator.SetInteger("DamageValue", 2);
        }
        else
        {
            // 正面からくらった
            Vector3 reverseDirectionToTarget = directionToTarget * -1.0f;
            Quaternion targetRotation = Quaternion.LookRotation(reverseDirectionToTarget);
            transform.rotation = targetRotation;
            animator.SetInteger("DamageValue", 1);
        }
        // ダウン値が5いかならよろけ
        if (downValue < 5)
        {
            rb.AddForce(directionToTarget * 10.0f, ForceMode.Impulse);
            animator.SetTrigger("Damage");
            Stop();
        }
        else
        {
            rb.AddForce(directionToTarget * 20.0f, ForceMode.Impulse);
            rb.AddForce(Vector3.up * 20.0f, ForceMode.Impulse);
            animator.SetTrigger("Down");
            downValue = 0;
            isDown = true;
            isDamageOk = false;
            Stop();
        }
    }

    /// <summary>
    /// ダメージアニメーションの終了
    /// </summary>
    void DamageFailde()
    {
        animator.SetInteger("DamageValue", 0);
        Go();
    }

    /// <summary>
    /// ダウン中
    /// </summary>
    void DownProsess()
    {
        if (isDown)
        {
            if (groundCheck)
            {
                Invoke("StandingOk", standingOkTime);
                Invoke("StandingProsess", autoStandingTime);
            }
            StandingProsess();
        }
    }

    /// <summary>
    /// 立ち上がり可能
    /// </summary>
    void StandingOk()
    {
        isStandingOk = true;
    }

    /// <summary>
    /// 立ち上がり処理
    /// </summary>
    void StandingProsess()
    {
        if (isStandingOk)
        {
            if (moveAxis != Vector2.zero)
            {
                Invoke("InvincibleRemoved", invincibleTime);
                animator.SetTrigger("Standing");
                isDown = false;
                isStandingOk = false;
                Go();
            }
        }
    }

    /// <summary>
    /// 無敵解除
    /// </summary>
    void InvincibleRemoved()
    {
        isDamageOk = true;
    }

    #endregion

    #region 行動のコントロール

    /// <summary>
    /// 動きを止める
    /// </summary>
    public void Stop()
    {
        isStopMove = true;
    }

    /// <summary>
    /// 動きを再開
    /// </summary>
    public void Go()
    {
        isStopMove = false;
    }
    #endregion

    #region 移動

    /// <summary>
    /// 移動処理
    /// </summary>
    void MoveProsess()
    {
        // 地面移動処理
        if (groundCheck.isGround) move.GroundMove(moveAxis);

        // ジャンプ処理
        if (!move.isDash) move.Jump(moveAxis, isJumpBtn);

        // ダッシュが終わったとき地面についていたら着地処理をする
        if (move.isDash && !isDashBtn && groundCheck.isGround) Landing();

        // ダッシュ処理
        move.Dash(moveAxis, isDashBtn);

        // 着地処理
        if (groundCheck.isGround && !groundCheck.oldIsGround) Landing();

        // ダッシュとジャンプ中は重力の影響を受けない
        rb.useGravity = (!move.isDash) && (!move.isJump);
    }

    /// <summary>
    /// 着地処理
    /// </summary>
    void Landing()
    {
        move.Landing();
        animator.SetTrigger("Landing");
        Stop();
        Invoke("Go", 1);
    }

    #endregion

    #region ビームライフル

    /// <summary>
    /// ビームライフル処理
    /// </summary>
    private void BeumRifleProcess()
    {
        // ライフル射撃機能が存在しないので処理を行わない
        if (!rifleShot) return;

        // 射撃入力あれば処理をする
        if (isMainShotBtn)
        {
            // falseなら射撃不可
            if (!BeumRifleShotCheck()) return;

            if (!rifleShot.isBackShot)
            {
                animator.SetTrigger("BeumRifleShot");
                animator.SetLayerWeight(beumRifleLayerIndex, 1);
            }
            else animator.SetTrigger("BeumRifleShotBack");

            Invoke("BeumRifleShotFailed", 0.8f);
        }
    }

    /// <summary>
    /// 射撃が可能かチェック
    /// </summary>
    /// <returns>
    /// true 射撃可能
    /// false 射撃不可
    /// </returns>
    private bool BeumRifleShotCheck()
    {
        if (bazookaShot.isNow) return false;
        if (!rifleShot.ShotCheck()) return false;

        return true;
    }

    /// <summary>
    /// ビームライフルの弾を生成
    /// アニメーションイベントで呼び出す
    /// </summary>
    private void BeumRifleCreateBullet()
    {
        rifleShot.CreateBullet();
    }

    /// <summary>
    /// ビームライフル攻撃が終わった時の処理
    /// </summary>
    private void BeumRifleShotFailed()
    {
        rifleShot.Failed();
        animator.SetLayerWeight(beumRifleLayerIndex, 0);
    }

    #endregion

    #region バズーカ

    /// <summary>
    /// バズーカ処理
    /// </summary>
    private void BazookaProsess()
    {
        // バズーカ射撃機能が存在しないので処理を行わない
        if (!bazookaShot) return;

        if (isSubShotBtn)
        {
            if (!bazookaShot.ShotCheck())
            {
                return;
            }

            animator.SetTrigger("BazookaShot");
            Invoke("BazookaFailed", 0.8f);
        }
    }

    /// <summary>
    /// バズーカの弾を生成
    /// アニメーションイベントで呼び出す
    /// </summary>
    private void BazookaCreateBullet()
    {
        bazookaShot.CreateBullet();
    }

    /// <summary>
    /// バズーカの終了処理
    /// </summary>
    private void BazookaFailed()
    {
        bazookaShot.Failed();
    }

    #endregion
}

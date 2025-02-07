using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ガンダム
/// </summary>
public class Gundam : BaseMs
{
    // trueなら動きを止める
    private bool isStopMove = false;

    [SerializeField, Header("頭の処理")]
    private LockHead lockHead;

    [SerializeField, Header("移動コンポーネント")]
    private MsMove move;

    [SerializeField, Header("ビームライフルコンポーネント")]
    private GundamRifleShot rifleShot;

    [SerializeField, Header("バズーカコンポーネント")]
    private GundamBazookaShot bazookaShot;

    [SerializeField, Header("ダメージ検出コンポーネント")]
    private MsDamageCheck msDamageCheck;

    [SerializeField, Header("ビームライフルオブジェクト")]
    private GameObject beumRifle;

    [SerializeField, Header("バーニアエフェクト")]
    private GameObject eff_roketFire;

    [SerializeField, Header("バーニア位置")]
    private Transform rokeFireTrs;

    private GameObject roketFire;

    // trueならダメージを受ける
    private bool isDamageOk = true;

    // trueなら立ち上がること可能
    private bool isStandingOk = false;

    [SerializeField, Header("ダウン着地してから立ち上がり可能になるまでの時間")]
    private float standingTime = 0;
    private float standingTimer = 0;

    [SerializeField, Header("無敵時間")]
    private float invincibleTime = 0;

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
            //return;
        }

        BoostCharge();

        if (!isStopMove && !isDown)
        {
            MoveProsess();
            BeumRifleProcess();
            BazookaProsess();
        }

        DownProsess();
        StandingProsess();

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

        // レイヤー番号を取得
        beumRifleLayerIndex = animator.GetLayerIndex("BeumRifleLayer");
    }

    /// <summary>
    ///処理に必要なものがそろっているかチェック
    /// </summary>
    protected override void ProsessCheck()
    {
        // 必ず必要なもの
        base.ProsessCheck();

        if (!move)
        {
            Debug.LogError("移動コンポーネントが存在しません");
            return;
        }
        else move.Initalize();

        // 存在しなくても大丈夫なもの
        if (!lockHead) Debug.LogWarning("LockHead存在しません");
        else lockHead.Initalize();
        if (!rifleShot) Debug.LogWarning("RifleShot存在しません");
        else rifleShot.Initalize();
        if (!bazookaShot) Debug.LogWarning("Bazooka存在しません");
        else bazookaShot.Initalize();
        if (!msDamageCheck) Debug.LogWarning("MsDamageChackが存在しません");
        else msDamageCheck.Initalize();
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
        if (!isDown)
        {
            animator.SetBool("Jump", move.isJump);
            animator.SetBool("Dash", move.isDash);
        }
        else
        {
            animator.SetBool("Jump", false);
            animator.SetBool("Dash", false);
        }
    }

    /// <summary>
    /// ロケットエフェクトコントロール
    /// </summary>
    void RoketEffControl()
    {
        if (!eff_roketFire || !rokeFireTrs) return;

        if (move.isDash || move.isJump)
        {
            if (!roketFire)
            {
                roketFire = Instantiate(eff_roketFire, rokeFireTrs);
            }
        }
        else
        {
            if (roketFire)
            {
                Destroy(roketFire);
            }
        }

        // ダウン中は止める
        if (isDown)
        {
            if (roketFire)
            {
                Destroy(roketFire);
            }
        }
    }

    #region ダメージ

    /// <summary>
    /// ダメージを与える
    /// </summary>
    /// <param name="damage"></param>
    public override void Damage(int damage, int _downValue, Vector3 bulletPos)
    {
        // falseならダメージ処理を行わない
        if (!isDamageOk) return;

        base.Damage(damage, _downValue, bulletPos);

        Vector3 directionToTarget = Vector3.Scale(transform.position - bulletPos, new Vector3(1, 0, 1));
        float dot = Vector3.Dot(directionToTarget.normalized, transform.forward);
        if (dot > 0)
        {
            // 背面から
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = targetRotation;
            animator.SetInteger("DamageDirection", 2);
        }
        else
        {
            // 正面からくらった
            Vector3 reverseDirectionToTarget = directionToTarget * -1.0f;
            Quaternion targetRotation = Quaternion.LookRotation(reverseDirectionToTarget);
            transform.rotation = targetRotation;
            animator.SetInteger("DamageDirection", 1);
        }
        // ダウン値が5以上ならダウン状態
        if (downValue < 5)
        {
            // 後ろに後退
            rb.AddForce(directionToTarget * 10.0f, ForceMode.Impulse);
            animator.SetTrigger("Damage");
            Stop();
        }
        else
        {
            rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
            rb.AddForce(directionToTarget * 20, ForceMode.Impulse);
            Stop();
            rb.useGravity = true;
            animator.SetTrigger("Down");
            // 無敵化
            isDamageOk = false;
            isDown = true;
            standingTimer = standingTime;
        }
    }

    /// <summary>
    /// ダウン状態処理
    /// </summary>
    private void DownProsess()
    {
        // ダウン状態でなければ処理しない
        if (!isDown) return;

        // 着地してから一定時間たてば立ち上がり可能
        if (groundCheck.isGround)
        {
            standingTimer -= Time.deltaTime;
            if(standingTimer <= 0)
            {
                isStandingOk = true;
            }
        }
    }

    /// <summary>
    /// 立ち上がり処理
    /// </summary>
    private void StandingProsess()
    {
        // 立ち上がり可能じゃなければ処理をしない
        if (!isStandingOk) return;

        // 移動入力があれば立ち上がる
        if (moveAxis != Vector2.zero)
        {
            animator.SetTrigger("Standing");
            Invoke("RemoveInvincible", invincibleTime);
            isDown = false;
            isStandingOk = false;
        }
    }

    /// <summary>
    /// 無敵解除
    /// </summary>
    private void RemoveInvincible()
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
        rb.velocity = Vector3.zero;
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
        Stop();
        animator.SetTrigger("Landing");
        move.Landing();
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

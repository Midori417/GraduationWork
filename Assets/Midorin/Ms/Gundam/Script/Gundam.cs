using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ガンダム
/// </summary>
public class Gundam : BaseMs
{
    [SerializeField, Header("true動きを止める")]
    private bool isStop = false;

    [SerializeField, Header("頭の処理")]
    private LockHead lockHead;

    [SerializeField, Header("移動コンポーネント")]
    private MsMove move;

    [SerializeField, Header("ビームライフルコンポーネント")]
    private GundamRifleShot rifleShot;

    [SerializeField, Header("バズーカコンポーネント")]
    private GundamBazookaShot bazookaShot;

    [SerializeField, Header("ビームライフルオブジェクト")]
    private GameObject beumRifle;

    [SerializeField, Header("バズーカオブジェクト")]
    private GameObject bazooka;

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
        if (!ComponentCheck())
        {
            Debug.LogError("必要なコンポーネント足りません");
            return;
        }

        BoostCharge();

        if (!isStop)
        {
            MoveProsess();
            BeumRifleProcess();
            BazookaProsess();
        }

        RoketFireControl();
        AnimUpdate();
    }

    #endregion

    /// <summary>
    /// 初期化する
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();
        ComponentCheck();

        lockHead.Initalize();
        move.Initalize();
        rifleShot.Initalize();
        bazookaShot.Initalize();

        // レイヤー番号を取得
        beumRifleLayerIndex = animator.GetLayerIndex("BeumRifleLayer");
        bazooka.SetActive(false);
    }

    /// <summary>
    /// 必要なコンポーネントがあるか
    /// </summary>
    /// <returns></returns>
    protected override bool ComponentCheck()
    {
        if (!base.ComponentCheck()) return false;
        if (!lockHead) return false;
        if (!move) return false;
        if (!rifleShot) return false;
        if (!bazookaShot) return false;

        return true;
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
    /// ロケットエフェクトコントロール
    /// </summary>
    void RoketFireControl()
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

    /// <summary>
    /// ダメージを与える
    /// </summary>
    /// <param name="damage"></param>
    public override void Damage(int damage, Vector3 bulletPos)
    {
        base.Damage(damage, bulletPos);

        Vector3 directionToTarget = Vector3.Scale(transform.position - bulletPos, new Vector3(1, 0, 1));
        float dot = Vector3.Dot(directionToTarget.normalized, transform.forward);
        rb.velocity = Vector3.zero;
        if (dot > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = targetRotation;
            animator.SetInteger("DamageValue", 2);
        }
        else
        {
            directionToTarget = Vector3.Scale(bulletPos - transform.position, new Vector3(1, 0, 1));
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = targetRotation;
            animator.SetInteger("DamageValue", 1);
        }
        animator.SetTrigger("Damage");
        Stop();
    }

    /// <summary>
    /// ダメージアニメーションの終了
    /// </summary>
    void DamageFailde()
    {
        animator.SetInteger("DamageValue", 0);
        Go();
    }

    #region 行動のコントロール

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
    #endregion

    #region 移動

    /// <summary>
    /// 移動処理
    /// </summary>
    void MoveProsess()
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
        if (move.isDash && !isDashBtn && groundCheck.isGround)
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
        if (isMainShotBtn)
        {
            if (bazookaShot.isNow)
            {
                return;
            }

            if (!rifleShot.ShotCheck())
            {
                // 射撃不可
                return;
            }

            if (!rifleShot.isBackShot)
            {
                animator.SetTrigger("BeumRifleShot");
                animator.SetLayerWeight(beumRifleLayerIndex, 1);
            }
            else
            {
                animator.SetTrigger("BeumRifleShotBack");
            }
            Invoke("BeumRifleShotFailed", 0.8f);
        }
    }

    /// <summary>
    /// ビームライフルの弾を生成
    /// </summary>
    public void BeumRifleCreateBullet()
    {
        rifleShot.CreateBullet();
    }

    /// <summary>
    /// ビームライフル攻撃が終わった時の処理
    /// </summary>
    void BeumRifleShotFailed()
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
        if (isSubShotBtn)
        {
            if (!bazookaShot.ShotCheck())
            {
                return;
            }

            animator.SetTrigger("BazookaShot");
            Invoke("BazookaFailed", 0.8f);
            bazooka.SetActive(true);
        }
    }

    /// <summary>
    /// バズーカの弾を生成
    /// </summary>
    public void BazookaCreateBullet()
    {
        bazookaShot.CreateBullet();
    }

    /// <summary>
    /// バズーカの終了処理
    /// </summary>
    private void BazookaFailed()
    {
        bazookaShot.Failed();
        bazooka.SetActive(false);
    }

    #endregion
}

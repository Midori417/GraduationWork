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

    [SerializeField, Header("ビームライフルコンポネント")]
    private GundamRifleShot shotRifle;

    [SerializeField, Header("ビームライフルオブジェクト")]
    private GameObject beumRifle;

    [SerializeField, Header("バズーカオブジェクト")]
    private GameObject bazooka;

    [SerializeField, Header("バーニアエフェクト")]
    private ParticleSystem eff_roketFire;

    // 仮入力
    Vector2 moveAxis;
    bool isJumpBtn;
    bool isDashBtn;
    bool isMainShotBtn;
    bool isSubShotBtn;

    // ビームライフル攻撃レイヤーインデックス
    int beumRifleLayerIndex = 0;

    float beumRifleShotTime;
    float beumRifleShotBackTime;

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
        isMainShotBtn = Input.GetKeyDown(KeyCode.Mouse0);
        isSubShotBtn = Input.GetKeyDown(KeyCode.Alpha1);

        BoostCharge();

        if (!isStop)
        {
            Move();
            BeumRifle();
            Bazooka();
        }

        RoketFireControl();
        AnimUpdate();
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
        shotRifle.Initalize();

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
        if (!move) return false;
        if (!shotRifle) return false;

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
        if(!eff_roketFire)
        {
            return;
        }

        if (move.isDash || move.isJump)
        {
            Debug.Log("A");
            eff_roketFire.Play();
        }
        else
        {
            // 既に存在しているパーティクルは消さないで放出だけ止める
            //eff_roketFire.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            eff_roketFire.Stop();
        }
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

    #endregion

    #region ビームライフル

    /// <summary>
    /// ビームライフル処理
    /// </summary>
    public void BeumRifle()
    {
        if (isMainShotBtn)
        {
            if (!shotRifle.ShotCheck())
            {
                // 射撃不可
                return;
            }

            if (!shotRifle.isBackShot)
            {
                animator.SetTrigger("BeumRifleShot");
                animator.SetLayerWeight(beumRifleLayerIndex, 1);
            }
            else
            {
                animator.SetTrigger("BeumRifleShotBack");
            }
            StartCoroutine("BeumRifleShotFailed");
        }
    }

    /// <summary>
    /// ビームライフル攻撃が終わった時の処理
    /// </summary>
    IEnumerator BeumRifleShotFailed()
    {
        yield return new WaitForSeconds(0.8f);
        shotRifle.Failed();
        animator.SetLayerWeight(beumRifleLayerIndex, 0);
    }

    #endregion

    #region バズーカ

    /// <summary>
    /// バズーカ処理
    /// </summary>
    void Bazooka()
    {
        if(isSubShotBtn)
        {
            animator.SetTrigger("BazookaShot");
            beumRifle.SetActive(false);
            bazooka.SetActive(true);
        }
    }

    /// <summary>
    /// Bazooka終了処理
    /// </summary>
    public void BazookaFailed()
    {
        beumRifle.SetActive(true);
        bazooka.SetActive(false);
    }

    #endregion
}

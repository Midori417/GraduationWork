using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ガンダム
/// </summary>
public class Gundam : MonoBehaviour
{
    [SerializeField, Header("自身のカメラ")]
    private Transform _myCamera;

    // 地面判定コンポーネント
    private GroundCheck groundCheck;

    [SerializeField, Header("移動コンポーネント")]
    private MsMove move;

    // 他に伝える
    public Rigidbody rb
    { get; private set; }
    public Animator animator
    { get; private set; }
    public Transform myCamera
    { get { return _myCamera; } }

    // 仮入力
    Vector2 moveAxis;
    bool isJumpBtn;
    bool isDashBtn;

    /// <summary>
    /// 必要なコンポーネントを取得
    /// </summary>
    void UseGetComponent()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        groundCheck = GetComponentInChildren<GroundCheck>();
    }

    #region イベント

    /// <summary>
    /// Updateより前に実行する
    /// </summary>
    private void Start()
    {
        UseGetComponent();
        rb.drag = 0.1f;
        boostParamater.Initialize();
        move.Initalize();
    }

    /// <summary>
    /// 必要なコンポーネントがあるか
    /// </summary>
    /// <returns></returns>
    bool ComponentCheck()
    {
        if (!move)
        {
            return false;
        }
        if (!groundCheck)
        {
            return false;
        }

        return true;
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

        boostParamater.Charge();

        animator.SetTrigger("BeumRifleShot");

        Move();
        AnimUpdate();
    }

    /// <summary>
    ///  Updateより後に実行
    /// </summary>
    private void LateUpdate()
    {

    }

    #endregion

    #region ブースト関係

    [System.Serializable]
    private struct BoostParamater
    {
        // エネルギーの最大量
        public static float max = 100;

        // 現在のエネルギー量
        private float _current;
        public float current
        {
            get
            {
                return _current;
            }
        }

        [Header("チャージ速度")]
        public float chargeSpeed;

        // チャージタイマー
        private float chargeTimer;

        [Header("チャージロック時間")]
        public float chargeLockTime;

        [Header("オーバーヒートしたときのチャージロック時間")]
        public float overHeartChargeLockTime;

        // trueなら使用中
        private bool isUse;

        /// <summary>
        /// ブーストパラメータの初期化
        /// </summary>
        public void Initialize()
        {
            _current = max;
        }

        /// <summary>
        /// チャージ処理
        /// </summary>
        public void Charge()
        {
            // 使用中はチャージを行わない
            if (isUse)
            {
                return;
            }

            // チャージタイマーが0以上あれば減らす
            if (chargeTimer > 0)
            {
                chargeTimer -= Time.deltaTime;
            }
            else
            {
                // エネルギーを回復する
                _current += chargeSpeed * Time.deltaTime;
            }

            // 値を補正
            chargeTimer = Mathf.Clamp(chargeTimer, 0, overHeartChargeLockTime);
            _current = Mathf.Clamp(_current, 0, max);
        }

        /// <summary>
        /// ブーストの消費
        /// </summary>
        /// <param name="value">消費量</param>
        public void UseBoost(float value)
        {
            // 0以下なら処理を行わない
            if (_current <= 0)
            {
                return;
            }

            // 使用中
            isUse = true;

            // 消費
            _current -= value;

            // チャージタイムを入れておく
            if (_current > 0)
            {
                chargeTimer = chargeLockTime;
            }
            // 0以下ならオーバーヒート状態
            else
            {
                chargeTimer = overHeartChargeLockTime;
            }

            // 値を補正
            _current = Mathf.Clamp(_current, 0, max);
        }
    }
    [SerializeField, Header("ブーストパラメータ")]
    private BoostParamater boostParamater;

    #endregion

    /// <summary>
    /// 移動処理
    /// </summary>
    void Move()
    {
        // 着地した瞬間
        if(groundCheck.isGround && groundCheck.oldIsGround)
        {
            //move.Landing();
            //animator.SetTrigger("Landing");
        }

        // 地面ついているときのみ
        if (groundCheck.isGround)
        {
            move.Move(moveAxis);
        }

        if (!move.isDash)
        {
            move.Jump(moveAxis, isJumpBtn);
        }
        move.Dash(moveAxis, isDashBtn);

        // ダッシュとジャンプ中は重力の影響を受けない
        rb.useGravity = (!isDashBtn) && (!isJumpBtn);
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
}

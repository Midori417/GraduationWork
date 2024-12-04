using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ガンダム
/// </summary>
public class Gundam : MonoBehaviour
{
    [SerializeField, Header("自身のカメラ")]
    private Transform myCamera;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private GroundCheck groundCheck;

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
        boostParamater.Initialize();
    }

    /// <summary>
    /// 毎フレーム実行
    /// </summary>
    private void Update()
    {
        boostParamater.Charge();

        Vector2 moveAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Move(moveAxis);
        MoveAnim();
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
            if(isUse)
            {
                return;
            }

            // チャージタイマーが0以上あれば減らす
            if(chargeTimer > 0)
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
            if(_current <= 0)
            {
                return;
            }

            // 使用中
            isUse = true;

            // 消費
            _current -= value;

            // チャージタイムを入れておく
            if(_current > 0)
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

    #region 移動関係

    /// <summary>
    /// 移動パラメータ
    /// </summary>
    [System.Serializable]
    private struct MoveParamater
    {
        [Header("移動速度")]
        public float speed;

        [Header("旋回速度")]
        public float rotationSpeed;
    }
    [SerializeField, Header("移動パラメータ")]
    private MoveParamater moveParamater;

    /// <summary>
    /// カメラを基準に移動方向を取得
    /// </summary>
    /// <param name="moveAxis">移動軸</param>
    /// <returns></returns>
    Vector3 MoveForward(Vector2 moveAxis)
    {
        // カメラの方向から、X-Z単位ベクトル(正規化)を取得
        Vector3 cameraForward = Vector3.Scale(myCamera.forward, new Vector3(1, 0, 1));
        Vector3 moveForward = cameraForward * moveAxis.y + myCamera.right * moveAxis.x;

        return moveForward;
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    public void Move(Vector2 moveAxis)
    {
        Vector3 moveFoward = MoveForward(moveAxis);

        // 進行方向に回転しながら正面方向に進む
        if (moveFoward != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(moveFoward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 
                moveParamater.rotationSpeed * Time.deltaTime);
            rb.velocity = transform.forward * moveParamater.speed + new Vector3(0, rb.velocity.y, 0);
        }
        else
        {
            // 移動入力がなくなったら速度をなくす
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    /// <summary>
    /// 移動関係のアニメーション
    /// </summary>
    void MoveAnim()
    {
        float speed = new Vector2(rb.velocity.x, rb.velocity.z).magnitude;
        animator.SetFloat("Speed", speed);
        animator.SetBool("IsGround", groundCheck.isGround);
    }

    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 機体の基本コンポーネント
/// </summary>
public class BaseMs : MonoBehaviour
{
    [SerializeField, Header("自身のセンター位置")]
    private Transform _center;

    // 毎カメラ
    private Transform _myCamera;

    // ターゲット機体
    private BaseMs _targetMs;

    // 地面判定コンポーネント
    public GroundCheck groundCheck
    {
        get;
        private set;
    }

    // 体力
    private int _hp;

    [SerializeField, Header("最大体力")]
    private int _hpMax;

    [System.Serializable]
    private struct BoostParamater
    {
        // エネルギーの最大量
        private static float max = 100;

        // 現在のエネルギー量
        private float current;

        // 現在のエネルギーの割合(0～1)
        public float current01
        { get { return Mathf.Clamp01((max - (max - current)) / max); } }

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
            current = max;
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
                current = max;
            }

            // 値を補正
            chargeTimer = Mathf.Clamp(chargeTimer, 0, overHeartChargeLockTime);
            current = Mathf.Clamp(current, 0, max);
        }

        /// <summary>
        /// ブーストの消費
        /// </summary>
        /// <param name="value">消費量</param>
        public void UseBoost(float value)
        {
            // 0以下なら処理を行わない
            if (current <= 0)
            {
                return;
            }

            // 消費
            current -= value * Time.deltaTime;

            // チャージタイムを入れておく
            if (current > 0)
            {
                chargeTimer = chargeLockTime;
            }
            // 0以下ならオーバーヒート状態
            else
            {
                chargeTimer = overHeartChargeLockTime;
            }

            // 値を補正
            current = Mathf.Clamp(current, 0, max);
        }
    }
    [SerializeField, Header("ブーストパラメータ")]
    private BoostParamater boostParamater;

    // 仮入力
    public Vector2 moveAxis;
    public bool isJumpBtn;
    public bool isDashBtn;
    public bool isMainShotBtn;
    public bool isSubShotBtn;

    #region  他に伝える

    public Rigidbody rb
    { get; private set; }
    public Animator animator
    { get; private set; }

    // 自身のカメラ
    public Transform myCamera
    { get { return _myCamera; } }

    // ターゲット機体
    public BaseMs targetMs
    { get { return _targetMs; } }

    // センター
    public Transform center
    { get { return _center; } }

    // 体力
    public int hp
    { get { return _hp; } }

    // 最大体力
    public int hpMax
    { get { return _hpMax; } }

    // 現在のエネルギーの割合(0～1)
    public float boost01
    { get { return boostParamater.current01; } }

    // UIに表示する弾
    public List<BaseMsAmoParts> uiArmed;

    #endregion

    /// <summary>
    /// 初期化
    /// </summary>
    public  virtual void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        groundCheck = GetComponentInChildren<GroundCheck>();

        boostParamater.Initialize();

        // 体力を設定
        _hp = hpMax;
    }

    /// <summary>
    /// 必要なコンポーネントがあるか
    /// </summary>
    /// <returns></returns>
    protected virtual bool ComponentCheck()
    {
        if (!rb) return false;
        if (!animator) return false;
        if (!groundCheck)
        {
            Debug.LogError("地面判定コンポーネントがない");
            return false;
        }
        if (!center) return false;

        return true;
    }

    /// <summary>
    /// ダメージを与える
    /// </summary>
    /// <param name="damage"></param>
    public virtual void Damage(int damage, Vector3 bulletPos)
    {
        _hp -= damage;
        _hp = Mathf.Clamp(_hp, 0, hpMax);
    }

    #region ブースト関係

    /// <summary>
    /// ブーストエネルギーのチャージ
    /// </summary>
    protected void BoostCharge()
    {
        if (!groundCheck.isGround)
        {
            return;
        }
        boostParamater.Charge();
    }

    /// <summary>
    /// ブーストの消費
    /// </summary>
    /// <param name="value">消費量</param>
    public void UseBoost(float value)
    {
        boostParamater.UseBoost(value);
    }

    #endregion

    /// <summary>
    /// ターゲット機体を設定
    /// </summary>
    /// <param name="target"></param>
    public void SetTargetMs(BaseMs target)
    {
        _targetMs = target;
    }

    /// <summary>
    /// 自身のカメラを設定
    /// </summary>
    /// <param name="myCameraTrs"></param>
    public void SetMyCamera(Transform myCameraTrs)
    {
        _myCamera = myCameraTrs;
    }
}

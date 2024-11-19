using UnityEngine;

/// <summary>
/// 基底コンポーネント
/// </summary>
public class BaseMs : MonoBehaviour
{
    // パイロットからの入力
    public PilotInput pilotInput
    {
        get;
        private set;
    }

    protected Rigidbody rb;
    protected Animator anim;

    // カメラの位置
    public Transform cameraTrs
    {
        get;
        private set;
    }

    // 地面判定
    private GroundCheck groundCheck;

    // 地面についているか取得
    public bool isGround
    {
        get
        {
            if (groundCheck)
            {
                return groundCheck.isGround;
            }
            else
            {
                return false;
            }
        }
    }
    // 前回のフレームで地面についていたか
    public bool olsIsGround
    {
        get
        {
            if (groundCheck)
            {
                return groundCheck.oldIsGround;
            }
            else
            {
                return false;
            }
        }

    }

    /// <summary>
    /// ブーストパラメータ
    /// </summary>
    [System.Serializable]
    private struct BoostParamter
    {
        [Header("最大ブースト量")]
        public float max;

        [Header("現在のブースト量")]
        public float current;

        [Header("チャージ速度")]
        public float chargeSpeed;

        // チャージタイマー
        public float timer;

        [Header("チャージまでの時間")]
        public float chargeTime;

        [Header("trueならチャージをロックする")]
        public bool chargeLock;
    }
    [SerializeField, Header("ブーストパラメータ")]
    private BoostParamter boostParamater;
    public float boostCurrent
    {
        get
        {
            return boostParamater.current;
        }
    }

    /// <summary>
    /// 基本となるコンポーネントを取得
    /// </summary>
    /// <returns>
    /// true=取得に成功
    /// false=取得に失敗
    /// </returns>
    public bool GetBaseMsComponent()
    {
        rb = GetComponent<Rigidbody>();
        if (!rb)
        {
            Debug.Log(gameObject.name + "Rigidbodyが取得できません");
            return false;
        }
        anim = GetComponent<Animator>();
        if (!anim)
        {
            Debug.Log(gameObject.name + "Animatorが取得できません");
            return false;
        }
        groundCheck = GetComponentInChildren<GroundCheck>();
        if (!groundCheck)
        {
            Debug.Log(gameObject.name + "GroundCheckが取得できません");
            return false;
        }

        return true;
    }

    /// <summary>
    /// カメラのトランスフォームを取得する
    /// </summary>
    /// <param name="cameraTrs"></param>
    public void SetCameraTrs(Transform _cameraTrs)
    {
        cameraTrs = _cameraTrs;
    }

    /// <summary>
    /// パイロットからの入力を設定
    /// </summary>
    /// <param name="_pilotInput"></param>
    public void SetPilotInput(PilotInput _pilotInput)
    {
        pilotInput = _pilotInput;
    }

    #region Boost関数

    /// <summary>
    /// ブーストパラメータの初期化
    /// </summary>
    protected void BoostGaugeInit()
    {
        boostParamater.max = 100;
        boostParamater.current = boostParamater.max;
        boostParamater.chargeTime = 0.2f;
        boostParamater.chargeSpeed = 100;
    }

    /// <summary>
    /// ブーストゲージのチャージ処理
    /// </summary>
    protected void BoostGaugeChage()
    {
        if (boostParamater.timer > 0 && isGround)
        {
            boostParamater.timer -= Time.deltaTime;
            if (boostParamater.timer <= 0)
            {
                boostParamater.chargeLock = false;
            }
        }

        if (!boostParamater.chargeLock)
        {
            if (boostParamater.current < boostParamater.max)
            {
                boostParamater.current += boostParamater.chargeSpeed;
            }
        }
        boostParamater.current = Mathf.Clamp(boostParamater.current, 0, boostParamater.max);
    }

    /// <summary>
    /// ブーストを使用する
    /// </summary>
    /// <param name="useBoost">使用量</param>
    public void UseBoost(float useBoost)
    {
        if (boostParamater.current > 0)
        {
            boostParamater.current -= useBoost * Time.deltaTime;
            boostParamater.timer = boostParamater.chargeTime;
            boostParamater.chargeLock = true;
        }
    }

    #endregion

    /// <summary>
    /// カメラの正面を基準に移動方向を計算する
    /// </summary>
    /// <returns></returns>
    public Vector3 MoveForward(Vector2 moveAxis)
    {
        if (!cameraTrs)
        {
            return Vector3.zero;
        }
        Vector3 cameraForward = Vector3.Scale(cameraTrs.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveForward = cameraForward * moveAxis.y + cameraTrs.right * moveAxis.x;

        return moveForward;
    }
}
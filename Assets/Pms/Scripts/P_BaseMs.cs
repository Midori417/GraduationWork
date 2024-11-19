using UnityEngine;

/// <summary>
/// 基底コンポーネント
/// </summary>
public class P_BaseMs : MonoBehaviour
{

    [Header("カメラトランスフォームコンポーネント")]
    public Transform cameraTrs;

    [Header("パイロットの入力")]
    protected PilotInput pilotInput;

    [SerializeField, Header("リジッドボディコンポーネント")]
    protected Rigidbody rb;

    [SerializeField, Header("アニメータコンポーネント")]
    protected Animator anim;

    [SerializeField, Header("地面設置判定コンポーネント")]
    private GroundCheck groundCheck;

    [SerializeField, Header("バーニアエフェクト")]
    private GameObject FireRoketEffect;

    [SerializeField, Header("パーティクルシステム")]
    private new ParticleSystem particleSystem;

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

    protected bool isStop;

    protected int maxHp;

    [SerializeField, Header("現在の体力")]
    protected int hp;
    public int HP
    {
        get
        {
            return hp;
        }
    }

    [SerializeField, Header("戦力値")]
    protected int strengthValue;
    public int StrengthValue
    {
        get
        {
            return strengthValue;
        }
    }

    /// <summary>
    /// ブーストパラメータ
    /// </summary>
    [System.Serializable]
    protected struct BoostParamter
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
    protected BoostParamter boostParamater;
    public float BoostFill
    {
        get
        {
            return Mathf.Clamp01((boostParamater.max - (boostParamater.max - boostParamater.current)) / boostParamater.max);
        }
    }

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

    public void Stop()
    {
        isStop = false;
    }

    /// <summary>
    /// 必要なコンポーネントを取得する
    /// </summary>
    /// <returns></returns>
    public bool UseGetComponent()
    {
        if (!cameraTrs)
        {
            Debug.Log("カメラのトランスフォームが取得できません");
            return false;
        }
        if (!rb)
        {
            rb = GetComponent<Rigidbody>();
            if (!rb)
            {
                Debug.Log(gameObject.name + "Rigidbodyが取得できません");
                return false;
            }
        }
        if (!anim)
        {
            anim = GetComponent<Animator>();
            if (!anim)
            {
                Debug.Log(gameObject.name + "Animatorが取得できません");
                return false;
            }
        }
        if (!groundCheck)
        {
            groundCheck = GetComponentInChildren<GroundCheck>();
            if (!groundCheck)
            {
                Debug.Log(gameObject.name + "GroundCheckが取得できません");
                return false;
            }
        }
        if (!pilotInput)
        {
            pilotInput = GetComponentInParent<PilotInput>();
            if (!pilotInput)
            {
                Debug.Log(gameObject.name + "PilotInputが取得できません");
                return false;
            }
        }
        if (!FireRoketEffect)
        {
            particleSystem = FireRoketEffect.GetComponent<ParticleSystem>();
            if (!FireRoketEffect)
            {
                Debug.Log(gameObject.name + "FireRoketEffectが取得できません");
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// ブーストゲージのチャージ処理
    /// </summary>
    protected void BoostGaugeChage()
    {
        if (boostParamater.timer > 0 && isGround)
        {
            boostParamater.timer -= Time.deltaTime;
            if(boostParamater.timer <= 0)
            {
                boostParamater.chargeLock = false;
            }
        }

        if(!boostParamater.chargeLock)
        {
            if (boostParamater.current < boostParamater.max)
            {
                boostParamater.current += boostParamater.chargeSpeed;
            }
        }
    }

    /// <summary>
    /// ブーストゲージを使用する
    /// </summary>
    /// <param name="useBoost">使用量</param>
    protected void UseBoostGauge(float useBoost)
    {
        if (boostParamater.current > 0)
        {
            boostParamater.current -= useBoost * Time.deltaTime;
            boostParamater.timer = boostParamater.chargeTime;
            boostParamater.chargeLock = true;
        }
    }

    // エフェクトを停止
    protected void StopEffect()
    {
        //if (!particleSystem && particleSystem.isPlaying)
        //{
        //    particleSystem.Stop();
        //}
    }

    // エフェクトを開始
    public void PlayEffect()
    {
        //if (!particleSystem && !particleSystem.isPlaying)
        //{
        //    particleSystem.Play();
        //}
    }

    /// <summary>
    /// カメラの正面を基準に移動方向を計算する
    /// </summary>
    /// <returns></returns>
    protected Vector3 MoveForward(Vector2 moveAxis)
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
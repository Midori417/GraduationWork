using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ガンダム
/// </summary>
public class P_Gundam : P_BaseMs
{
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
    /// ジャンプパラメータ
    /// </summary>
    [System.Serializable]
    private struct JumpParamater
    {
        [Header("ジャンプ力")]
        public float power;

        [Header("移動速度")]
        public float speed;

        [Header("旋回速度")]
        public float rotationSpeed;

        [Header("ブーストゲージの消費量")]
        public float useBoost;

        [Header("ジャンプ中か")]
        public bool isNow;

        [Header("着地時の慣性")]
        public float inertia;
    }
    [SerializeField, Header("ジャンプパラメータ")]
    private JumpParamater jumpParamater;
    private float additionalGravity;  //重力の強化

    /// <summary>
    /// ダッシュパラメータ
    /// </summary>
    [System.Serializable]
    private struct DashParamater
    {
        [Header("移動速度")]
        public float speed;

        [Header("旋回速度")]
        public float rotationSpeed;

        [Header("ブーストゲージの消費量")]
        public float useBoost;

        [Header("ダッシュ中か")]
        public bool isNow;
    }
    [SerializeField, Header("ダッシュパラメータ")]
    private DashParamater dashParamater;

    [SerializeField, Header("ターゲット機体")]
    private GameObject target;

    /// <summary>
    /// Updateの前に実行
    /// </summary>
    private void Start()
    {
        // 必要なコンポーネントを取得する
        // コンポーネントが足りなければスクリプトを停止する
        if (!UseGetComponent())
        {
            enabled = false;
        }

        Initialize();
    }

    /// <summary>
    /// ガンダムの機体パラメータの初期化
    /// </summary>
    private void Initialize()
    {
        maxHp = 600;
        hp = maxHp;
        strengthValue = 2000;

        BoostGaugeInit();

        moveParamater.speed = 20.0f;
        moveParamater.rotationSpeed = 0.01f;

        jumpParamater.power = 25.0f;
        jumpParamater.speed = 5.0f;
        jumpParamater.rotationSpeed = 0.08f;
        jumpParamater.useBoost = 20;
        jumpParamater.inertia = 0.23f;

        dashParamater.speed = 40.0f;
        dashParamater.rotationSpeed = 0.12f;
        dashParamater.useBoost = 20;

        additionalGravity = 40.0f; //重力強化
        rb.drag = 0.1f;            //空気抵抗
        //rb.angularDrag = 0.05f;  //回転の空気抵抗
    }

    /// <summary>
    /// 毎フレーム実行
    /// </summary>
    private void Update()
    {
        UseGravity();
        BoostGaugeChage();

        if (!olsIsGround && isGround)
        {
            anim.SetTrigger("Ground");
            isStop = true;
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, jumpParamater.inertia);
        }
        if (isStop)
        {
            return;
        }

        Move(pilotInput.moveAxis);
        Jump(pilotInput.moveAxis, pilotInput.isJumpBtn);
        Dash(pilotInput.moveAxis, pilotInput.isDashBtn);

        // ブーストパラメータを補正
        boostParamater.current = Mathf.Clamp(boostParamater.current, 0, boostParamater.max);

        // エフェクトの開始と停止
        if (jumpParamater.isNow || dashParamater.isNow)
        {
            PlayEffect();
        }
        else if (!jumpParamater.isNow && !dashParamater.isNow)
        {
            StopEffect();
        }
    }

    void FixedUpdate()
    {
        // Y軸方向に追加の重力を加える
        rb.AddForce(Vector3.down * additionalGravity, ForceMode.Acceleration);
    }

    /// <summary>
    /// 重力が必要か
    /// </summary>
    void UseGravity()
    {
        if (dashParamater.isNow)
        {
            rb.useGravity = false;
        }
        else
        {
            rb.useGravity = true;
        }
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    private void Move(Vector2 moveAxis)
    {
        if (!isGround)
        {
            anim.SetBool("Move", false);
            return;
        }

        Vector3 moveFoward = MoveForward(moveAxis);

        // 進行方向に補間しながら回転
        if (moveFoward != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(moveFoward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, moveParamater.rotationSpeed);
            rb.velocity = transform.forward * moveParamater.speed;
            anim.SetBool("Move", true);
        }
        else
        {
            rb.velocity = Vector3.zero;
            anim.SetBool("Move", false);
        }
    }

    /// <summary>
    /// ジャンプ処理
    /// </summary>
    private void Jump(Vector2 moveAxis, bool isJump)
    {
        if (isJump)
        {
            if (boostParamater.current > 0)
            {
                Vector3 moveFoward = MoveForward(moveAxis);

                // 進行方向に補間しながら回転
                if (moveFoward != Vector3.zero)
                {
                    Quaternion rotation = Quaternion.LookRotation(moveFoward);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, jumpParamater.rotationSpeed);
                    rb.velocity = transform.forward * jumpParamater.speed;
                }
                rb.velocity = new Vector3(rb.velocity.x, jumpParamater.power, rb.velocity.z);

                UseBoostGauge(jumpParamater.useBoost);
                jumpParamater.isNow = true;
            }
            else
            {
                jumpParamater.isNow = false;
            }
        }
        else
        {
            jumpParamater.isNow = false;
        }

        anim.SetBool("Jump", jumpParamater.isNow);
    }

    /// <summary>
    /// ダッシュ処理
    /// </summary>
    private void Dash(Vector2 moveAxis, bool isDash)
    {
        if (isDash)
        {
            if (boostParamater.current > 0)
            {
                Vector3 moveFoward = MoveForward(moveAxis);
                // 進行方向に補間しながら回転
                if (moveFoward != Vector3.zero)
                {
                    Quaternion rotation = Quaternion.LookRotation(moveFoward);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, dashParamater.rotationSpeed);
                }
                rb.velocity = transform.forward * dashParamater.speed;

                UseBoostGauge(dashParamater.useBoost);
                dashParamater.isNow = true;
            }
            else
            {
                dashParamater.isNow = false;
            }
        }
        else
        {
            dashParamater.isNow = false;
        }

        anim.SetBool("Dash", dashParamater.isNow);
    }

    /// <summary>
    /// ダメージを受ける
    /// 攻撃する側に呼び出してもらう
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(int damage)
    {
        hp -= damage;
    }
}

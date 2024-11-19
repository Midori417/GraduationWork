using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ガンダムの移動
/// </summary>
public class GundomMove : BaseParts
{
    Rigidbody rb;

    private float additionalGravity;  //重力の強化

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
    public bool isMove
    {
        get;
        private set;
    }

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
    public bool isJump
    {
        get
        {
            return jumpParamater.isNow;
        }
    }

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
    public bool isDash
    {
        get
        {
            return dashParamater.isNow;
        }
    }

    void FixedUpdate()
    {
        // Y軸方向に追加の重力を加える
        rb.AddForce(Vector3.down * additionalGravity, ForceMode.Acceleration);
    }

    /// <summary>
    /// ステータスの初期化
    /// </summary>
    public void Initalize()
    {
        GetBasePartsComponent();
        rb = GetComponent<Rigidbody>();
        if (!rb)
        {
            Debug.Log("Rigidbodyの取得に失敗");
            return;
        }

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
    }

    /// <summary>
    /// 着地処理
    /// </summary>
    public void Landing()
    {
        rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, jumpParamater.inertia);
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    public void Move(Vector2 moveAxis)
    {
        Vector3 moveFoward = MoveForward(moveAxis);

        // 進行方向に補間しながら回転
        if (moveFoward != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(moveFoward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, moveParamater.rotationSpeed);
            rb.velocity = transform.forward * moveParamater.speed;
            isMove = true;
        }
        else
        {
            rb.velocity = Vector3.zero;
            isMove = false;
        }
    }

    /// <summary>
    /// ジャンプ処理
    /// </summary>
    public void Jump(Vector2 moveAxis, bool isJumpBtn)
    {
        if (isJumpBtn)
        {
            if (GetBoostCurrent() > 0)
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

                UseBoost(jumpParamater.useBoost);
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
    }

    /// <summary>
    /// ダッシュ処理
    /// </summary>
    public void Dash(Vector2 moveAxis, bool isDashBtn)
    {
        if (isDashBtn)
        {
            if (GetBoostCurrent() > 0)
            {
                Vector3 moveFoward = MoveForward(moveAxis);
                // 進行方向に補間しながら回転
                if (moveFoward != Vector3.zero)
                {
                    Quaternion rotation = Quaternion.LookRotation(moveFoward);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, dashParamater.rotationSpeed);
                }
                rb.velocity = transform.forward * dashParamater.speed;

                UseBoost(dashParamater.useBoost);
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
    }
}

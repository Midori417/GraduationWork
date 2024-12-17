using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Msの移動
/// </summary>
public class MsMove : BaseMsParts
{
    // 自身のカメラ
    private Transform myCamera;

    [SerializeField]
    private AudioSource se_AudioSource;

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

    // 他のところに伝える変数
    public bool isJump
    {
        get
        {
            return jumpParamater.isNow;
        }
    }
    public bool isDash
    {
        get
        {
            return dashParamater.isNow;
        }
    }

    [SerializeField, Header("ブースト使用開始音")]
    private AudioClip se_boostStart;
    [SerializeField, Header("着地音")]
    private AudioClip se_landing;

    /// <summary>
    /// 初期処理
    /// </summary>
    public override bool Initalize()
    {
        base.Initalize();
        myCamera = mainMs.myCamera;

        return true;
    }

    /// <summary>
    /// 着地処理
    /// </summary>
    public void Landing()
    {
        se_AudioSource.PlayOneShot(se_landing);
        rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, jumpParamater.inertia);
    }

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
    /// 進行方向に回転
    /// </summary>
    void MoveForwardRot(Vector3 moveForward, float rotSpeed)
    {
        Quaternion rotation = Quaternion.LookRotation(moveForward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    public void Move(Vector2 moveAxis)
    {
        if (isDash)
        {
            return;
        }

        // 進行方向に回転しながら正面方向に進む
        if (moveAxis != Vector2.zero)
        {
            Vector3 moveFoward = MoveForward(moveAxis);
            MoveForwardRot(moveFoward, moveParamater.rotationSpeed);
            rb.velocity = transform.forward * moveParamater.speed + new Vector3(0, rb.velocity.y, 0);
        }
        else
        {
            // 移動入力がなくなったら速度をなくす
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    /// <summary>
    /// ジャンプ処理
    /// </summary>
    public void Jump(Vector2 moveAxis, bool isJumpBtn)
    {
        if (isJumpBtn)
        {
            if (mainMs.boost01 > 0)
            {
                Vector3 moveFoward = MoveForward(moveAxis);

                // 進行方向に補間しながら回転
                if (moveFoward != Vector3.zero)
                {
                    MoveForwardRot(moveFoward, jumpParamater.rotationSpeed);
                    rb.velocity = transform.forward * moveParamater.speed + new Vector3(0, rb.velocity.y, 0);
                }
                rb.velocity = new Vector3(rb.velocity.x, jumpParamater.power, rb.velocity.z);

                // 開始時に一度だけサウンド
                if(!jumpParamater.isNow)
                { }

                jumpParamater.isNow = true;

                // エネルギーの使用
                mainMs.UseBoost(jumpParamater.useBoost);
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
            if (mainMs.boost01 > 0)
            {
                Vector3 moveFoward = MoveForward(moveAxis);
                // 進行方向に補間しながら回転
                if (moveFoward != Vector3.zero)
                {
                    MoveForwardRot(moveFoward, dashParamater.rotationSpeed);
                }
                rb.velocity = transform.forward * dashParamater.speed;

                dashParamater.isNow = true;

                // エネルギーの使用
                mainMs.UseBoost(dashParamater.useBoost);
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

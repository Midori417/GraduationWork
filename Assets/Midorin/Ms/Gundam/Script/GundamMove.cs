using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Msの移動
/// </summary>
public class MsMove : MonoBehaviour
{
    // メインコンポネント
    private Gundam mainMs;

    // 物理コンポーネント
    private Rigidbody rb;
    
    // 自身のカメラ
    private Transform myCamera;

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

    /// <summary>
    /// 初期処理
    /// </summary>
    public void Initalize()
    {
        mainMs = GetComponent<Gundam>();
        if (!mainMs)
        {
            Debug.LogError("メインコンポーネントが取得できません");
            mainMs.enabled = false;
            return;
        }

        rb = mainMs.rb;
        myCamera = mainMs.myCamera;
    }

    /// <summary>
    /// 着地処理
    /// </summary>
    public void Landing()
    {
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
        if(isDash)
        {
            return;
        }

        Vector3 moveFoward = MoveForward(moveAxis);

        // 進行方向に回転しながら正面方向に進む
        if (moveFoward != Vector3.zero)
        {
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
            if (10 > 0)
            {
                Vector3 moveFoward = MoveForward(moveAxis);

                // 進行方向に補間しながら回転
                if (moveFoward != Vector3.zero)
                {
                    MoveForwardRot(moveFoward, jumpParamater.rotationSpeed);
                    rb.velocity = transform.forward * moveParamater.speed + new Vector3(0, rb.velocity.y, 0);
                }
                rb.velocity = new Vector3(rb.velocity.x, jumpParamater.power, rb.velocity.z);

                jumpParamater.isNow = true;
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
            if (10 > 0)
            {
                Vector3 moveFoward = MoveForward(moveAxis);
                // 進行方向に補間しながら回転
                if (moveFoward != Vector3.zero)
                {
                    MoveForwardRot(moveFoward, dashParamater.rotationSpeed);
                }
                rb.velocity = transform.forward * dashParamater.speed;

                dashParamater.isNow = true;
            }
        }
        else
        {
            dashParamater.isNow = false;
        }
    }
}

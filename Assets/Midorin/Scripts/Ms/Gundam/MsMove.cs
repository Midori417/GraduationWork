using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Msの移動
/// </summary>
public class MsMove : BaseMsParts
{
    // 自身のカメラ
    private Transform _myCamera;
    
    [Serializable]
    private struct MoveValiable
    {
        [Header("移動速度")]
        public float _speed;

        [Header("旋回速度")]
        public float _rotationSpeed;
    }
    [SerializeField, Header("移動パラメータ")]
    private MoveValiable _move;

    [Serializable]
    private struct JumpValiable
    {
        [Header("ジャンプ力")]
        public float _power;

        [Header("移動速度")]
        public float _speed;

        [Header("旋回速度")]
        public float _rotationSpeed;

        [Header("ブーストゲージの消費量")]
        public float useBoost;

        [HideInInspector, Header("ジャンプ中か")]
        public bool isNow;

        [Header("着地時の慣性")]
        public float inertia;
    }
    [SerializeField, Header("ジャンプパラメータ")]
    private JumpValiable _jump;

   [Serializable]
    private struct DashValiable
    {
        [Header("移動速度")]
        public float speed;

        [Header("旋回速度")]
        public float rotationSpeed;

        [Header("ブーストゲージの消費量")]
        public float useBoost;

        [HideInInspector, Header("ダッシュ中か")]
        public bool isNow;
    }
    [SerializeField, Header("ダッシュパラメータ")]
    private DashValiable _dash;

    #region プロパティ

    public bool isJump => _jump.isNow;
    public bool isDash => _dash.isNow;
    
    #endregion

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Initalize()
    {
        base.Initalize();
        _myCamera = mainMs.myCamera;
    }

    /// <summary>
    /// 着地処理
    /// </summary>
    public void Landing()
    {
        rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, _jump.inertia);
    }

    /// <summary>
    /// カメラを基準に移動方向を取得
    /// </summary>
    /// <param name="moveAxis">移動軸</param>
    /// <returns></returns>
    Vector3 MoveForward(Vector2 moveAxis)
    {
        // カメラの方向から、X-Z単位ベクトル(正規化)を取得
        Vector3 cameraForward = Vector3.Scale(_myCamera.forward, new Vector3(1, 0, 1));
        Vector3 moveForward = cameraForward * moveAxis.y + _myCamera.right * moveAxis.x;

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
    public void GroundMove(Vector2 moveAxis)
    {
        if (isDash)
        {
            return;
        }

        // 進行方向に回転しながら正面方向に進む
        if (moveAxis != Vector2.zero)
        {
            Vector3 moveFoward = MoveForward(moveAxis);
            MoveForwardRot(moveFoward, _move._rotationSpeed);
            rb.velocity = transform.forward * _move._speed + new Vector3(0, rb.velocity.y, 0);
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
                    MoveForwardRot(moveFoward, _jump._rotationSpeed);
                    rb.velocity = transform.forward * _move._speed + new Vector3(0, rb.velocity.y, 0);
                }
                rb.velocity = new Vector3(rb.velocity.x, _jump._power, rb.velocity.z);

                _jump.isNow = true;

                // エネルギーの使用
                mainMs.UseBoost(_jump.useBoost);
            }
            else
            {
                _jump.isNow = false;
            }
        }
        else
        {
            _jump.isNow = false;
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
                    MoveForwardRot(moveFoward, _dash.rotationSpeed);
                }
                rb.velocity = transform.forward * _dash.speed;

                _dash.isNow = true;

                // エネルギーの使用
                mainMs.UseBoost(_dash.useBoost);
            }
            else
            {
                _dash.isNow = false;
            }
        }
        else
        {
            _dash.isNow = false;
        }
    }
}

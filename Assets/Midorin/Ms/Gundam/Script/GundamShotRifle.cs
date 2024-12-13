using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GundamShotRifle : MonoBehaviour
{
    // メインコンポネント
    private Gundam mainMs;

    /// <summary> 弾用プレハブ </summary>
    [Header("弾用プレハブ")]
    [SerializeField] private BeumRifleBullet bulletPrefab;

    /// <summary> 弾生成位置 </summary>
    [Header("弾生成位置")]
    [SerializeField] private Vector3 shotPos;

    // 現在の弾
    private int amo;

    [SerializeField, Header("最大弾")]
    private int amoMax;

    [SerializeField, Header("発射インターバル")]
    private float interval;

    [SerializeField, Header("腹ボーン")]
    private Transform spineBone;

    // ターゲット
    private Transform target;

    // タイマー
    private float shotTimer;

    // 射撃中
    private bool isShot;

    private bool returnRotaion;

    float rotationSpeed;

    // 初期回転を保存する変数
    [SerializeField]
    private Quaternion initialRotation;

    Quaternion targetRot;

    /// <summary>
    /// 初期処理
    /// </summary>
    public void Initalize()
    {
        mainMs = GetComponent<Gundam>();
        if (!mainMs)
        {
            Debug.LogError("メインコンポーネントが取得できません");
            return;
        }
        DebugLog();

        // 初期回転を保存
        if (spineBone != null)
        {
            initialRotation = spineBone.localRotation;
        }

        amo = amoMax;
    }

    /// <summary>
    /// デバッグ用
    /// </summary>
    private void DebugLog()
    {
        if (!bulletPrefab)
        {
            Debug.Log("弾用プレハブが設定されていません");
        }
    }

    private void Update()
    {
        if (shotTimer > 0)
        {
            shotTimer -= Time.deltaTime;
            if (shotTimer <= 0)
            {
                shotTimer = 0;
            }
        }
    }

    #region 一旦
    /// <summary>
    /// 
    /// </summary>
    private void LateUpdate()
    {
        // 射撃状態
        if (isShot)
        {
            if (!returnRotaion)
            {
                Vector3 directionToTarget = target.position - spineBone.position;
                Vector3 localDirection = spineBone.parent.InverseTransformDirection(directionToTarget);

                // ターゲット方向の回転を計算
                Quaternion targetRotation = Quaternion.LookRotation(localDirection);
                targetRot = targetRotation;

                // 現在の回転からターゲット回転への補完
                rotationSpeed += 5 * Time.deltaTime;
                rotationSpeed = Mathf.Clamp01(rotationSpeed);
                Quaternion smoothRotation = Quaternion.Slerp(spineBone.localRotation, targetRotation, rotationSpeed);

                // 回転を適用
                spineBone.localRotation = smoothRotation;
            }
            else
            {
                rotationSpeed += 5 * Time.deltaTime;
                rotationSpeed = Mathf.Clamp01(rotationSpeed);
                Quaternion smoothRotation = Quaternion.Slerp(targetRot, initialRotation, rotationSpeed);
                spineBone.localRotation = smoothRotation;
            }
        }
    }

    #endregion

    /// <summary>
    /// 弾が発射できるかチェック
    /// </summary>
    /// <returns>
    /// false 発射不可
    /// true 発射可能
    /// </returns>
    public bool ShotCheck()
    {
        // 弾がゼロ以下
        if (amo <= 0)
        {
            return false;
        }
        // 発射タイマーがゼロ以上
        if (shotTimer > 0)
        {
            return false;
        }
        if (isShot)
        {
            return false;
        }

        // ターゲットを指定
        if (mainMs.targetMs)
        {
            target = mainMs.targetMs.transform;
        }

        isShot = true;
        rotationSpeed = 0;
        // 射撃
        return true;
    }


    /// <summary>
    /// 弾を生成
    /// アニメーションイベントで呼び出す
    /// </summary>
    public void CreateBullet()
    {
        // 弾生成
        if (bulletPrefab)
        {
            if (target)
            {
                // ターゲットの方向を計算
                Vector3 directionToTarget = target.position - transform.position;
                Quaternion rot = Quaternion.LookRotation(directionToTarget);
                // 弾の生成位置を計算
                Vector3 pos = transform.position + rot * shotPos;

                // 弾を生成
                BeumRifleBullet bullet = Instantiate(bulletPrefab, pos, rot);
            }
            else
            {
                BeumRifleBullet bullet = Instantiate(bulletPrefab, spineBone);
            }
            // 弾を減らす
            amo--;
            shotTimer = interval;
        }
    }

    public void ReturnRotation()
    {
        returnRotaion = true;
        rotationSpeed = 0;
    }

    /// <summary>
    /// 終了処理
    /// </summary>
    public void Failed()
    {
        returnRotaion = false;
        target = null;
        isShot = false;
    }
}

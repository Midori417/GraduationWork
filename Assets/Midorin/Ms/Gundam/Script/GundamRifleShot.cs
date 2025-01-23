using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ガンダムのビームライフル武装
/// </summary>
public class GundamRifleShot : BaseMsAmoParts
{
    /// <summary> 弾用プレハブ </summary>
    [Header("弾用プレハブ")]
    [SerializeField] private RifleBullet bulletPrefab;

    /// <summary> 弾生成位置 </summary>
    [Header("弾生成位置")]
    [SerializeField] private Vector3 shotPos;

    [SerializeField, Header("インターバル")]
    private float interval;

    // trueなら射撃可能
    private bool isShotOk = true;

    [SerializeField, Header("腹ボーン")]
    private Transform spineBone;

    // ターゲット
    private Transform target;

    // trueなら元の角度に戻る
    private bool returnRotaion = false;

    [SerializeField, Header("回転速度")]
    private float rotationSpeed = 0;

    // 初期回転を保存する変数
    private Quaternion initialRotation;

    // 前回フレーム時の回転
    private Quaternion oldRotation;

    [SerializeField, Header("一発の弾がリロードされるまでの時間")]
    private float reloadTime = 0;

    // リロードタイマー
    private float reloadTimer = 0;

    // バックショット
    public bool isBackShot
    { get; private set; }

    public bool isNow
    { get; private set; }

    #region イベント

    private void LateUpdate()
    {
        // 行動中
        if (isNow)
        {
            if (!isBackShot)
            {
                SpineLookRotation();
            }
            else
            {
                BackLookRotaion();
            }
        }
        ReloadProsess();
    }

    #endregion

    /// <summary>
    /// リロード処理
    /// </summary>
    private void ReloadProsess()
    {
        if (amo < amoMax && reloadTimer <= 0)
        {
            reloadTimer = reloadTime;
        }
        if(reloadTimer > 0)
        {
            reloadTimer -= Time.deltaTime;
            if(reloadTimer<=0)
            {
                reloadTimer = 0;
                amo++;
            }
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns>
    /// true    初期化成功
    /// faklse  初期化失敗
    /// </returns>
    public override bool Initalize()
    {
        if (!base.Initalize())
        {
            return false;
        }

        // 初期回転を保存
        if (spineBone != null)
        {
            initialRotation = spineBone.localRotation;
        }

        amo = amoMax;
        isNow = false;
        isShotOk = true;
        isBackShot = false;

        return true;
    }

    /// <summary>
    /// 上半身をターゲットの方向に向ける
    /// </summary>
    void SpineLookRotation()
    {
        if (!returnRotaion)
        {
            if (target)
            {
                Vector3 directionToTarget = target.position - spineBone.position;
                Vector3 localDirection = spineBone.parent.InverseTransformDirection(directionToTarget);

                // ターゲット方向の回転を計算
                Quaternion targetRotation = Quaternion.LookRotation(localDirection);

                // 現在の回転からターゲット回転への補完
                Quaternion smoothRotation = Quaternion.Slerp(oldRotation, targetRotation, rotationSpeed * Time.deltaTime);

                // 回転を適用
                spineBone.localRotation = smoothRotation;
                oldRotation = spineBone.localRotation;
            }
        }
        else
        {
            Quaternion smoothRotation = Quaternion.Slerp(oldRotation, initialRotation, rotationSpeed * Time.deltaTime);
            spineBone.localRotation = smoothRotation;
            oldRotation = spineBone.localRotation;
        }
    }

    /// <summary>
    /// ターゲットの逆方向に向ける
    /// </summary>
    void BackLookRotaion()
    {
        if (!target)
        {
            return;
        }
        // ターゲット方向の回転を計算
        Vector3 directionToTarget = Vector3.Scale(transform.position - target.position, new Vector3(1, 0, 1));
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = targetRotation;
        rb.velocity = Vector3.zero;
    }

    /// <summary>
    /// 弾が発射できるかチェック
    /// </summary>
    /// <returns>
    /// false 発射不可
    /// true 発射可能
    /// </returns>
    public bool ShotCheck()
    {
        // 射撃不可条件
        // 射撃不可 弾が0以下 インターバルが0以上
        if (!isShotOk || amo <= 0)
        {
            return false;
        }

        // ターゲットを指定
        if (mainMs.targetMs)
        {
            target = mainMs.targetMs.center;
            oldRotation = spineBone.localRotation;
        }

        // バックショットか判定
        if (target)
        {
            // ターゲット方向を計算
            Vector3 directioToTarget = transform.position - target.position;
            float dot = Vector3.Dot(directioToTarget.normalized, transform.forward);
            if (dot > 0.5f)
            {
                isBackShot = true;
            }
        }

        // 射撃行動中
        isNow = true;
        isShotOk = false;

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
                // 自身の機体のセンターを代入
                Transform center = mainMs.center;

                // ターゲットの方向を計算
                Vector3 directionToTarget = target.position - center.position;
                Quaternion rot = Quaternion.LookRotation(directionToTarget);
                // 弾の生成位置を計算
                Vector3 pos = center.position + rot * shotPos;

                // 弾を生成
                RifleBullet bullet = Instantiate(bulletPrefab, pos, rot);
                bullet.Target = target;
            }
            else
            {
                // 自身の機体のセンターを代入
                Transform center = mainMs.center;
                Quaternion rot = transform.rotation;
                Vector3 pos = center.position + rot * shotPos;
                RifleBullet bullet = Instantiate(bulletPrefab, pos, rot);
            }
            // 弾を減らす
            amo--;
        }
    }

    /// <summary>
    /// 元の角度に戻る
    /// アニメーションで呼び出す
    /// </summary>
    public void ReturnRotation()
    {
        returnRotaion = true;
    }

    /// <summary>
    /// 終了処理
    /// </summary>
    public void Failed()
    {
        returnRotaion = false;
        target = null;
        isBackShot = false;
        isNow = false;
        Invoke("Interval", interval);
    }

    /// <summary>
    /// インターバル処理
    /// </summary>
    void Interval()
    {
        isShotOk = true;
    }
}

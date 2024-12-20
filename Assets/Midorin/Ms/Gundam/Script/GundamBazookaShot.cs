using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ガンダムバズーカ
/// </summary>
public class GundamBazookaShot : BaseMsAmoParts
{
    [SerializeField]
    private RifleBullet bulletPrefab;

    [SerializeField]
    private Vector3 shotPos;

    // 行動中か
    private bool isNow = false;

    // ターゲット
    private Transform target;

    private void LateUpdate()
    {
        if(isNow)
        {
            LookRotaion();
        }
    }

    /// <summary>
    /// ターゲットの方向に向ける
    /// </summary>
    void LookRotaion()
    {
        if (!target)
        {
            return;
        }
        // ターゲット方向の回転を計算
        Vector3 directionToTarget = Vector3.Scale(target.position - transform.position, new Vector3(1, 0, 1));
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = targetRotation;
        rb.velocity = Vector3.zero;
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

        amo = amoMax;

        return true;
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
        if (isNow || amo <= 0)
        {
            return false;
        }

        // ターゲットを指定
        if (mainMs.targetMs)
        {
            target = mainMs.targetMs.center;
        }

        // 射撃行動中
        isNow = true;

        // 射撃
        return true;
    }

    /// <summary>
    /// 弾を生成する
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
    /// 終了処理
    /// </summary>
    public void Failed()
    {
        isNow = false;
    }
}

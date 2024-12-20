using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RifleBullet : MonoBehaviour
{
    /// <summary> 速度 </summary>
    [Header("速度")]
    [SerializeField] private float speed;

    /// <summary> 射撃から弾が消えるまでの時間 </summary>
    [Header("射撃から弾が消えるまでの時間")]
    [SerializeField] private float destroyTime;

    /// <summary> 追尾可能な角度 </summary>
    [Header("追尾可能な角度(度数法)")]
    [SerializeField, Range(0, 180.0f)] private float homingAngle;

    /// <summary> 追尾性能(%) </summary>
    [Header("追尾性能(%), 100%で追尾可能な角度分回転して追尾する")]
    [SerializeField, Range(0, 100.0f)] private float homingPercent;

    /// <summary> 射撃対象 </summary>
    private Transform target;

    /// <summary> %の最大値 </summary>
    private const float percentMax = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        // 一定時間後に削除
        Destroy(gameObject, destroyTime);

        // nullチェック
        if(!target)
        {
            Debug.Log("射撃対象が見つかりません。");
            return;
        }

        // 射撃対象がいる方向に向ける
        transform.LookAt(target.position);
    }

    // Update is called once per frame
    void Update()
    {
        // 向いている方向に進む
        transform.position += transform.forward * speed * Time.deltaTime;

        // nullチェック
        if (!target)
        {        
            return;
        }

        // 射撃対象までの向きベクトル
        Vector3 direction = target.position - transform.position;

        // 射撃対象までの角度
        float angleDiff = Vector3.Angle(transform.forward, direction);

        // 射撃対象の方向を向いたときのクォータニオン
        Quaternion rotateTarget = Quaternion.LookRotation(direction);

        // 角度が一定以下なら射撃対象に向かって回転させる
        if (angleDiff <= homingAngle)
        {
            // 追尾性能(割合)
            float homingRatio = homingPercent / percentMax;

            // 追尾性能(割合)分の回転をさせる
            transform.rotation = Quaternion.Slerp(transform.rotation, rotateTarget, homingRatio);
        }
    }

    /// <summary>
    /// 射撃対象用プロパティ
    /// </summary>
    public Transform Target
    {
        set { target = value; }
    }
}

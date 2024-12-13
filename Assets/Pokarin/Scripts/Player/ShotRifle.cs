using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotRifle : MonoBehaviour
{
    /// <summary> 弾用プレハブ </summary>
    [Header("弾用プレハブ")]
    [SerializeField] private RifleBullet bulletPrefab;

    /// <summary> 弾生成位置 </summary>
    [Header("弾生成位置")]
    [SerializeField] private Transform bulletSpawner;

    [Header("使用するバーチャルカメラ")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    /// <summary> プレイヤー用アニメーター </summary>
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // 右クリックで射撃
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // デバッグ用
            DebugLog();

            // 射撃用アニメーションを再生
            if (animator)
            {
                animator.SetTrigger("BeumRifleShot");
            }
        }
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

        if (!bulletSpawner)
        {
            Debug.Log("弾生成位置が設定されていません");
        }

        if (!animator)
        {
            Debug.Log("アニメーターが追加されていません");
        }
    }

    /// <summary>
    /// 射撃アニメーション開始の処理
    /// </summary>
    private void StartRifleShotAnim()
    {
        // 弾生成
        if (bulletPrefab && bulletSpawner)
        {
            RifleBullet bullet = Instantiate(bulletPrefab, bulletSpawner.position, Quaternion.identity);
            bullet.target = virtualCamera.LookAt;
        }
    }
}

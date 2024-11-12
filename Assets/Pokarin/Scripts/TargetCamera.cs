using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TargetCamera : MonoBehaviour
{
    /// <summary> バーチャルカメラ </summary>
    [Header("使用するバーチャルカメラ")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    /// <summary> 注視対象 </summary>
    [Header("注視対象")]
    [SerializeField] private Transform[] targetList;

    /// <summary> 注視対象の要素番号 </summary>
    private int targetIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        // nullチェック
        if (NullCheck())
        {
            return;
        }

        // 注視対象を初期化する
        virtualCamera.LookAt = targetList[0];
        targetIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // ----------------------------
        // nullチェック
        // ----------------------------

        if (NullCheck())
        {
            return;
        }

        // -------------------------------
        // 注視対象の変更
        // -------------------------------

        if (Input.GetKeyDown(KeyCode.T))
        {
            ChangeTarget();
        }
    }

    /// <summary>
    /// 注視対象を変更する
    /// </summary>
    void ChangeTarget()
    {
        // --------------------------
        // 要素番号の変更
        // --------------------------

        targetIndex++;

        if(targetIndex >= targetList.Length)
        {
            targetIndex = 0;
        }

        // -----------------------------
        // 注視対象の変更
        // -----------------------------

        virtualCamera.LookAt = targetList[targetIndex];
    }

    /// <summary>
    /// nullチェック用関数
    /// </summary>
    /// <returns> Nullならtrue </returns>
    bool NullCheck()
    {
        if (!virtualCamera)
        {
            return true;
        }

        if (targetList.Length == 0)
        {
            return true;
        }

        return false;
    }
}

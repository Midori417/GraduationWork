using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

// --------------------------------
// ※プレイヤー用
// --------------------------------

public class TargetSetter : MonoBehaviour
{
    /// <summary> バーチャルカメラ </summary>
    [Header("使用するバーチャルカメラ")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    /// <summary> 注視対象リスト </summary>
    private List<Transform> targetList;

    /// <summary> 注視対象の要素番号 </summary>
    private int targetIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        // 注視対象配列の初期化
        InitTargetList();

        // nullチェック
        if (NullCheck())
        {
            return;
        }

        // 注視対象を初期化する
        virtualCamera.LookAt = targetList[0].transform;
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
    private void ChangeTarget()
    {
        // --------------------------
        // 要素番号の変更
        // --------------------------

        targetIndex++;

        if (targetIndex >= targetList.Count)
        {
            targetIndex = 0;
        }

        // -----------------------------
        // 注視対象の変更
        // -----------------------------

        virtualCamera.LookAt = targetList[targetIndex].transform;
    }

    /// <summary>
    /// nullチェック用関数
    /// </summary>
    /// <returns> Nullならtrue </returns>
    private bool NullCheck()
    {
        if (!virtualCamera)
        {
            return true;
        }

        if (targetList.Count == 0)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 注視対象配列を初期化する
    /// </summary>
    private void InitTargetList()
    {
        // 注視対象配列の初期化
        targetList = new List<Transform>();

        // シーン内のMS
        GameObject[] msList = GameObject.FindGameObjectsWithTag("MS");

        foreach (var ms in msList)
        {
            // 自身以外なら注視対象に追加する
            if (ms != gameObject)
            {
                targetList.Add(ms.transform);
            }
        }
    }

    /// <summary>
    /// 注視対象用プロパティ
    /// </summary>
    public Transform Target
    {
        get
        {
            return virtualCamera.LookAt;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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
        // 注視対象リストを初期化する
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
    void ChangeTarget()
    {
        // --------------------------
        // 要素番号の変更
        // --------------------------

        targetIndex++;

        if(targetIndex >= targetList.Count)
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
    bool NullCheck()
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
    /// 注視対象リストを初期化する
    /// </summary>
    void InitTargetList()
    {
        // 初期化
        targetList = new List<Transform>();

        // シーン内のMS
        GameObject[] msList = GameObject.FindGameObjectsWithTag("MS");

        // 自身以外のMSを注視対象リストに追加する
        foreach (GameObject ms in msList)
        {
            // 自身なら何もしない
            if (ms == gameObject)
            {
                continue;
            }

            // 注視対象リストに追加する
            targetList.Add(ms.transform);
        }
    }
}

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラ管理コンポーネント
/// </summary>
public class CameraManager : MonoBehaviour
{
    [SerializeField, Header("メインカメラ")]
    private GameObject _mainCamera;

    // メインカメラ
    public GameObject mainCamera
    { get { return _mainCamera; } }

    // エネミー配列
    private List<Transform> enemys;

    [SerializeField, Header("仮想シネマカメラ")]
    CinemachineVirtualCamera virtualCamera;

    // とりつく機体
    Transform myMs;

    // 現在のターゲット
    Transform target;

    // 現在のターゲット番号
    int index = 0;

    /// <summary>
    /// 初期化
    /// パイロットで呼び出す
    /// </summary>
    public void Initialize(Transform _myMs, List<Transform> _enemys)
    {
        myMs = _myMs;
        enemys = _enemys;
    }

    /// <summary>
    /// 毎フレーム更新
    /// </summary>
    private void Update()
    {
        // エネミー配列がない
        if (enemys.Count <= 0)
            return;
        // とりつく機体がいない
        if (!myMs)
            return;
        // とりつく機体が設定されていなければ設定
        if (!virtualCamera.Follow)
        {
            virtualCamera.Follow = myMs;
        }

        // ターゲットが存在しなければ0を設定
        if (!target)
        {
            target = enemys[0];
        }
        else
        {
            virtualCamera.LookAt = target;
        }
    }

    /// <summary>
    /// ターゲットチェンジ
    /// パイロットで呼んでもらう
    /// </summary>
    public void TargetChange()
    {
        if (enemys.Count - 1 == index)
        {
            index = 0;
        }
        index++;
        target = enemys[index];
    }
}

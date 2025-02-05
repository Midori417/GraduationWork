using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラ管理コンポーネント
/// </summary>
public class CameraManager : BaseGameObject
{
    [SerializeField, Header("メインカメラ")]
    private Camera _mainCamera;

    [SerializeField, Header("仮想シネマカメラ")]
    CinemachineVirtualCamera _virtualCamera;

    // とりつく機体
    private Transform _myMs;

    // ターゲット機体
    private Transform _target;

    // 現在のターゲット番号
    int _index = 0;

    public Camera mainCamera => _mainCamera;
    public Transform myMs
    {
        set => _myMs = value;
    }
    public Transform target
    {
        set => _target = value;
    }

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    private void Update()
    {
        if (ProsessCheck())
            return;

        // とりつく機体が設定されていなければ設定
        if (!_virtualCamera.Follow)
            _virtualCamera.Follow = _myMs;

        if (isStop) return;

        if (_target) _virtualCamera.LookAt = _target;
    }

    /// <summary>
    /// trueなら処理可能
    /// </summary>
    /// <returns></returns>
    private bool ProsessCheck()
    {
        return (!_myMs) || (!_virtualCamera);
    }
}

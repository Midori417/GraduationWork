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
    private GameManager _mainCamera;

    [SerializeField, Header("仮想シネマカメラ")]
    CinemachineVirtualCamera _virtualCamera;

    // とりつく機体
    private Transform _myMs;

    // 敵機体配列
    private List<Transform> _enemys;

    // ターゲット機体
    private Transform _target;

    // 現在のターゲット番号
    int _index = 0;

    public GameManager mainCamera => _mainCamera;
    public Transform myMs
    {
        set => _myMs = value;
    }
    public List<Transform> enemys
    {
        set => _enemys = value;
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

        if(isStop) return;

        if (!_target)
            _target = _enemys[0];
        else _virtualCamera.LookAt = _target;
    }

    /// <summary>
    /// trueなら処理可能
    /// </summary>
    /// <returns></returns>
    private bool ProsessCheck()
    {
        return (!_myMs) || (_enemys.Count <= 0) || (!_virtualCamera);
    }

    /// <summary>
    /// ターゲットチェンジ
    /// パイロットで呼んでもらう
    /// </summary>
    public void TargetChange()
    {
        if (_enemys.Count - 1 == _index)
        {
            _index = 0;
        }
        _index++;
        _target = _enemys[_index];
    }

}

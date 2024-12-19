using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// パイロットの基底クラス
/// </summary>
public class BasePilot : MonoBehaviour
{
    [SerializeField, Header("自身のカメラ")]
    private CameraManager _myCameraManager;

    [SerializeField, Header("自身の機体")]
    private BaseMs _myMs;

    [SerializeField, Header("ターゲットパイロット")]
    private BasePilot targetPilot;

    public BaseMs myMs
    { get { return _myMs; } }

    public BaseMs taregetMs
    { get { return targetPilot.myMs; } }

    /// <summary>
    /// 初期化処理
    /// </summary>
    protected virtual void Initialize()
    {
        _myMs.SetMyCamera(_myCameraManager.mainCamera.transform);
        _myMs.SetTargetMs(taregetMs);

        _myMs.Initialize();
    }
}

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
}

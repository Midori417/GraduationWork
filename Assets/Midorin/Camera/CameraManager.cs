using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �J�����Ǘ��R���|�[�l���g
/// </summary>
public class CameraManager : MonoBehaviour
{
    [SerializeField, Header("���C���J����")]
    private GameObject _mainCamera;

    // ���C���J����
    public GameObject mainCamera
    { get { return _mainCamera; } }
}

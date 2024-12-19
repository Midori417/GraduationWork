using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �p�C���b�g�̊��N���X
/// </summary>
public class BasePilot : MonoBehaviour
{
    [SerializeField, Header("���g�̃J����")]
    private CameraManager _myCameraManager;

    [SerializeField, Header("���g�̋@��")]
    private BaseMs _myMs;

    [SerializeField, Header("�^�[�Q�b�g�p�C���b�g")]
    private BasePilot targetPilot;

    public BaseMs myMs
    { get { return _myMs; } }

    public BaseMs taregetMs
    { get { return targetPilot.myMs; } }

    /// <summary>
    /// ����������
    /// </summary>
    protected virtual void Initialize()
    {
        _myMs.SetMyCamera(_myCameraManager.mainCamera.transform);
        _myMs.SetTargetMs(taregetMs);

        _myMs.Initialize();
    }
}

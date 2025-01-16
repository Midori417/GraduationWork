using Cinemachine;
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

    // �G�l�~�[�z��
    private List<Transform> enemys;

    [SerializeField, Header("���z�V�l�}�J����")]
    CinemachineVirtualCamera virtualCamera;

    // �Ƃ���@��
    Transform myMs;

    // ���݂̃^�[�Q�b�g
    Transform target;

    // ���݂̃^�[�Q�b�g�ԍ�
    int index = 0;

    /// <summary>
    /// ������
    /// �p�C���b�g�ŌĂяo��
    /// </summary>
    public void Initialize(Transform _myMs, List<Transform> _enemys)
    {
        myMs = _myMs;
        enemys = _enemys;
    }

    /// <summary>
    /// ���t���[���X�V
    /// </summary>
    private void Update()
    {
        // �G�l�~�[�z�񂪂Ȃ�
        if (enemys.Count <= 0)
            return;
        // �Ƃ���@�̂����Ȃ�
        if (!myMs)
            return;
        // �Ƃ���@�̂��ݒ肳��Ă��Ȃ���ΐݒ�
        if (!virtualCamera.Follow)
        {
            virtualCamera.Follow = myMs;
        }

        // �^�[�Q�b�g�����݂��Ȃ����0��ݒ�
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
    /// �^�[�Q�b�g�`�F���W
    /// �p�C���b�g�ŌĂ�ł��炤
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

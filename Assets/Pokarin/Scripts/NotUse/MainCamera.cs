using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// -------------------------------------------------------------
// ���g�p�s��
// -------------------------------------------------------------

public class MainCamera : MonoBehaviour
{
    /// <summary> �v���C���[�̒��S </summary>
    [Header("�J�������Ǐ]����v���C���[�̒��S")]
    [SerializeField] private Transform playerCenter;

    /// <summary> �J�����R���|�[�l���g </summary>
    private Camera mainCamera;

    /// <summary> �v���C���[�܂ł̋����x�N�g�� </summary>
    private Vector3 difference;

    /// <summary> �X�e�[�W�p���C���[ </summary>
    private int stageLayer;

    // Start is called before the first frame update
    void Start()
    {
        // �J�����R���|�[�l���g�̎擾
        mainCamera = GetComponent<Camera>();

        // �X�e�[�W�p���C���[�̎擾
        stageLayer = LayerMask.NameToLayer("Stage");

        // �J�����R���|�[�l���g���Ȃ��ꍇ�̓G���[���o��
        if (!mainCamera)
        {
            Debug.LogError("�J�����R���|�[�l���g������܂���B\n" +
                "���̃X�N���v�g�̓J�����R���|�[�l���g���ǉ�����Ă���I�u�W�F�N�g�ɒǉ����Ă��������B");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // �J�����R���|�[�l���g���Ȃ��ꍇ�͉������Ȃ�
        if (!mainCamera)
        {
            return;
        }

        // �v���C���[�܂ł̋����x�N�g��
        difference = playerCenter.position - transform.position;

        // �v���C���[�܂ł̕���
        Vector3 direction = difference.normalized;

        // �v���C���[�Ɍ�����Ray
        Ray ray = new Ray(transform.position, direction);

        // �S�Ă̏�Q���̏��
        // �J�����ƃv���C���[�̊Ԃ�Ray�̏Փ˔�����s�����Ƃ�
        // �J�����ƃv���C���[�̊Ԃɏ�Q�����Ȃ������ׂ�
        RaycastHit[] hitInfoList = Physics.RaycastAll(ray, difference.magnitude);

        // ��Q���̃��C���[�𒲂ׂ�
        foreach (var hitInfo in hitInfoList)
        {
            // ��Q���ɃX�e�[�W���܂܂��Ȃ�
            // �X�e�[�W���\���ɂ��ďI������
            if (hitInfo.transform.gameObject.layer == stageLayer)
            {
                mainCamera.cullingMask &= ~(1 << stageLayer);
                break;
            }

            // �X�e�[�W���܂܂�Ȃ����ɃX�e�[�W���\�������悤�ɂ���
            mainCamera.cullingMask |= (1 << stageLayer);
        }
    }
}

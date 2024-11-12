using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TargetCamera : MonoBehaviour
{
    /// <summary> �o�[�`�����J���� </summary>
    [Header("�g�p����o�[�`�����J����")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    /// <summary> �����Ώ� </summary>
    [Header("�����Ώ�")]
    [SerializeField] private Transform[] targetList;

    /// <summary> �����Ώۂ̗v�f�ԍ� </summary>
    private int targetIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        // null�`�F�b�N
        if (NullCheck())
        {
            return;
        }

        // �����Ώۂ�����������
        virtualCamera.LookAt = targetList[0];
        targetIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // ----------------------------
        // null�`�F�b�N
        // ----------------------------

        if (NullCheck())
        {
            return;
        }

        // -------------------------------
        // �����Ώۂ̕ύX
        // -------------------------------

        if (Input.GetKeyDown(KeyCode.T))
        {
            ChangeTarget();
        }
    }

    /// <summary>
    /// �����Ώۂ�ύX����
    /// </summary>
    void ChangeTarget()
    {
        // --------------------------
        // �v�f�ԍ��̕ύX
        // --------------------------

        targetIndex++;

        if(targetIndex >= targetList.Length)
        {
            targetIndex = 0;
        }

        // -----------------------------
        // �����Ώۂ̕ύX
        // -----------------------------

        virtualCamera.LookAt = targetList[targetIndex];
    }

    /// <summary>
    /// null�`�F�b�N�p�֐�
    /// </summary>
    /// <returns> Null�Ȃ�true </returns>
    bool NullCheck()
    {
        if (!virtualCamera)
        {
            return true;
        }

        if (targetList.Length == 0)
        {
            return true;
        }

        return false;
    }
}

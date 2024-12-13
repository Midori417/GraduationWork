using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TargetSetter : MonoBehaviour
{
    /// <summary> �o�[�`�����J���� </summary>
    [Header("�g�p����o�[�`�����J����")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    /// <summary> �����Ώۃ��X�g </summary>
    private List<Transform> targetList;

    /// <summary> �����Ώۂ̗v�f�ԍ� </summary>
    private int targetIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        // �����Ώۃ��X�g������������
        InitTargetList();

        // null�`�F�b�N
        if (NullCheck())
        {
            return;
        }

        // �����Ώۂ�����������
        virtualCamera.LookAt = targetList[0].transform;
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

        if(targetIndex >= targetList.Count)
        {
            targetIndex = 0;
        }

        // -----------------------------
        // �����Ώۂ̕ύX
        // -----------------------------

        virtualCamera.LookAt = targetList[targetIndex].transform;
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

        if (targetList.Count == 0)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// �����Ώۃ��X�g������������
    /// </summary>
    void InitTargetList()
    {
        // ������
        targetList = new List<Transform>();

        // �V�[������MS
        GameObject[] msList = GameObject.FindGameObjectsWithTag("MS");

        // ���g�ȊO��MS�𒍎��Ώۃ��X�g�ɒǉ�����
        foreach (GameObject ms in msList)
        {
            // ���g�Ȃ牽�����Ȃ�
            if (ms == gameObject)
            {
                continue;
            }

            // �����Ώۃ��X�g�ɒǉ�����
            targetList.Add(ms.transform);
        }
    }
}

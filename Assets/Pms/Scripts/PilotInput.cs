using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �p�C���b�g�̓���
/// </summary>
public class PilotInput : MonoBehaviour
{
    // �ړ���
    public Vector2 moveAxis
    {
        get;
        private set;
    }

    // �W�����v�{�^��
    public bool isJumpBtn
    {
        get;
        private set;
    }

    // �_�b�V���{�^��
    public bool isDashBtn
    {
        get;
        private set;
    }

    // ���͂̍X�V
    void Update()
    {
        moveAxis = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        isJumpBtn = Input.GetKey(KeyCode.Space);
        isDashBtn = Input.GetKey(KeyCode.LeftShift);

    }
}

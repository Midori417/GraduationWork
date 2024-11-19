using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �K���_���̈ړ�
/// </summary>
public class GundomMove : BaseParts
{
    Rigidbody rb;

    private float additionalGravity;  //�d�͂̋���

    /// <summary>
    /// �ړ��p�����[�^
    /// </summary>
    [System.Serializable]
    private struct MoveParamater
    {
        [Header("�ړ����x")]
        public float speed;

        [Header("���񑬓x")]
        public float rotationSpeed;
    }
    [SerializeField, Header("�ړ��p�����[�^")]
    private MoveParamater moveParamater;
    public bool isMove
    {
        get;
        private set;
    }

    /// <summary>
    /// �W�����v�p�����[�^
    /// </summary>
    [System.Serializable]
    private struct JumpParamater
    {
        [Header("�W�����v��")]
        public float power;

        [Header("�ړ����x")]
        public float speed;

        [Header("���񑬓x")]
        public float rotationSpeed;

        [Header("�u�[�X�g�Q�[�W�̏����")]
        public float useBoost;

        [Header("�W�����v����")]
        public bool isNow;

        [Header("���n���̊���")]
        public float inertia;
    }
    [SerializeField, Header("�W�����v�p�����[�^")]
    private JumpParamater jumpParamater;
    public bool isJump
    {
        get
        {
            return jumpParamater.isNow;
        }
    }

    /// <summary>
    /// �_�b�V���p�����[�^
    /// </summary>
    [System.Serializable]
    private struct DashParamater
    {
        [Header("�ړ����x")]
        public float speed;

        [Header("���񑬓x")]
        public float rotationSpeed;

        [Header("�u�[�X�g�Q�[�W�̏����")]
        public float useBoost;

        [Header("�_�b�V������")]
        public bool isNow;
    }
    [SerializeField, Header("�_�b�V���p�����[�^")]
    private DashParamater dashParamater;
    public bool isDash
    {
        get
        {
            return dashParamater.isNow;
        }
    }

    void FixedUpdate()
    {
        // Y�������ɒǉ��̏d�͂�������
        rb.AddForce(Vector3.down * additionalGravity, ForceMode.Acceleration);
    }

    /// <summary>
    /// �X�e�[�^�X�̏�����
    /// </summary>
    public void Initalize()
    {
        GetBasePartsComponent();
        rb = GetComponent<Rigidbody>();
        if (!rb)
        {
            Debug.Log("Rigidbody�̎擾�Ɏ��s");
            return;
        }

        moveParamater.speed = 20.0f;
        moveParamater.rotationSpeed = 0.01f;

        jumpParamater.power = 25.0f;
        jumpParamater.speed = 5.0f;
        jumpParamater.rotationSpeed = 0.08f;
        jumpParamater.useBoost = 20;
        jumpParamater.inertia = 0.23f;

        dashParamater.speed = 40.0f;
        dashParamater.rotationSpeed = 0.12f;
        dashParamater.useBoost = 20;

        additionalGravity = 40.0f; //�d�͋���
        rb.drag = 0.1f;            //��C��R
    }

    /// <summary>
    /// ���n����
    /// </summary>
    public void Landing()
    {
        rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, jumpParamater.inertia);
    }

    /// <summary>
    /// �ړ�����
    /// </summary>
    public void Move(Vector2 moveAxis)
    {
        Vector3 moveFoward = MoveForward(moveAxis);

        // �i�s�����ɕ�Ԃ��Ȃ����]
        if (moveFoward != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(moveFoward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, moveParamater.rotationSpeed);
            rb.velocity = transform.forward * moveParamater.speed;
            isMove = true;
        }
        else
        {
            rb.velocity = Vector3.zero;
            isMove = false;
        }
    }

    /// <summary>
    /// �W�����v����
    /// </summary>
    public void Jump(Vector2 moveAxis, bool isJumpBtn)
    {
        if (isJumpBtn)
        {
            if (GetBoostCurrent() > 0)
            {
                Vector3 moveFoward = MoveForward(moveAxis);

                // �i�s�����ɕ�Ԃ��Ȃ����]
                if (moveFoward != Vector3.zero)
                {
                    Quaternion rotation = Quaternion.LookRotation(moveFoward);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, jumpParamater.rotationSpeed);
                    rb.velocity = transform.forward * jumpParamater.speed;
                }
                rb.velocity = new Vector3(rb.velocity.x, jumpParamater.power, rb.velocity.z);

                UseBoost(jumpParamater.useBoost);
                jumpParamater.isNow = true;
            }
            else
            {
                jumpParamater.isNow = false;
            }
        }
        else
        {
            jumpParamater.isNow = false;
        }
    }

    /// <summary>
    /// �_�b�V������
    /// </summary>
    public void Dash(Vector2 moveAxis, bool isDashBtn)
    {
        if (isDashBtn)
        {
            if (GetBoostCurrent() > 0)
            {
                Vector3 moveFoward = MoveForward(moveAxis);
                // �i�s�����ɕ�Ԃ��Ȃ����]
                if (moveFoward != Vector3.zero)
                {
                    Quaternion rotation = Quaternion.LookRotation(moveFoward);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, dashParamater.rotationSpeed);
                }
                rb.velocity = transform.forward * dashParamater.speed;

                UseBoost(dashParamater.useBoost);
                dashParamater.isNow = true;
            }
            else
            {
                dashParamater.isNow = false;
            }
        }
        else
        {
            dashParamater.isNow = false;
        }
    }
}

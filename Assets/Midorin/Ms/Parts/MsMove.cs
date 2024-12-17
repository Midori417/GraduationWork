using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ms�̈ړ�
/// </summary>
public class MsMove : BaseMsParts
{
    // ���g�̃J����
    private Transform myCamera;

    [SerializeField]
    private AudioSource se_AudioSource;

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

    // ���̂Ƃ���ɓ`����ϐ�
    public bool isJump
    {
        get
        {
            return jumpParamater.isNow;
        }
    }
    public bool isDash
    {
        get
        {
            return dashParamater.isNow;
        }
    }

    [SerializeField, Header("�u�[�X�g�g�p�J�n��")]
    private AudioClip se_boostStart;
    [SerializeField, Header("���n��")]
    private AudioClip se_landing;

    /// <summary>
    /// ��������
    /// </summary>
    public override bool Initalize()
    {
        base.Initalize();
        myCamera = mainMs.myCamera;

        return true;
    }

    /// <summary>
    /// ���n����
    /// </summary>
    public void Landing()
    {
        se_AudioSource.PlayOneShot(se_landing);
        rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, jumpParamater.inertia);
    }

    /// <summary>
    /// �J��������Ɉړ��������擾
    /// </summary>
    /// <param name="moveAxis">�ړ���</param>
    /// <returns></returns>
    Vector3 MoveForward(Vector2 moveAxis)
    {
        // �J�����̕�������AX-Z�P�ʃx�N�g��(���K��)���擾
        Vector3 cameraForward = Vector3.Scale(myCamera.forward, new Vector3(1, 0, 1));
        Vector3 moveForward = cameraForward * moveAxis.y + myCamera.right * moveAxis.x;

        return moveForward;
    }

    /// <summary>
    /// �i�s�����ɉ�]
    /// </summary>
    void MoveForwardRot(Vector3 moveForward, float rotSpeed)
    {
        Quaternion rotation = Quaternion.LookRotation(moveForward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotSpeed * Time.deltaTime);
    }

    /// <summary>
    /// �ړ�����
    /// </summary>
    public void Move(Vector2 moveAxis)
    {
        if (isDash)
        {
            return;
        }

        // �i�s�����ɉ�]���Ȃ��琳�ʕ����ɐi��
        if (moveAxis != Vector2.zero)
        {
            Vector3 moveFoward = MoveForward(moveAxis);
            MoveForwardRot(moveFoward, moveParamater.rotationSpeed);
            rb.velocity = transform.forward * moveParamater.speed + new Vector3(0, rb.velocity.y, 0);
        }
        else
        {
            // �ړ����͂��Ȃ��Ȃ����瑬�x���Ȃ���
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    /// <summary>
    /// �W�����v����
    /// </summary>
    public void Jump(Vector2 moveAxis, bool isJumpBtn)
    {
        if (isJumpBtn)
        {
            if (mainMs.boost01 > 0)
            {
                Vector3 moveFoward = MoveForward(moveAxis);

                // �i�s�����ɕ�Ԃ��Ȃ����]
                if (moveFoward != Vector3.zero)
                {
                    MoveForwardRot(moveFoward, jumpParamater.rotationSpeed);
                    rb.velocity = transform.forward * moveParamater.speed + new Vector3(0, rb.velocity.y, 0);
                }
                rb.velocity = new Vector3(rb.velocity.x, jumpParamater.power, rb.velocity.z);

                // �J�n���Ɉ�x�����T�E���h
                if(!jumpParamater.isNow)
                { }

                jumpParamater.isNow = true;

                // �G�l���M�[�̎g�p
                mainMs.UseBoost(jumpParamater.useBoost);
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
            if (mainMs.boost01 > 0)
            {
                Vector3 moveFoward = MoveForward(moveAxis);
                // �i�s�����ɕ�Ԃ��Ȃ����]
                if (moveFoward != Vector3.zero)
                {
                    MoveForwardRot(moveFoward, dashParamater.rotationSpeed);
                }
                rb.velocity = transform.forward * dashParamater.speed;

                dashParamater.isNow = true;

                // �G�l���M�[�̎g�p
                mainMs.UseBoost(dashParamater.useBoost);
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

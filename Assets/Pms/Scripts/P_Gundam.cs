using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �K���_��
/// </summary>
public class P_Gundam : P_BaseMs
{
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
    private float additionalGravity;  //�d�͂̋���

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

    [SerializeField, Header("�^�[�Q�b�g�@��")]
    private GameObject target;

    /// <summary>
    /// Update�̑O�Ɏ��s
    /// </summary>
    private void Start()
    {
        // �K�v�ȃR���|�[�l���g���擾����
        // �R���|�[�l���g������Ȃ���΃X�N���v�g���~����
        if (!UseGetComponent())
        {
            enabled = false;
        }

        Initialize();
    }

    /// <summary>
    /// �K���_���̋@�̃p�����[�^�̏�����
    /// </summary>
    private void Initialize()
    {
        maxHp = 600;
        hp = maxHp;
        strengthValue = 2000;

        BoostGaugeInit();

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
        //rb.angularDrag = 0.05f;  //��]�̋�C��R
    }

    /// <summary>
    /// ���t���[�����s
    /// </summary>
    private void Update()
    {
        UseGravity();
        BoostGaugeChage();

        if (!olsIsGround && isGround)
        {
            anim.SetTrigger("Ground");
            isStop = true;
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, jumpParamater.inertia);
        }
        if (isStop)
        {
            return;
        }

        Move(pilotInput.moveAxis);
        Jump(pilotInput.moveAxis, pilotInput.isJumpBtn);
        Dash(pilotInput.moveAxis, pilotInput.isDashBtn);

        // �u�[�X�g�p�����[�^��␳
        boostParamater.current = Mathf.Clamp(boostParamater.current, 0, boostParamater.max);

        // �G�t�F�N�g�̊J�n�ƒ�~
        if (jumpParamater.isNow || dashParamater.isNow)
        {
            PlayEffect();
        }
        else if (!jumpParamater.isNow && !dashParamater.isNow)
        {
            StopEffect();
        }
    }

    void FixedUpdate()
    {
        // Y�������ɒǉ��̏d�͂�������
        rb.AddForce(Vector3.down * additionalGravity, ForceMode.Acceleration);
    }

    /// <summary>
    /// �d�͂��K�v��
    /// </summary>
    void UseGravity()
    {
        if (dashParamater.isNow)
        {
            rb.useGravity = false;
        }
        else
        {
            rb.useGravity = true;
        }
    }

    /// <summary>
    /// �ړ�����
    /// </summary>
    private void Move(Vector2 moveAxis)
    {
        if (!isGround)
        {
            anim.SetBool("Move", false);
            return;
        }

        Vector3 moveFoward = MoveForward(moveAxis);

        // �i�s�����ɕ�Ԃ��Ȃ����]
        if (moveFoward != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(moveFoward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, moveParamater.rotationSpeed);
            rb.velocity = transform.forward * moveParamater.speed;
            anim.SetBool("Move", true);
        }
        else
        {
            rb.velocity = Vector3.zero;
            anim.SetBool("Move", false);
        }
    }

    /// <summary>
    /// �W�����v����
    /// </summary>
    private void Jump(Vector2 moveAxis, bool isJump)
    {
        if (isJump)
        {
            if (boostParamater.current > 0)
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

                UseBoostGauge(jumpParamater.useBoost);
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

        anim.SetBool("Jump", jumpParamater.isNow);
    }

    /// <summary>
    /// �_�b�V������
    /// </summary>
    private void Dash(Vector2 moveAxis, bool isDash)
    {
        if (isDash)
        {
            if (boostParamater.current > 0)
            {
                Vector3 moveFoward = MoveForward(moveAxis);
                // �i�s�����ɕ�Ԃ��Ȃ����]
                if (moveFoward != Vector3.zero)
                {
                    Quaternion rotation = Quaternion.LookRotation(moveFoward);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, dashParamater.rotationSpeed);
                }
                rb.velocity = transform.forward * dashParamater.speed;

                UseBoostGauge(dashParamater.useBoost);
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

        anim.SetBool("Dash", dashParamater.isNow);
    }

    /// <summary>
    /// �_���[�W���󂯂�
    /// �U�����鑤�ɌĂяo���Ă��炤
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(int damage)
    {
        hp -= damage;
    }
}

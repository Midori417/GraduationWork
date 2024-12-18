using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GundamRifleShot : BaseMsParts
{
    /// <summary> �e�p�v���n�u </summary>
    [Header("�e�p�v���n�u")]
    [SerializeField] private RifleBullet bulletPrefab;

    /// <summary> �e�����ʒu </summary>
    [Header("�e�����ʒu")]
    [SerializeField] private Vector3 shotPos;

    // ���݂̒e
    private int amo;

    [SerializeField, Header("�ő�e")]
    private int amoMax;

    [SerializeField, Header("���˃C���^�[�o��")]
    private float interval;

    [SerializeField, Header("���{�[��")]
    private Transform spineBone;

    // �^�[�Q�b�g
    private Transform target;

    // true�Ȃ�s����
    private bool isNow = false;

    // true�Ȃ猳�̊p�x�ɖ߂�
    private bool returnRotaion = false;

    // ��]���x
    private float rotationSpeed = 0;

    // �^�[�Q�b�g�̊p�x
    private Quaternion targetRot = Quaternion.identity;

    // ������]��ۑ�����ϐ�
    private Quaternion initialRotation;

    // �o�b�N�V���b�g
    public bool isBackShot
    { get; private set; }

    #region �C�x���g

    private void Update()
    {
        if (mainMs)
        {
            mainMs.uiArmedValue[0] = amo;
        }
    }

    private void LateUpdate()
    {
        // �s����
        if (isNow)
        {
            if (!isBackShot)
            {
                SpineLookRotation();
            }
            else
            {
                BackLookRotaion();
            }
        }
    }

    #endregion

    /// <summary>
    /// ������
    /// </summary>
    /// <returns>
    /// true    ����������
    /// faklse  ���������s
    /// </returns>
    public override bool Initalize()
    {
        if (!base.Initalize())
        {
            return false;
        }
        mainMs.uiArmedValue.Add(amo);

        // ������]��ۑ�
        if (spineBone != null)
        {
            initialRotation = spineBone.localRotation;
        }

        amo = amoMax;

        return true;
    }

    /// <summary>
    /// �㔼�g���^�[�Q�b�g�̕����Ɍ�����
    /// </summary>
    void SpineLookRotation()
    {
        if (!returnRotaion)
        {
            if (target)
            {
                Vector3 directionToTarget = target.position - spineBone.position;
                Vector3 localDirection = spineBone.parent.InverseTransformDirection(directionToTarget);

                // �^�[�Q�b�g�����̉�]���v�Z
                Quaternion targetRotation = Quaternion.LookRotation(localDirection);
                targetRot = targetRotation;

                // ���݂̉�]����^�[�Q�b�g��]�ւ̕⊮
                rotationSpeed += 5 * Time.deltaTime;
                rotationSpeed = Mathf.Clamp01(rotationSpeed);
                Quaternion smoothRotation = Quaternion.Slerp(spineBone.localRotation, targetRotation, rotationSpeed);

                // ��]��K�p
                spineBone.localRotation = smoothRotation;
            }
        }
        else
        {
            rotationSpeed += 5 * Time.deltaTime;
            rotationSpeed = Mathf.Clamp01(rotationSpeed);
            Quaternion smoothRotation = Quaternion.Slerp(targetRot, initialRotation, rotationSpeed);
            spineBone.localRotation = smoothRotation;
        }
    }

    /// <summary>
    /// �^�[�Q�b�g�̋t�����Ɍ�����
    /// </summary>
    void BackLookRotaion()
    {
        // �^�[�Q�b�g�����̉�]���v�Z
        Vector3 directionToTarget = Vector3.Scale(transform.position - target.position, new Vector3(1, 0, 1));
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = targetRotation;
        rb.velocity = Vector3.zero;
    }

    /// <summary>
    /// �e�����˂ł��邩�`�F�b�N
    /// </summary>
    /// <returns>
    /// false ���˕s��
    /// true ���ˉ\
    /// </returns>
    public bool ShotCheck()
    {
        // �ˌ��s����
        // �ˌ��s�� �e��0�ȉ� �C���^�[�o����0�ȏ�
        if (isNow || amo <= 0)
        {
            return false;
        }

        // �^�[�Q�b�g���w��
        if (mainMs.targetMs)
        {
            target = mainMs.targetMs.center;
            // ��]���x�����Z�b�g
            rotationSpeed = 0;
        }

        // �o�b�N�V���b�g������
        {
            // �^�[�Q�b�g�������v�Z
            Vector3 directioToTarget = transform.position - target.position;
            float dot = Vector3.Dot(directioToTarget, transform.forward);
            if (dot > 0)
            {
                isBackShot = true;
            }
        }

        // �ˌ��s����
        isNow = true;

        // �ˌ�
        return true;
    }

    /// <summary>
    /// �e�𐶐�
    /// �A�j���[�V�����C�x���g�ŌĂяo��
    /// </summary>
    public void CreateBullet()
    {
        // �e����
        if (bulletPrefab)
        {
            if (target)
            {
                // ���g�̋@�̂̃Z���^�[����
                Transform center = mainMs.center;

                // �^�[�Q�b�g�̕������v�Z
                Vector3 directionToTarget = target.position - center.position;
                Quaternion rot = Quaternion.LookRotation(directionToTarget);
                // �e�̐����ʒu���v�Z
                Vector3 pos = center.position + rot * shotPos;

                // �e�𐶐�
                RifleBullet bullet = Instantiate(bulletPrefab, pos, rot);
                bullet.Target = target;
            }
            else
            {
                // ���g�̋@�̂̃Z���^�[����
                Transform center = mainMs.center;
                Quaternion rot = transform.rotation;
                Vector3 pos = center.position + rot * shotPos;
                RifleBullet bullet = Instantiate(bulletPrefab, pos, rot);
            }
            // �e�����炷
            amo--;
        }
    }

    /// <summary>
    /// ���̊p�x�ɖ߂�
    /// �A�j���[�V�����ŌĂяo��
    /// </summary>
    public void ReturnRotation()
    {
        returnRotaion = true;
        rotationSpeed = 0;
    }

    /// <summary>
    /// �I������
    /// </summary>
    public void Failed()
    {
        returnRotaion = false;
        target = null;
        isBackShot = false;
        isNow = false;
    }
}

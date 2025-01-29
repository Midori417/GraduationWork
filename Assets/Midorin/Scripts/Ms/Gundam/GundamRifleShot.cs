using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �K���_���̃r�[�����C�t������
/// </summary>
public class GundamRifleShot : BaseMsAmoParts
{
    /// <summary> �e�p�v���n�u </summary>
    [Header("�e�p�v���n�u")]
    [SerializeField] private RifleBullet bulletPrefab;

    /// <summary> �e�����ʒu </summary>
    [Header("�e�����ʒu")]
    [SerializeField] private Vector3 shotPos;

    [SerializeField, Header("�C���^�[�o��")]
    private float interval;

    // true�Ȃ�ˌ��\
    private bool isShotOk = true;

    [SerializeField, Header("���{�[��")]
    private Transform spineBone;

    // �^�[�Q�b�g
    private Transform target;

    // true�Ȃ猳�̊p�x�ɖ߂�
    private bool returnRotaion = false;

    [SerializeField, Header("��]���x")]
    private float rotationSpeed = 0;

    // ������]��ۑ�����ϐ�
    private Quaternion initialRotation;

    // �O��t���[�����̉�]
    private Quaternion oldRotation;

    [SerializeField, Header("�ꔭ�̒e�������[�h�����܂ł̎���")]
    private float reloadTime = 0;

    // �����[�h�^�C�}�[
    private float reloadTimer = 0;

    // �o�b�N�V���b�g
    public bool isBackShot
    { get; private set; }

    public bool isNow
    { get; private set; }

    #region �C�x���g

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
        ReloadProsess();
    }

    #endregion

    /// <summary>
    /// �����[�h����
    /// </summary>
    private void ReloadProsess()
    {
        if (amo < amoMax && reloadTimer <= 0)
        {
            reloadTimer = reloadTime;
        }
        if(reloadTimer > 0)
        {
            reloadTimer -= Time.deltaTime;
            if(reloadTimer<=0)
            {
                reloadTimer = 0;
                amo++;
            }
        }
    }

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

        // ������]��ۑ�
        if (spineBone != null)
        {
            initialRotation = spineBone.localRotation;
        }

        amo = amoMax;
        isNow = false;
        isShotOk = true;
        isBackShot = false;

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

                // ���݂̉�]����^�[�Q�b�g��]�ւ̕⊮
                Quaternion smoothRotation = Quaternion.Slerp(oldRotation, targetRotation, rotationSpeed * Time.deltaTime);

                // ��]��K�p
                spineBone.localRotation = smoothRotation;
                oldRotation = spineBone.localRotation;
            }
        }
        else
        {
            Quaternion smoothRotation = Quaternion.Slerp(oldRotation, initialRotation, rotationSpeed * Time.deltaTime);
            spineBone.localRotation = smoothRotation;
            oldRotation = spineBone.localRotation;
        }
    }

    /// <summary>
    /// �^�[�Q�b�g�̋t�����Ɍ�����
    /// </summary>
    void BackLookRotaion()
    {
        if (!target)
        {
            return;
        }
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
        if (!isShotOk || amo <= 0)
        {
            return false;
        }

        // �^�[�Q�b�g���w��
        if (mainMs.targetMs)
        {
            target = mainMs.targetMs.center;
            oldRotation = spineBone.localRotation;
        }

        // �o�b�N�V���b�g������
        if (target)
        {
            // �^�[�Q�b�g�������v�Z
            Vector3 directioToTarget = transform.position - target.position;
            float dot = Vector3.Dot(directioToTarget.normalized, transform.forward);
            if (dot > 0.5f)
            {
                isBackShot = true;
            }
        }

        // �ˌ��s����
        isNow = true;
        isShotOk = false;

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
        Invoke("Interval", interval);
    }

    /// <summary>
    /// �C���^�[�o������
    /// </summary>
    void Interval()
    {
        isShotOk = true;
    }
}

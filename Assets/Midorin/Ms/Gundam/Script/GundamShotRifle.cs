using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GundamShotRifle : MonoBehaviour
{
    // ���C���R���|�l���g
    private Gundam mainMs;

    /// <summary> �e�p�v���n�u </summary>
    [Header("�e�p�v���n�u")]
    [SerializeField] private BeumRifleBullet bulletPrefab;

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

    private Transform center;

    // �^�C�}�[
    private float shotTimer;

    // true�Ȃ�ˌ��\
    private bool isShotOk = true;

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

    /// <summary>
    /// ��������
    /// </summary>
    public void Initalize()
    {
        mainMs = GetComponent<Gundam>();
        if (!mainMs)
        {
            Debug.LogError("���C���R���|�[�l���g���擾�ł��܂���");
            return;
        }
        DebugLog();
        mainMs.uiArmedValue.Add(amo);

        // ������]��ۑ�
        if (spineBone != null)
        {
            initialRotation = spineBone.localRotation;
        }

        center = mainMs.center;
        amo = amoMax;
    }

    /// <summary>
    /// �f�o�b�O�p
    /// </summary>
    private void DebugLog()
    {
        if (!bulletPrefab)
        {
            Debug.Log("�e�p�v���n�u���ݒ肳��Ă��܂���");
        }
    }

    private void Update()
    {
        if(mainMs)
        {
            mainMs.uiArmedValue[0] = amo;
        }
        if (shotTimer > 0)
        {
            shotTimer -= Time.deltaTime;
            if (shotTimer <= 0)
            {
                shotTimer = 0;
            }
        }
    }

    private void LateUpdate()
    {
        // �s����
        if (isNow)
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
        if (!isShotOk || amo <= 0 || shotTimer > 0)
        {
            return false;
        }

        // �o�O���Ă�̂ŋ~�ς����i���Ƃł��������j
        if(isNow)
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

        isShotOk = false;
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
                // �^�[�Q�b�g�̕������v�Z
                Vector3 directionToTarget = target.position - center.position;
                Quaternion rot = Quaternion.LookRotation(directionToTarget);
                // �e�̐����ʒu���v�Z
                Vector3 pos = center.position + rot * shotPos;

                // �e�𐶐�
                BeumRifleBullet bullet = Instantiate(bulletPrefab, pos, rot);
            }
            else
            {
                BeumRifleBullet bullet = Instantiate(bulletPrefab, spineBone);
            }
            // �ˌ����\��
            isShotOk = true;
            // �C���^�[�o����ݒ�
            shotTimer = interval;
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
        isNow = false;
    }
}

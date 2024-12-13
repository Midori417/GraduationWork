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

    // �^�C�}�[
    private float shotTimer;

    // �ˌ���
    private bool isShot;

    private bool returnRotaion;

    float rotationSpeed;

    // ������]��ۑ�����ϐ�
    [SerializeField]
    private Quaternion initialRotation;

    Quaternion targetRot;

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

        // ������]��ۑ�
        if (spineBone != null)
        {
            initialRotation = spineBone.localRotation;
        }

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
        if (shotTimer > 0)
        {
            shotTimer -= Time.deltaTime;
            if (shotTimer <= 0)
            {
                shotTimer = 0;
            }
        }
    }

    #region ��U
    /// <summary>
    /// 
    /// </summary>
    private void LateUpdate()
    {
        // �ˌ����
        if (isShot)
        {
            if (!returnRotaion)
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
            else
            {
                rotationSpeed += 5 * Time.deltaTime;
                rotationSpeed = Mathf.Clamp01(rotationSpeed);
                Quaternion smoothRotation = Quaternion.Slerp(targetRot, initialRotation, rotationSpeed);
                spineBone.localRotation = smoothRotation;
            }
        }
    }

    #endregion

    /// <summary>
    /// �e�����˂ł��邩�`�F�b�N
    /// </summary>
    /// <returns>
    /// false ���˕s��
    /// true ���ˉ\
    /// </returns>
    public bool ShotCheck()
    {
        // �e���[���ȉ�
        if (amo <= 0)
        {
            return false;
        }
        // ���˃^�C�}�[���[���ȏ�
        if (shotTimer > 0)
        {
            return false;
        }
        if (isShot)
        {
            return false;
        }

        // �^�[�Q�b�g���w��
        if (mainMs.targetMs)
        {
            target = mainMs.targetMs.transform;
        }

        isShot = true;
        rotationSpeed = 0;
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
                Vector3 directionToTarget = target.position - transform.position;
                Quaternion rot = Quaternion.LookRotation(directionToTarget);
                // �e�̐����ʒu���v�Z
                Vector3 pos = transform.position + rot * shotPos;

                // �e�𐶐�
                BeumRifleBullet bullet = Instantiate(bulletPrefab, pos, rot);
            }
            else
            {
                BeumRifleBullet bullet = Instantiate(bulletPrefab, spineBone);
            }
            // �e�����炷
            amo--;
            shotTimer = interval;
        }
    }

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
        isShot = false;
    }
}

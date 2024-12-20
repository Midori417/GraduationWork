using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �K���_���o�Y�[�J
/// </summary>
public class GundamBazookaShot : BaseMsAmoParts
{
    [SerializeField]
    private RifleBullet bulletPrefab;

    [SerializeField]
    private Vector3 shotPos;

    // �s������
    private bool isNow = false;

    // �^�[�Q�b�g
    private Transform target;

    private void LateUpdate()
    {
        if(isNow)
        {
            LookRotaion();
        }
    }

    /// <summary>
    /// �^�[�Q�b�g�̕����Ɍ�����
    /// </summary>
    void LookRotaion()
    {
        if (!target)
        {
            return;
        }
        // �^�[�Q�b�g�����̉�]���v�Z
        Vector3 directionToTarget = Vector3.Scale(target.position - transform.position, new Vector3(1, 0, 1));
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = targetRotation;
        rb.velocity = Vector3.zero;
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

        amo = amoMax;

        return true;
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
        }

        // �ˌ��s����
        isNow = true;

        // �ˌ�
        return true;
    }

    /// <summary>
    /// �e�𐶐�����
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
    /// �I������
    /// </summary>
    public void Failed()
    {
        isNow = false;
    }
}

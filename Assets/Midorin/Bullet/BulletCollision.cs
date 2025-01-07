using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    [SerializeField, Header("�^����_���[�W")]
    private int damage;

    [SerializeField, Header("�_�E���l")]
    private float downValue;

    [SerializeField, Header("�Փ˂����Ƃ��Ɏ��g��j�󂷂邩")]
    private bool isDead = false;

    [SerializeField, Header("�Փˎ��̃G�t�F�N�g")]
    private GameObject pfb_eff_hit;

    /// <summary>
    /// �Փ˂����Ƃ��ɏ���
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        // �G�t�F�N�g�𐶐�
        if(pfb_eff_hit)
        {
            Instantiate(pfb_eff_hit, transform.position, transform.rotation);
        }

        // �@�̂ɏՓ˂�����_���[�W��^����
        if (other.gameObject.tag == "MS")
        {
            other.GetComponent<BaseMs>().Damage(damage, transform.position);
            if(isDead)
            {
                Destroy(gameObject);
            }
        }

        // �����ɏՓ˂����玩�g��j��
        if(other.gameObject.tag == "Ground" || other.gameObject.tag == "Building")
        {
            Destroy(gameObject);
        }
    }
}

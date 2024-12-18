using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    [SerializeField, Header("�^����_���[�W")]
    private int damage;

    /// <summary>
    /// �Փ˂����Ƃ��ɏ���
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MS")
        {
            other.GetComponent<BaseMs>().Damage(damage);
        }

    }
}

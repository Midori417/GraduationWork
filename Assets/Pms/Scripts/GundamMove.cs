using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GundomMove : MonoBehaviour
{
    public float moveSpeed = 5f; //�ړ����x

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        //���͒l�Ɋ�Â��Ĉړ��x�N�g�����v�Z
        Vector3 move = new Vector3(moveX, 0, moveZ) * moveSpeed * Time.deltaTime;

        //�v���C���[�̈ʒu���X�V
        transform.Translate(move, Space.World);
    }
}

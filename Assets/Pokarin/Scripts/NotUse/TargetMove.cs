using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// -------------------------------------
// ���G�ړ��e�X�g�p
// ��Pokarin�ȊO�A�g�p�s��
// -------------------------------------

public class TargetMove : MonoBehaviour
{
    [Header("�ړ����x")]
    [SerializeField] private float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;

        if(Input.GetKey(KeyCode.J))
        {
            position.x -= speed * Time.deltaTime;
        }

        if(Input.GetKey(KeyCode.L))
        {
            position.x += speed * Time.deltaTime;
        }
        
        if(Input.GetKey(KeyCode.U))
        {
            position.y -= speed * Time.deltaTime;
        }

        if(Input.GetKey(KeyCode.O))
        {
            position.y += speed * Time.deltaTime;
        }

        transform.position = position;
    }
}

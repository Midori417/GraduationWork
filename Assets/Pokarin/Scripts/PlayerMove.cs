using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    [Header("�ړ����x")]
    [SerializeField] private float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // �v���C���[�̈ʒu
        Vector3 position = transform.position;

        if (Input.GetKey(KeyCode.W))
        {
            position.z += moveSpeed * Time.deltaTime;
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            position.z -= moveSpeed * Time.deltaTime;
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            position.x += moveSpeed * Time.deltaTime;
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            position.x -= moveSpeed * Time.deltaTime;
        }
        
        if (Input.GetKey(KeyCode.E))
        {
            position.y += moveSpeed * Time.deltaTime;
        }
        
        if (Input.GetKey(KeyCode.Q))
        {
            position.y -= moveSpeed * Time.deltaTime;
        }

        // �ʒu�X�V
        transform.position = position;
    }
}

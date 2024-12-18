using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMove : MonoBehaviour
{
    [Header("ˆÚ“®‘¬“x")]
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

        transform.position = position;
    }
}

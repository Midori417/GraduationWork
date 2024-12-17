using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RifleBullet : MonoBehaviour
{
    /// <summary> ���x </summary>
    [Header("���x")]
    [SerializeField] private float speed;

    [Header("�ˌ�����e��������܂ł̎���")]
    [SerializeField] private float destroyTime;

    /// <summary> �ˌ��Ώ� </summary>
    [NonSerialized] public Transform target;

    private Vector3 position;

    private float maxAcceleration = 100.0f;

    private Vector3 velocity;

    private float period = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        // ��莞�Ԍ�ɍ폜
        Destroy(gameObject, destroyTime);

        // �Ώۂ��������
        transform.LookAt(target.position);
    }

    // Update is called once per frame
    void Update()
    {
        var acceleration = Vector3.zero;
        var diff = target.position - position;

        //���xvelocity�̕��̂�period�b���diff�i�ނ��߂̉����x
        acceleration += (diff - velocity * period) * 2f / (period * period);

        if (acceleration.magnitude > maxAcceleration)
        {
            acceleration = acceleration.normalized * maxAcceleration;
        }

        period -= Time.deltaTime;

        if (period < 0f)
            return;

        velocity += acceleration * Time.deltaTime;
        position += velocity * Time.deltaTime;
        transform.position = position;

    }
}

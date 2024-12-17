using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RifleBullet : MonoBehaviour
{
    /// <summary> 速度 </summary>
    [Header("速度")]
    [SerializeField] private float speed;

    [Header("射撃から弾が消えるまでの時間")]
    [SerializeField] private float destroyTime;

    /// <summary> 射撃対象 </summary>
    [NonSerialized] public Transform target;

    private Vector3 position;

    private float maxAcceleration = 100.0f;

    private Vector3 velocity;

    private float period = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        // 一定時間後に削除
        Destroy(gameObject, destroyTime);

        // 対象がいる方向
        transform.LookAt(target.position);
    }

    // Update is called once per frame
    void Update()
    {
        var acceleration = Vector3.zero;
        var diff = target.position - position;

        //速度velocityの物体がperiod秒後にdiff進むための加速度
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

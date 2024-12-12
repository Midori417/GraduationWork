using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleBullet : MonoBehaviour
{
    /// <summary> ‘¬“x </summary>
    [Header("‘¬“x")]
    [SerializeField] private float speed;

    /// <summary> ËŒ‚‘ÎÛ </summary>
    [NonSerialized] public Transform target;

    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3);

        // ‘ÎÛ‚ª‚¢‚é•ûŒü
        transform.LookAt(target.position);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}

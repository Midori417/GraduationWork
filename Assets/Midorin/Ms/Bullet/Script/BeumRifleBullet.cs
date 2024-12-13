using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeumRifleBullet : MonoBehaviour
{
    /// <summary> ë¨ìx </summary>
    [Header("ë¨ìx")]
    [SerializeField] private float speed;

    /// <summary> éÀåÇëŒè€ </summary>
    [SerializeField]
    private Transform target;

    void Start()
    {
        transform.parent = null;
        Destroy(gameObject, 3);
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }
}

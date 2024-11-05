using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GundomMove : MonoBehaviour
{

    private float input_Hori;
    private float input_ver;

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            Debug.Log("デバッグ");
        }
    }
}

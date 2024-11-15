using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GundomMove : MonoBehaviour
{
    public float moveSpeed = 5f; //移動速度

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        //入力値に基づいて移動ベクトルを計算
        Vector3 move = new Vector3(moveX, 0, moveZ) * moveSpeed * Time.deltaTime;

        //プレイヤーの位置を更新
        transform.Translate(move, Space.World);
    }
}

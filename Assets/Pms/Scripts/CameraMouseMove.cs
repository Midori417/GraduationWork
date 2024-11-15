using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マウスでカメラを動かす
/// </summary>
public class CameraMouseMove : MonoBehaviour
{
    [SerializeField, Header("追尾オブジェクト")]
    private GameObject stokerObject;

    [SerializeField, Header("回転速度")]
    private Vector2 rotationSpeed;

    //最後の追尾オブジェクトの座標
    private Vector3 lastTargetPosition;     

    /// <summary>
    /// 最初に実行
    /// </summary>
    void Start()
    {
        lastTargetPosition = stokerObject.transform.position;
    }

    /// <summary>
    /// 毎フレーム実行
    /// </summary>
    void Update()
    {
        if (!stokerObject)
        {
            Debug.LogError("追尾オブジェクトがありません");
            return;
        }

        Translate();
        Rotate();
    }

    /// <summary>
    /// 追尾オブジェクトに合わせて位置を変える
    /// </summary>
    void Translate()
    {
        transform.position += stokerObject.transform.position - lastTargetPosition;
        lastTargetPosition = stokerObject.transform.position;
    }

    /// <summary>
    /// マウスの移動量に合わせて回転
    /// </summary>
    void Rotate()
    {
        Vector2 newAngle;
        newAngle.x = rotationSpeed.x * Input.GetAxis("Mouse X");
        newAngle.y = rotationSpeed.y * Input.GetAxis("Mouse Y");

        transform.RotateAround(stokerObject.transform.position, Vector3.up, newAngle.x);
        transform.RotateAround(stokerObject.transform.position, transform.right, -newAngle.y);
    }
}

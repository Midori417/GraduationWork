using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �}�E�X�ŃJ�����𓮂���
/// </summary>
public class CameraMouseMove : MonoBehaviour
{
    [SerializeField, Header("�ǔ��I�u�W�F�N�g")]
    private GameObject stokerObject;

    [SerializeField, Header("��]���x")]
    private Vector2 rotationSpeed;

    //�Ō�̒ǔ��I�u�W�F�N�g�̍��W
    private Vector3 lastTargetPosition;     

    /// <summary>
    /// �ŏ��Ɏ��s
    /// </summary>
    void Start()
    {
        lastTargetPosition = stokerObject.transform.position;
    }

    /// <summary>
    /// ���t���[�����s
    /// </summary>
    void Update()
    {
        if (!stokerObject)
        {
            Debug.LogError("�ǔ��I�u�W�F�N�g������܂���");
            return;
        }

        Translate();
        Rotate();
    }

    /// <summary>
    /// �ǔ��I�u�W�F�N�g�ɍ��킹�Ĉʒu��ς���
    /// </summary>
    void Translate()
    {
        transform.position += stokerObject.transform.position - lastTargetPosition;
        lastTargetPosition = stokerObject.transform.position;
    }

    /// <summary>
    /// �}�E�X�̈ړ��ʂɍ��킹�ĉ�]
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

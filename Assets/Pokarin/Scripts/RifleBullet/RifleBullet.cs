using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RifleBullet : MonoBehaviour
{
    /// <summary> ���x </summary>
    [Header("���x")]
    [SerializeField] private float speed;

    /// <summary> �ˌ�����e��������܂ł̎��� </summary>
    [Header("�ˌ�����e��������܂ł̎���")]
    [SerializeField] private float destroyTime;

    /// <summary> �ǔ��\�Ȋp�x </summary>
    [Header("�ǔ��\�Ȋp�x(�x���@)")]
    [SerializeField, Range(0, 180.0f)] private float homingAngle;

    /// <summary> �ǔ����\(%) </summary>
    [Header("�ǔ����\(%), 100%�Œǔ��\�Ȋp�x����]���Ēǔ�����")]
    [SerializeField, Range(0, 100.0f)] private float homingPercent;

    /// <summary> �ˌ��Ώ� </summary>
    private Transform target;

    /// <summary> %�̍ő�l </summary>
    private const float percentMax = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        // ��莞�Ԍ�ɍ폜
        Destroy(gameObject, destroyTime);

        // null�`�F�b�N
        if(!target)
        {
            Debug.Log("�ˌ��Ώۂ�������܂���B");
            return;
        }

        // �ˌ��Ώۂ���������Ɍ�����
        transform.LookAt(target.position);
    }

    // Update is called once per frame
    void Update()
    {
        // �����Ă�������ɐi��
        transform.position += transform.forward * speed * Time.deltaTime;

        // null�`�F�b�N
        if (!target)
        {        
            return;
        }

        // �ˌ��Ώۂ܂ł̌����x�N�g��
        Vector3 direction = target.position - transform.position;

        // �ˌ��Ώۂ܂ł̊p�x
        float angleDiff = Vector3.Angle(transform.forward, direction);

        // �ˌ��Ώۂ̕������������Ƃ��̃N�H�[�^�j�I��
        Quaternion rotateTarget = Quaternion.LookRotation(direction);

        // �p�x�����ȉ��Ȃ�ˌ��ΏۂɌ������ĉ�]������
        if (angleDiff <= homingAngle)
        {
            // �ǔ����\(����)
            float homingRatio = homingPercent / percentMax;

            // �ǔ����\(����)���̉�]��������
            transform.rotation = Quaternion.Slerp(transform.rotation, rotateTarget, homingRatio);
        }
    }

    /// <summary>
    /// �ˌ��Ώۗp�v���p�e�B
    /// </summary>
    public Transform Target
    {
        set { target = value; }
    }
}

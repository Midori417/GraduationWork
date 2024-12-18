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

    /// <summary> �ˌ�����e��������܂ł̎��� </summary>
    [Header("�ˌ�����e��������܂ł̎���")]
    [SerializeField] private float destroyTime;

    /// <summary> �ǔ����\(%) </summary>
    [Header("�ǔ����\(%)")]
    [SerializeField, Range(0, 100.0f)] private float homingPercent;

    /// <summary> �ˌ��Ώ� </summary>
    private Transform target;

    /// <summary> homingAngle�x�ȉ��̈ʒu�Ɏˌ��Ώۂ�����Ȃ�ǔ����� </summary>
    private const float homingAngle = 1.0f;

    /// <summary> %�̍ő�l </summary>
    private const float percentMax = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        // �ˌ��Ώۂ��擾����
        target = FindObjectOfType<TargetSetter>().Target;

        // ��莞�Ԍ�ɍ폜
        Destroy(gameObject, destroyTime);

        // �Ώۂ��������
        transform.LookAt(target.position);
    }

    // Update is called once per frame
    void Update()
    {
        // null�`�F�b�N
        if (!target)
        {
            Debug.Log("�ˌ��Ώۂ�������܂���BTargetSetter�����݂��Ȃ��\��������܂��B");
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

        // �����Ă�������ɐi��
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockHead : BaseMsParts
{
    [SerializeField, Header("���{�[��")]
    private Transform headBone;

    [SerializeField, Header("��]���x")]
    private float rotationSpeed = 0.05f;

    [SerializeField, Header("��]�����i���E�̍ő�p�x�j")]
    public float maxHorizontalAngle = 80.0f;

    [SerializeField, Header("��]�����i�㉺�̍ő�p�x�j")]
    private float maxVerticalAngle = 25.0f;

    // ������]��ۑ�����ϐ�
    private Quaternion initialRotation;

    /// <summary>
    /// ����������
    /// </summary>
    public override bool Initalize()
    {
        base.Initalize();
        // ������]��ۑ�
        if (headBone != null)
        {
            initialRotation = headBone.localRotation;
            return true;
        }
        return false;
    }

    /// <summary>
    /// ����
    /// </summary>
    void LateUpdate()
    {
        if (targetMs != null && headBone != null)
        {
            Vector3 directionToTarget = targetMs.position - headBone.position;
            Vector3 localDirection = headBone.parent.InverseTransformDirection(directionToTarget);

            // �^�[�Q�b�g�����̉�]���v�Z
            Quaternion targetRotation = Quaternion.LookRotation(localDirection);

            // �I�C���[�p�Ő���
            Vector3 eulerAngles = targetRotation.eulerAngles;

            // -180�x����180�x�͈̔͂ɕϊ�
            eulerAngles.y = Mathf.Repeat(eulerAngles.y + 180f, 360f) - 180f;
            eulerAngles.x = Mathf.Repeat(eulerAngles.x + 180f, 360f) - 180f;

            Quaternion smoothRotation;

            // ��]�����𒴂����ꍇ�͐��ʂɕ⊮���Ė߂�
            if (Mathf.Abs(eulerAngles.y) > maxHorizontalAngle || Mathf.Abs(eulerAngles.x) > maxVerticalAngle)
            {
                // ���ʁi������]�j�ɕ⊮���Ė߂�
                smoothRotation = Quaternion.Slerp(headBone.localRotation, initialRotation, rotationSpeed);
            }
            else
            {
                // ���݂̉�]����^�[�Q�b�g��]�ւ̕⊮
                smoothRotation = Quaternion.Slerp(headBone.localRotation, targetRotation, rotationSpeed);
            }

            // ��]��K�p
            headBone.localRotation = smoothRotation;
        }
    }
}

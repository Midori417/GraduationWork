using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockHead : MonoBehaviour
{
    [SerializeField, Header("���{�[��")]
    private Transform headBone;

    [SerializeField, Header("�")]
    private Transform baseTrs;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private float rotationSpeed;

    [SerializeField, Header("��]�����i���E�̍ő�p�x�j")]
    public float maxRotationAngle = 60.0f; 

    void LateUpdate()
    {
        //if (target != null && headBone != null)
        //{
        //    Vector3 directionToTarget = headBone.position- target.position;
        //    Vector3 localDirection = headBone.parent.InverseTransformDirection(directionToTarget);

        //    // �^�[�Q�b�g�����̉�]���v�Z
        //    Quaternion targetRotation = Quaternion.LookRotation(localDirection);

        //    // ���݂̉�]����^�[�Q�b�g��]�ւ̕⊮
        //    Quaternion smoothRotation = Quaternion.Slerp(headBone.localRotation, targetRotation, rotationSpeed * Time.deltaTime);

        //    // ��]������K�p
        //    Vector3 limitedEulerAngles = smoothRotation.eulerAngles;
        //    limitedEulerAngles.y = Mathf.Clamp(limitedEulerAngles.y, -maxRotationAngle, maxRotationAngle);

        //    headBone.localRotation = Quaternion.Euler(limitedEulerAngles);
        //}
        headBone.rotation = Quaternion.LookRotation(transform.forward);
    }
}

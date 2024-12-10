using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockHead : MonoBehaviour
{
    [SerializeField, Header("頭ボーン")]
    private Transform headBone;

    [SerializeField, Header("基準")]
    private Transform baseTrs;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private float rotationSpeed;

    [SerializeField, Header("回転制限（左右の最大角度）")]
    public float maxRotationAngle = 60.0f; 

    void LateUpdate()
    {
        //if (target != null && headBone != null)
        //{
        //    Vector3 directionToTarget = headBone.position- target.position;
        //    Vector3 localDirection = headBone.parent.InverseTransformDirection(directionToTarget);

        //    // ターゲット方向の回転を計算
        //    Quaternion targetRotation = Quaternion.LookRotation(localDirection);

        //    // 現在の回転からターゲット回転への補完
        //    Quaternion smoothRotation = Quaternion.Slerp(headBone.localRotation, targetRotation, rotationSpeed * Time.deltaTime);

        //    // 回転制限を適用
        //    Vector3 limitedEulerAngles = smoothRotation.eulerAngles;
        //    limitedEulerAngles.y = Mathf.Clamp(limitedEulerAngles.y, -maxRotationAngle, maxRotationAngle);

        //    headBone.localRotation = Quaternion.Euler(limitedEulerAngles);
        //}
        headBone.rotation = Quaternion.LookRotation(transform.forward);
    }
}

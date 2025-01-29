using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockHead : BaseMsParts
{
    [SerializeField, Header("頭ボーン")]
    private Transform headBone;

    [SerializeField, Header("回転速度")]
    private float rotationSpeed = 0.05f;

    [SerializeField, Header("回転制限（左右の最大角度）")]
    public float maxHorizontalAngle = 80.0f;

    [SerializeField, Header("回転制限（上下の最大角度）")]
    private float maxVerticalAngle = 25.0f;

    // 初期回転を保存する変数
    private Quaternion initialRotation;

    // 前回フレーム時の回転
    private Quaternion oldRotataion;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns>
    /// true    初期化成功
    /// faklse  初期化失敗
    /// </returns>
    public override bool Initalize()
    {
        base.Initalize();

        // 初期回転を保存
        if (!headBone)
        {
            return false;
        }
        initialRotation = headBone.localRotation;
        oldRotataion = headBone.localRotation;
        return true;
    }

    /// <summary>
    /// Updateの後に毎フレーム
    /// </summary>
    private void LateUpdate()
    {
        if (targetMs != null && headBone != null)
        {
            Vector3 directionToTarget = targetMs.position - headBone.position;
            Vector3 localDirection = headBone.parent.InverseTransformDirection(directionToTarget);

            // ターゲット方向の回転を計算
            Quaternion targetRotation = Quaternion.LookRotation(localDirection);

            // オイラー角で制限
            Vector3 eulerAngles = targetRotation.eulerAngles;

            // -180度から180度の範囲に変換
            eulerAngles.y = Mathf.Repeat(eulerAngles.y + 180f, 360f) - 180f;
            eulerAngles.x = Mathf.Repeat(eulerAngles.x + 180f, 360f) - 180f;

            Quaternion smoothRotation;

            // 回転制限を超えた場合は正面に補完して戻る
            if (Mathf.Abs(eulerAngles.y) > maxHorizontalAngle || Mathf.Abs(eulerAngles.x) > maxVerticalAngle)
            {
                // 正面（初期回転）に補完して戻す
                smoothRotation = Quaternion.Slerp(oldRotataion, initialRotation, rotationSpeed * Time.deltaTime);
            }
            else
            {
                // 現在の回転からターゲット回転への補完
                smoothRotation = Quaternion.Slerp(oldRotataion, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // 回転を適用
            headBone.localRotation = smoothRotation;
            oldRotataion = headBone.localRotation;
        }
    }
}

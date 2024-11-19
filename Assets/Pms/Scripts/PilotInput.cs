using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// パイロットの入力
/// </summary>
public class PilotInput : MonoBehaviour
{
    // 移動軸
    public Vector2 moveAxis
    {
        get;
        private set;
    }

    // ジャンプボタン
    public bool isJumpBtn
    {
        get;
        private set;
    }

    // ダッシュボタン
    public bool isDashBtn
    {
        get;
        private set;
    }

    // 入力の更新
    void Update()
    {
        moveAxis = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        isJumpBtn = Input.GetKey(KeyCode.Space);
        isDashBtn = Input.GetKey(KeyCode.LeftShift);

    }
}

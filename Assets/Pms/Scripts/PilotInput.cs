using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// パイロットの入力
/// </summary>
public class PilotInput : MonoBehaviour
{
    public enum BtnState
    {
        Consecutive,
        Down,
        Up,
    }

    public Vector2 MoveAxis
    {
        get;
        private set;
    }

    public bool IsJumpBtn
    {
        get;
        private set;
    }

    public bool IsDashBtn
    {
        get;
        private set;
    }

    public bool GetAttackBtn(BtnState state)
    {
        switch(state)
        {
            case BtnState.Consecutive:
                return Input.GetMouseButton(0);
            case BtnState.Down:
                return Input.GetMouseButtonDown(0);
            case BtnState.Up:
                return Input.GetMouseButtonUp(0);
        }
        return false;
    }

    void Update()
    {
        MoveAxis = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        IsJumpBtn = Input.GetKey(KeyCode.Space);
        IsDashBtn = Input.GetKey(KeyCode.LeftShift);

    }
}

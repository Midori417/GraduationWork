using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CPUパイロット
/// </summary>
public class CpuPilot : BasePilot
{
    private List<Vector2> moveAxiss = new List<Vector2>();

    [SerializeField, Header("移動方向切り替え時間")]
    float moveTimer = 0;

    [SerializeField, Header("次の移動アクションまでの時間")]
    private float moveActionTimer = 0;

    [SerializeField, Header("次の攻撃アクションまでの時間")]
    private float attackActionTimer = 0;

    enum MoveActionState
    {
        // 何もしない
        None,

        Dash,

        Jump,

        Max,
    }

    enum AttackActionState
    {
        None,

        MainShot,

        SubShot,

        Max,
    }


    void Start()
    {
        base.Initialize();

        // 仮想軸を追加
        moveAxiss.Add(Vector2.zero);
        moveAxiss.Add(Vector2.right);
        moveAxiss.Add(Vector2.left);
        moveAxiss.Add(Vector2.up);
        moveAxiss.Add(Vector2.down);
        moveAxiss.Add(new Vector2(1, 1));
        moveAxiss.Add(new Vector2(-1, 1));
        //moveAxiss.Add(new Vector2(-1, -1));
        //moveAxiss.Add(new Vector2(1, -1));

        // 初期行動
        MoveAxisProsess();
        Invoke("MoveActionProsess", moveActionTimer);
        Invoke("AttackActionProsess", attackActionTimer);
    }

    /// <summary>
    /// 移動方向を決める
    /// </summary>
    void MoveAxisProsess()
    {
        int index = Random.Range(0, moveAxiss.Count);

        myMs.moveAxis = moveAxiss[index];

        Invoke("MoveAxisProsess", moveTimer);
    }

    /// <summary>
    /// 移動アクション
    /// </summary>
    void MoveActionProsess()
    {
        MoveActionState state = (MoveActionState)Random.Range(0, (int)MoveActionState.Max);

        myMs.isDashBtn = false;
        myMs.isJumpBtn = false;
        switch (state)
        {
            case MoveActionState.None:
                break;
            case MoveActionState.Dash:
                myMs.isDashBtn = true;
                break;
            case MoveActionState.Jump:
                myMs.isJumpBtn = true;
                break;
        }

        Invoke("MoveActionProsess", moveActionTimer);
    }

    /// <summary>
    /// 攻撃行動
    /// </summary>
    void AttackActionProsess()
    {
        AttackActionState state = (AttackActionState)Random.Range(0, (int)AttackActionState.Max);

        myMs.isMainShotBtn = false;
        myMs.isSubShotBtn = false;
        switch (state)
        {
            case AttackActionState.None:
                break;
            case AttackActionState.MainShot:
                myMs.isMainShotBtn = true;
                break;
            case AttackActionState.SubShot:
                myMs.isSubShotBtn = true;
                break;
        }

        Invoke("AttackActionProsess", attackActionTimer);
    }

}

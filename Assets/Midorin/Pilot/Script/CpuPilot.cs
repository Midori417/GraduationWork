using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CPUパイロット
/// </summary>
public class CpuPilot : BasePilot
{
    [SerializeField, Header("trueなら機体を止める")]
    private bool isStopMs = false;

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

    }

    /// <summary>
    /// プレイ
    /// </summary>
    public override void StartProsess()
    {
        base.StartProsess();

        // 初期行動
        MoveAxisProsess();
        Invoke("MoveActionProsess", moveActionTimer);
        Invoke("AttackActionProsess", attackActionTimer);
    }

    /// <summary>
    /// ストップ
    /// </summary>
    public override void StopProsess()
    {
        base.StopProsess();
    }

    /// <summary>
    /// 移動方向を決める
    /// </summary>
    void MoveAxisProsess()
    {
        if (!isStopMs)
        {
            int index = Random.Range(0, moveAxiss.Count);

            myMs.moveAxis = moveAxiss[index];
        }
        else
        {
            myMs.moveAxis = Vector2.zero;
        }
        Invoke("MoveAxisProsess", moveTimer);
    }

    /// <summary>
    /// 移動アクション
    /// </summary>
    void MoveActionProsess()
    {
        if (!isStopMs)
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
        }
        else
        {
            myMs.isDashBtn = false;
            myMs.isJumpBtn = false;
        }
        Invoke("MoveActionProsess", moveActionTimer);
    }

    /// <summary>
    /// 攻撃行動
    /// </summary>
    void AttackActionProsess()
    {
        if (!isStopMs)
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
        }
        else
        {
            myMs.isMainShotBtn = false;
            myMs.isSubShotBtn = false;
        }
        Invoke("AttackActionProsess", attackActionTimer);
    }
}

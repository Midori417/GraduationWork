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

    /// <summary>
    /// 移動方向を決める
    /// </summary>
    void MoveAxisProsess()
    {
        int index = Random.Range(0, moveAxiss.Count);

        msInput._move = moveAxiss[index];
        myMs.msInput = msInput;
        Invoke("MoveAxisProsess", moveTimer);
    }

    /// <summary>
    /// 移動アクション
    /// </summary>
    void MoveActionProsess()
    {
        MoveActionState state = (MoveActionState)Random.Range(0, (int)MoveActionState.Max);

        msInput._dash = false;
        msInput._jump = false;
        switch (state)
        {
            case MoveActionState.None:
                break;
            case MoveActionState.Dash:
                msInput._dash = true;
                break;
            case MoveActionState.Jump:
                msInput._jump = true;
                break;
        }
        myMs.msInput = msInput;
        Invoke("MoveActionProsess", moveActionTimer);
    }

    /// <summary>
    /// 攻撃行動
    /// </summary>
    void AttackActionProsess()
    {
        AttackActionState state = (AttackActionState)Random.Range(0, (int)AttackActionState.Max);

        msInput._mainShot = false;
        msInput._subShot = false;
        switch (state)
        {
            case AttackActionState.None:
                break;
            case AttackActionState.MainShot:
                msInput._mainShot = true;
                break;
            case AttackActionState.SubShot:
                msInput._subShot = true;
                break;
        }
        myMs.msInput = msInput;
        Invoke("AttackActionProsess", attackActionTimer);
    }

    #region イベント関数

    private void Start()
    {
        // 仮想軸を追加
        moveAxiss.Add(Vector2.zero);
        moveAxiss.Add(Vector2.right);
        moveAxiss.Add(Vector2.left);
        moveAxiss.Add(Vector2.up);
        moveAxiss.Add(Vector2.down);
        moveAxiss.Add(new Vector2(1, 1));
        moveAxiss.Add(new Vector2(-1, 1));
    }

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    private void Update()
    {
        if (isStop) return;

        MsUpdate();
    }

    #endregion

    /// <summary>
    /// オブジェクトの処理を開始
    /// </summary>
    public override void Play()
    {
        base.Play();
        // 初期行動
        MoveAxisProsess();
        Invoke("MoveActionProsess", moveActionTimer);
        Invoke("AttackActionProsess", attackActionTimer);
    }

    /// <summary>
    /// 機体の更新
    /// </summary>
    protected override void MsUpdate()
    {
        base.MsUpdate();
        MsInput();
    }

    /// <summary>
    /// 機体の入力
    /// </summary>
    private void MsInput()
    {
    }
}

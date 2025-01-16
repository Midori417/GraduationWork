using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CPU�p�C���b�g
/// </summary>
public class CpuPilot : BasePilot
{
    [SerializeField, Header("true�Ȃ�@�̂��~�߂�")]
    private bool isStopMs = false;

    private List<Vector2> moveAxiss = new List<Vector2>();

    [SerializeField, Header("�ړ������؂�ւ�����")]
    float moveTimer = 0;

    [SerializeField, Header("���̈ړ��A�N�V�����܂ł̎���")]
    private float moveActionTimer = 0;

    [SerializeField, Header("���̍U���A�N�V�����܂ł̎���")]
    private float attackActionTimer = 0;

    enum MoveActionState
    {
        // �������Ȃ�
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
        // ���z����ǉ�
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
    /// �v���C
    /// </summary>
    public override void StartProsess()
    {
        base.StartProsess();

        // �����s��
        MoveAxisProsess();
        Invoke("MoveActionProsess", moveActionTimer);
        Invoke("AttackActionProsess", attackActionTimer);
    }

    /// <summary>
    /// �X�g�b�v
    /// </summary>
    public override void StopProsess()
    {
        base.StopProsess();
    }

    /// <summary>
    /// �ړ����������߂�
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
    /// �ړ��A�N�V����
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
    /// �U���s��
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CPUパイロット
/// </summary>
public class CpuPilot : BasePilot
{
    // 仮想軸
    private List<Vector2> _moveAxisList = new List<Vector2>();

    [SerializeField, Header("移動方向切り替え時間")]
    private GameTimer _moveTimer = new GameTimer();

    [SerializeField, Header("次の移動アクションまでの時間")]
    private GameTimer _moveActionTimer = new GameTimer();

    [SerializeField, Header("次の攻撃アクションまでの時間")]
    private GameTimer _attackActionTimer = new GameTimer();

    [SerializeField, Header("ターゲット切り替え時間")]
    private GameTimer _targetChangeTimer = new GameTimer();

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

    #region イベント関数

    private void Start()
    {
        // 仮想軸を追加
        _moveAxisList.Add(Vector2.zero);
        _moveAxisList.Add(Vector2.right);
        _moveAxisList.Add(Vector2.left);
        _moveAxisList.Add(Vector2.up);
        _moveAxisList.Add(Vector2.down);
        _moveAxisList.Add(new Vector2(1, 1));
        _moveAxisList.Add(new Vector2(-1, 1));
    }

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    private void Update()
    {
        if (isStop) return;
        MsUpdate();
        TargetUpdate();
    }

    #endregion

    /// <summary>
    /// 機体の更新
    /// </summary>
    protected override void MsUpdate()
    {
        base.MsUpdate();
        //return;
        MsInput();
    }

    /// <summary>
    /// 機体の入力
    /// </summary>
    private void MsInput()
    {
        SetMoveAxis();
        SetMoveAction();
        SetAttackAction();
        SetTargetChange();
    }

    /// <summary>
    /// 移動軸を設定する
    /// </summary>
    private void SetMoveAxis()
    {
        if(_moveTimer.UpdateTimer())
        {
            _moveTimer.ResetTimer();
            int index = Random.Range(0, _moveAxisList.Count);
            msInput.SetMoveAxis(_moveAxisList[index]);
        }
    }

    /// <summary>
    /// 移動アクションを設定する
    /// </summary>
    private void SetMoveAction()
    {
        if(_moveActionTimer.UpdateTimer())
        {
            _moveActionTimer.ResetTimer();

            MoveActionState state = (MoveActionState)Random.Range(0, (int)MoveActionState.Max);
            bool dash = false;
            bool jump = false;
            switch (state)
            {
                case MoveActionState.None:
                    break;
                case MoveActionState.Dash:
                    dash = true;
                    break;
                case MoveActionState.Jump:
                    jump = true;
                    break;
            }
            msInput.SetInput(GameInputState.Dash, dash);
            msInput.SetInput(GameInputState.Jump, jump);
        }
    }

    /// <summary>
    /// 攻撃アクションを設定する
    /// </summary>
    private void SetAttackAction()
    {
        if (_attackActionTimer.UpdateTimer())
        {
            _attackActionTimer.ResetTimer();
            // 赤距離なら近接攻撃を出すようにする
            if (myMs.isRedDistance)
            {
                int rnd = Random.Range(0, 2);
                if (rnd > 0)
                {
                    msInput.SetInput(GameInputState.MainAttack, false);
                    msInput.SetInput(GameInputState.MainAttack, true);
                }
                else
                {
                    SetAttackActionHelper();
                }
            }
            else
            {
                SetAttackActionHelper();
            }
        }
    }

    /// <summary>
    /// 攻撃アクション設定補助関数
    /// </summary>
    private void SetAttackActionHelper()
    {
        AttackActionState state = (AttackActionState)Random.Range(0, (int)AttackActionState.Max);
        switch (state)
        {
            case AttackActionState.None:
                msInput.SetInput(GameInputState.MainAttack, false);
                msInput.SetInput(GameInputState.MainShot, false);
                msInput.SetInput(GameInputState.SubShot, false);
                break;
            case AttackActionState.MainShot:
                msInput.SetInput(GameInputState.MainShot, false);
                msInput.SetInput(GameInputState.MainShot, true);
                break;
            case AttackActionState.SubShot:
                msInput.SetInput(GameInputState.SubShot, false);
                msInput.SetInput(GameInputState.SubShot, true);
                break;
        }
    }

    /// <summary>
    /// ターゲットの切り替えを設定
    /// </summary>
    private void SetTargetChange()
    {
        if (_targetChangeTimer.UpdateTimer())
        {
            _targetChangeTimer.ResetTimer();
            int random = Random.Range(0, 10);
            // 近いほうのパイロットを取得
            BasePilot pilot = EnemyDistance();
            //近いパイロットが同じなら基本そのまま
            if (targetPilot == pilot)
            {
                if (random > 8)
                {
                    msInput.SetInput(GameInputState.TargetChange, false);
                    msInput.SetInput(GameInputState.TargetChange, true);
                }
            }
            else
            {
                if (random > 2)
                {
                    msInput.SetInput(GameInputState.TargetChange, false);
                    msInput.SetInput(GameInputState.TargetChange, true);
                }
            }
        }
    }

    /// <summary>
    /// 近いほうのエネミーを取得する
    /// </summary>
    private BasePilot EnemyDistance()
    {
        // エネミーの数が少ない
        if(enemyPilots.Count <= 1)
        {
            return null;
        }
        BasePilot tmp = null;
        float distanceA = Vector3.Distance(myMs.transform.position, enemyPilots[0].myMs.transform.position);
        float distanceB = Vector3.Distance(myMs.transform.position, enemyPilots[1].myMs.transform.position);
        if(distanceA > distanceB)
        {
            return enemyPilots[1];
        }
        return enemyPilots[0];
    }
}

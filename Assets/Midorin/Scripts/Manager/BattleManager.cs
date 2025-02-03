using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// バトル管理クラス
/// </summary>
public class BattleManager : SingletonBehavior<BattleManager>
{
    enum State
    {
        Standby,
        Go,
        Battle,
        End
    }
    StateMachine<State> _stateMachine = new StateMachine<State>();

    [SerializeField, Header("STANBY時間")]
    private float _stanbyTime = 0;

    [SerializeField, Header("GO時間")]
    private float _goTime = 0;

    private class TeamCost
    {
        // 赤チームコスト
        public int _red;

        // 青チームコスト
        public int _blue;

        /// <summary>
        /// チームコストが0以下になっていないか検知
        /// </summary>
        /// <returns>
        /// true
        /// </returns>
        public bool IsEnd()
        {
            return (_red <= 0) || (_blue <= 0);
        }
    }
    private TeamCost _teamCost = new TeamCost();

    private struct PilotVaiable
    {
        // 全パイロット
        public List<BasePilot> _all;

        // 赤チームパイロット
        public List<BasePilot> _red;

        // 青チームパイロット
        public List<BasePilot> _blue;

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            _all = new List<BasePilot>();
            _red = new List<BasePilot>();
            _blue = new List<BasePilot>();
        }
    }
    private PilotVaiable _pilot;

    // バトル時間を格納する変数
    private int _battleTime = 0;
    private int _battleTimer = 0;

    [SerializeField, Header("マップ")]
    private MapManager _mapManager;

    [SerializeField, Header("バトル")]
    private BattleEventUIControl _battleEventUIControl;

    // ゲームマネージャー
    private GameManager _gameManager;

    public int battleTimer => _battleTimer;
    public int redCost => _teamCost._red;
    public int blueCost => _teamCost._blue ;

    #region イベント関数

    /// <summary>
    /// 生成時に呼び出される
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        SetUp();
    }

    /// <summary>
    /// Updateの前に呼び出される
    /// </summary>
    private void Start()
    {
        _pilot.Initialize();
        Setting();
    }

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    private void Update()
    {
        _stateMachine.UpdateState();
    }

    #endregion

    #region バトルセッティング

    /// <summary>
    /// バトルを設定
    /// </summary>
    private void Setting()
    {
        _gameManager = GameManager.I;
        //BattleInfo battleInfo = _gameManager._battleInfo;

        //テスト用↓
        BattleInfo battleInfo = new BattleInfo();
        battleInfo.pilotsInfo = new List<PilotInfo>();
        {
            {
                battleInfo.time = 2 * 60;
                battleInfo.teamRedCost = 6000;
                battleInfo.teamBlueCost = 3000;
            }
            {
                PilotInfo pilotInfo;
                pilotInfo.teamId = Team.Red;
                pilotInfo.playerType = PlayerType.Human;
                pilotInfo.useMs = MsList.Gundam;
                battleInfo.pilotsInfo.Add(pilotInfo);
            }
            {
                PilotInfo pilotInfo;
                pilotInfo.teamId = Team.Blue;
                pilotInfo.playerType = PlayerType.Cpu;
                pilotInfo.useMs = MsList.Gundam;
                battleInfo.pilotsInfo.Add(pilotInfo);
            }

            {
                PilotInfo pilotInfo;
                pilotInfo.teamId = Team.None;
                pilotInfo.playerType = PlayerType.Cpu;
                pilotInfo.useMs = MsList.Gundam;
                battleInfo.pilotsInfo.Add(pilotInfo);
            }
            {
                PilotInfo pilotInfo;
                pilotInfo.teamId = Team.None;
                pilotInfo.playerType = PlayerType.Cpu;
                pilotInfo.useMs = MsList.Gundam;
                battleInfo.pilotsInfo.Add(pilotInfo);
            }
        }
        BattleSetting(battleInfo);
        PilotSetting(battleInfo);
        MsStartPos();
    }

    /// <summary>
    /// バトル情報を設定
    /// </summary>
    private void BattleSetting(BattleInfo battleInfo)
    {
        _teamCost._red = battleInfo.teamRedCost;
        _teamCost._blue = battleInfo.teamBlueCost;
        _battleTime = (battleInfo.time);
    }

    /// <summary>
    /// パイロット設定
    /// </summary>
    /// <param name="battleInfo"></param>
    private void PilotSetting(BattleInfo battleInfo)
    {
        for (int i = 0; i < battleInfo.pilotsInfo.Count; ++i)
        {
            PilotInfo pilotInfo = battleInfo.pilotsInfo[i];

            // 不参加
            if (pilotInfo.teamId == Team.None) continue;

            // 人間かCPUか判断
            BasePilot pilot;
            if (pilotInfo.playerType == PlayerType.Human)
                pilot = Instantiate(_gameManager.GetPilotPrefab(true));
            else
                pilot = Instantiate(_gameManager.GetPilotPrefab(false));

            // 機体選び
            pilot.myMs = MsInstance(pilotInfo.useMs);

            // チームに追加
            if (pilotInfo.teamId == Team.Red)
            {
                pilot.team = Team.Red;
                _pilot._red.Add(pilot);
            }
            else if (pilotInfo.teamId == Team.Blue)
            {
                pilot.team = Team.Blue;
                _pilot._blue.Add(pilot);
            }
            // リストに追加
            _pilot._all.Add(pilot);
        }

        // 敵チームを設定
        foreach (BasePilot pilot in _pilot._red)
        {
            pilot.enemyPilots = _pilot._blue;
        }
        foreach (BasePilot pilot in _pilot._blue)
        {
            pilot.enemyPilots = _pilot._red;
        }

        // パイロットの初期化
        foreach (BasePilot pilot in _pilot._all)
        {
            pilot.Initialize();
        }
    }

    /// <summary>
    /// 機体の生成
    /// </summary>
    private BaseMs MsInstance(MsList useMs)
    {
        BaseMs ms = null;

        switch (useMs)
        {
            case MsList.Gundam:
                ms = Instantiate(_gameManager.GetMsPrefab((int)MsList.Gundam));
                break;
        }
        return ms;
    }

    /// <summary>
    /// 機体の位置をセット
    /// </summary>
    private void MsStartPos()
    {
        if (!_mapManager)
        {
            Debug.LogError("MapManagerが設定されていないよ");
            return;
        }
        List<Transform> respones = _mapManager.responTrs;
        foreach (BasePilot pilot in _pilot._red)
        {
            pilot.myMs.transform.SetPositionAndRotation(respones[0].position, respones[0].rotation);
        }
        foreach (BasePilot pilot in _pilot._blue)
        {
            pilot.myMs.transform.SetPositionAndRotation(respones[1].position, respones[1].rotation);
        }
    }

    #endregion

    #region 状態をセットアップ

    /// <summary>
    /// 状態をセットアップ
    /// </summary>
    private void SetUp()
    {
        SetUpStandby();
        SetUpGo();
        SetUpBattle();
        SetUpEnd();

        _stateMachine.Setup(State.Standby);
    }

    /// <summary>
    /// Standby状態をセットアップ
    /// </summary>
    private void SetUpStandby()
    {
        State state = State.Standby;
        GameTimer timer = new GameTimer(_stanbyTime);
        Action<State> enter = (prev) =>
        {
            _battleEventUIControl.Stanby();
        };
        Action update = () =>
        {
            if (timer.UpdateTimer())
            {
                _stateMachine.ChangeState(State.Go);
            }
        };
        Action<State> exit = (next) =>
        {
        };
        _stateMachine.AddState(state, enter, update, exit);
    }

    /// <summary>
    /// Go状態をセットアップ
    /// </summary>
    private void SetUpGo()
    {
        State state = State.Go;
        GameTimer timer = new GameTimer(_goTime);
        Action<State> enter = (prev) =>
        {
            _battleEventUIControl.Go();
        };
        Action update = () =>
        {
            if (timer.UpdateTimer())
            {
                _stateMachine.ChangeState(State.Battle);
            }
        };
        Action<State> exit = (next) =>
        {
        };
        _stateMachine.AddState(state, enter, update, exit);
    }

    /// <summary>
    /// バトル状態をセットアップ
    /// </summary>
    private void SetUpBattle()
    {
        State state = State.Battle;
        GameTimer timer = new GameTimer();
        Action<State> enter = (prev) =>
        {
            _battleEventUIControl.NoImg();
            timer.ResetTimer(_battleTime);
            foreach (BasePilot pilot in _pilot._all)
            {
                pilot.Play();
            }
        };
        Action update = () =>
        {
            _battleTimer = (int)timer.remain;
            // 時間制限かどちらかのコストがなくなれば終了
            if (timer.UpdateTimer() || _teamCost.IsEnd())
            {
                _stateMachine.ChangeState(State.End);
            }
        };
        Action<State> exit = (next) =>
        {
            foreach (BasePilot pilot in _pilot._all)
            {
                pilot.Stop();
            }
        };
        _stateMachine.AddState(state, enter, update, exit);
    }

    /// <summary>
    /// End状態をセットアップ
    /// </summary>
    private void SetUpEnd()
    {
        State state = State.End;
        Action<State> enter = (prev) =>
        {
        };
        Action update = () =>
        {
        };
        Action<State> exit = (next) =>
        {
        };
        _stateMachine.AddState(state, enter, update, exit);
    }

    #endregion
}

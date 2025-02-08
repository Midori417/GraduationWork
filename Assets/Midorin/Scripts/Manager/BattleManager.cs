using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    bool isStop = false;

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
    private float _battleTime = 0;
    private float _battleTimer = 0;

    [SerializeField, Header("マップ")]
    private MapManager _mapManager;

    // 復活位置
    private List<Transform> _responTrs;

    [SerializeField, Header("バトル")]
    private BattleEventUIControl _battleEventUIControl;

    // ゲームマネージャー
    private GameManager _gameManager;

    public float battleTimer => _battleTimer;
    public int redCost => _teamCost._red;
    public int blueCost => _teamCost._blue;

    [SerializeField, Header("フェードオブジェクト")]
    private FadeOut _fadeOut;

    [SerializeField, Header("btnの親")]
    private GameObject _btnParent;

    int _selctNum = 0;

    [SerializeField]
    private List<Image> _selectedImages;
    [SerializeField]
    private List<Button> _selectedButtons;

    Color _noramColor = new Color(0.4f, 0.4f, 0.4f);
    Color _hightColor = Color.white;
    bool isOn = false;


    [Serializable]
    private struct AudioValiable
    {
        [SerializeField, Header("audioBGM")]
        private AudioSource _bgmSource;

        [SerializeField, Header("bgmAudioClip")]
        private List<AudioClip> _bgmClipList;

        [SerializeField, Header("audioSE")]
        private AudioSource _seSource;

        [SerializeField, Header("Stanby")]
        private AudioClip _seStanby;

        [SerializeField, Header("Go")]
        private AudioClip _seGo;

        [SerializeField, Header("Finish")]
        private AudioClip _seFinish;

        [SerializeField, Header("選択オン")]
        private AudioClip _seSelect;

        #region BGM関数

        /// <summary>
        /// BGMを再生
        /// </summary>
        public void BGMPlay()
        {
            if (!_bgmSource) return;
            if (_bgmClipList.Count <= 0) return;

            // clipが設定されていないなら乱数で設定
            if (!_bgmSource.clip)
            {
                int random = UnityEngine.Random.Range(0, _bgmClipList.Count);
                _bgmSource.clip = _bgmClipList[random];
            }
            _bgmSource.Play();
        }

        /// <summary>
        /// BGMをストップ
        /// </summary>
        public void BGMStop()
        {
            if (!_bgmSource) return;
            _bgmSource.Stop();
        }

        #endregion

        #region SE関数

        /// <summary>
        /// Ready音声を流す
        /// </summary>
        public void StanbyPlay()
        {
            if (!_seSource) return;
            if (!_seStanby) return;
            _seSource.PlayOneShot(_seStanby);
        }

        /// <summary>
        /// Go音声を流す
        /// </summary>
        public void GoPlay()
        {
            if (!_seSource) return;
            if (!_seGo) return;
            _seSource.PlayOneShot(_seGo);
        }

        /// <summary>
        /// Finish音声を流す
        /// </summary>
        public void FinishPlay()
        {
            if (!_seSource) return;
            if (!_seFinish) return;
            _seSource.PlayOneShot(_seFinish);
        }

        /// <summary>
        /// Finish音声を流す
        /// </summary>
        public void SelectPlay()
        {
            if (!_seSource) return;
            if (!_seSelect) return;
            _seSource.PlayOneShot(_seSelect);
        }

        #endregion
    }
    [SerializeField, Header("サウンド関係")]
    private AudioValiable _audio;

    private List<BasicBulletMove> _bulletList = new List<BasicBulletMove>(100);

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
        //テスト用↓
        BattleInfo battleInfo = new BattleInfo();
        battleInfo.pilotsInfo = new List<PilotInfo>();
        {
            {
                battleInfo.time = 3 * 60;
                battleInfo.teamRedCost = 6000;
                battleInfo.teamBlueCost = 6000;
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
                pilotInfo.teamId = Team.Blue;
                pilotInfo.playerType = PlayerType.Cpu;
                pilotInfo.useMs = MsList.Gundam;
                battleInfo.pilotsInfo.Add(pilotInfo);
            }
            {
                PilotInfo pilotInfo;
                pilotInfo.teamId = Team.Red;
                pilotInfo.playerType = PlayerType.Cpu;
                pilotInfo.useMs = MsList.Gundam;
                battleInfo.pilotsInfo.Add(pilotInfo);
            }
        }
        BattleSetting(battleInfo);
        PilotSetting(battleInfo);
        MsStartPos();
        foreach (BasePilot pilot in _pilot._all)
        {
            pilot.Stop();
        }
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
        if (_pilot._red.Count > 1)
        {
            _pilot._red[0].teamPilot = _pilot._red[1];
            _pilot._red[1].teamPilot = _pilot._red[0];
        }
        if (_pilot._blue.Count > 1)
        {
            _pilot._blue[0].teamPilot = _pilot._blue[1];
            _pilot._blue[1].teamPilot = _pilot._blue[0];
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
        _responTrs = _mapManager.responTrs;
        for (int i = 0; i < _pilot._red.Count; ++i)
        {
            _pilot._red[i].myMs.transform.SetPositionAndRotation(_responTrs[0].position, _responTrs[0].rotation);
            _pilot._red[i].myMs.transform.Translate(30 * i, 0, 0);
        }
        for (int i = 0; i < _pilot._blue.Count; ++i)
        {
            _pilot._blue[i].myMs.transform.SetPositionAndRotation(_responTrs[1].position, _responTrs[1].rotation);
            _pilot._blue[i].myMs.transform.Translate(30 * i, 0, 0);
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
            _audio.StanbyPlay();
        };
        Action update = () =>
        {
            if (timer.UpdateTimer())
            {
                _stateMachine.ChangeState(State.Go);
            }
        };
        Action lateUpdate = () =>
        {

        };
        Action<State> exit = (next) =>
        {
        };
        _stateMachine.AddState(state, enter, update, lateUpdate, exit);
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
            _audio.GoPlay();
        };
        Action update = () =>
        {
            if (timer.UpdateTimer())
            {
                _stateMachine.ChangeState(State.Battle);
            }
        };
        Action lateUpdate = () =>
        {

        };
        Action<State> exit = (next) =>
        {
        };
        _stateMachine.AddState(state, enter, update, lateUpdate, exit);
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
            _audio.BGMPlay();
            _battleEventUIControl.NoImg();
            timer.ResetTimer(_battleTime);
            Play();
        };
        Action update = () =>
        {
            if (isStop) return;

            _battleTimer = timer.remain;
            // 時間制限かどちらかのコストがなくなれば終了
            if (timer.UpdateTimer() || _teamCost.IsEnd())
            {
                _battleTimer = timer.remain;
                _stateMachine.ChangeState(State.End);
            }
        };
        Action lateUpdate = () =>
        {

        };
        Action<State> exit = (next) =>
        {
            _audio.BGMStop();
            Stop();
        };
        _stateMachine.AddState(state, enter, update, lateUpdate, exit);
    }

    /// <summary>
    /// End状態をセットアップ
    /// </summary>
    private void SetUpEnd()
    {
        State state = State.End;
        GameTimer uitimer = new GameTimer(1);
        Action<State> enter = (prev) =>
        {
            _audio.FinishPlay();
            if (battleTimer <= 0)
            {
                // 青の勝ち
                if (redCost < blueCost)
                {
                    foreach (BasePilot pilot in _pilot._red)
                    {
                        pilot.SetVitory(Victory.Lose);
                    }
                    foreach (BasePilot pilot in _pilot._blue)
                    {
                        pilot.SetVitory(Victory.Win);
                    }
                }
                // 勝ちの勝ち
                else if (blueCost < redCost)
                {
                    foreach (BasePilot pilot in _pilot._red)
                    {
                        pilot.SetVitory(Victory.Win);
                    }
                    foreach (BasePilot pilot in _pilot._blue)
                    {
                        pilot.SetVitory(Victory.Lose);
                    }
                }
                // 引き分け
                else
                {
                    foreach (BasePilot pilot in _pilot._all)
                    {
                        pilot.SetVitory(Victory.Draw);
                    }
                }
            }
            else
            {
                if (redCost <= 0)
                {
                    foreach (BasePilot pilot in _pilot._red)
                    {
                        pilot.SetVitory(Victory.Lose);
                    }
                    foreach (BasePilot pilot in _pilot._blue)
                    {
                        pilot.SetVitory(Victory.Win);
                    }
                }
                if (blueCost <= 0)
                {
                    foreach (BasePilot pilot in _pilot._red)
                    {
                        pilot.SetVitory(Victory.Win);
                    }
                    foreach (BasePilot pilot in _pilot._blue)
                    {
                        pilot.SetVitory(Victory.Lose);
                    }
                }
            }

        };
        Action update = () =>
        {
            if (uitimer.UpdateTimer())
            {
                // ボタンを表示する
                if (_btnParent) _btnParent.SetActive(true);

                EndControlUpdate();
            }
        };
        Action lateUpdate = () =>
        {
        };
        Action<State> exit = (next) =>
        {
        };
        _stateMachine.AddState(state, enter, update, lateUpdate, exit);
    }

    #endregion

    #region ボタン

    /// <summary>
    /// 操作の更新
    /// </summary>
    void EndControlUpdate()
    {
        if (isOn) return;
        // 一個前に戻る
        if (Gamepad.current == null)
        {
            foreach (Button button in _selectedButtons)
            {
                button.enabled = true;
            }
        }
        else
        {
            // 上下の切り替え
            if (Gamepad.current.dpad.left.wasPressedThisFrame)
            {
                _selctNum--;
            }
            if (Gamepad.current.dpad.right.wasPressedThisFrame)
            {
                _selctNum++;
            }
            _selctNum = Mathf.Clamp(_selctNum, 0, 1);
            if (Gamepad.current.crossButton.wasPressedThisFrame)
            {
                isOn = true;
                if (_selctNum == 0)
                {
                    PushBattleSetting();
                }
                else if (_selctNum == 1)
                {
                    PushTitle();
                }
            }
            foreach (Button button in _selectedButtons)
            {
                button.enabled = false;
            }
            for (int i = 0; i < _selectedImages.Count; i++)
            {
                if (_selctNum == i)
                {
                    _selectedImages[i].color = _noramColor;
                }
                else
                {
                    _selectedImages[i].color = _hightColor;
                }
            }
        }
    }

    /// <summary>
    /// Battleボタンを選択したときの処理
    /// </summary>
    public void PushBattleSetting()
    {
        if (!_fadeOut)
        {
            Debug.LogError("フェードオブジェクトが存在しません");
            return;
        }
        // バトル設定シーンに移行させる
        _fadeOut.FadeStrt(Global._battleSettingScene);
        _audio.SelectPlay();
    }

    /// <summary>
    /// Battleボタンを選択したときの処理
    /// </summary>
    public void PushTitle()
    {
        if (!_fadeOut)
        {
            Debug.LogError("フェードオブジェクトが存在しません");
            return;
        }
        // タイトルシーンに移行させる
        _fadeOut.FadeStrt(Global._titleScene);
        _audio.SelectPlay();
    }

    #endregion

    private void Play()
    {
        isStop = false;
        foreach (BasePilot pilot in _pilot._all)
        {
            pilot.Play();
        }
        foreach (BasicBulletMove bullet in _bulletList)
        {
            bullet.Play();
        }
    }

    private void Stop()
    {
        isStop = true;
        foreach (BasePilot pilot in _pilot._all)
        {
            pilot.Stop();
        }
        foreach (BasicBulletMove bullet in _bulletList)
        {
            bullet.Stop();
        }
    }

    public void SetBullet(BasicBulletMove bullet)
    {
        _bulletList.Add(bullet);
    }

    public void RemoveBullet(BasicBulletMove bullet)
    {
        _bulletList.Remove(bullet);
    }

    /// <summary>
    /// コストダウン
    /// </summary>
    /// <param name="cost"></param>
    public void CostDown(int cost, Team team)
    {
        if (team == Team.Red)
        {
            _teamCost._red -= cost;
        }
        else if (team == Team.Blue)
        {
            _teamCost._blue -= cost;
        }
    }

    /// <summary>
    /// チームコストを取得
    /// </summary>
    /// <param name="team"></param>
    public int GetTeamCost(Team team)
    {
        if (team == Team.Red)
        {
            return redCost;
        }
        else if (team == Team.Blue)
        {
            return blueCost;
        }

        return 0;
    }

    /// <summary>
    /// 復活位置を取得
    /// </summary>
    /// <returns></returns>
    public Vector3 GetResponPos()
    {
        int random = UnityEngine.Random.Range(0, _responTrs.Count);
        return _responTrs[random].position;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// バトル管理クラス
/// </summary>
public class BattleManager : MonoBehaviour
{
    [SerializeField, Header("初まりからスタンバイをするまで")]
    private float startChangeStanbyTime;

    [SerializeField, Header("スタンバイからゴーをするまで")]
    private float stanbyChangeGoTime;

    [SerializeField, Header("ゴーからバトルスタートするまで")]
    private float goChangeBattleStartTim;

    // 赤チームのコスト
    int teamRedCost;

    // 青チームのコスト
    int teamBlueCost;

    [SerializeField, Header("戦うパイロット")]
    private List<BasePilot> battlePilots = new List<BasePilot>();

    // 赤チームパイロット
    private List<BasePilot> redTeamPilots = new List<BasePilot>();

    // 青チームパイロット
    private List<BasePilot> blueTeamPilots = new List<BasePilot>();

    [SerializeField, Header("マップ")]
    private MapManager mapManager;
    private List<Transform> respones = new List<Transform>();

    [SerializeField, Header("バトルUIコントロール")]
    private BattleEventUIControl battleEventUIControl;

    BattleInfo battleInfo;

    [SerializeField]
    GameManager gameManager;

    /// <summary>
    /// スタートイベント
    /// </summary>
    private void Start()
    {
        // バトル情報を取得
        //BattleInfo battleInfo = GameManager.instance.battleInfo;
        gameManager = GameManager.instance;
        {
            battleInfo.pilotsInfo = new List<PilotInfo>();
            // テスト
            battleInfo.teamRedCost = GameManager.teamCostMax;
            battleInfo.teamBlueCost = GameManager.teamCostMax;
            battleInfo.time = 5;
            {
                PilotInfo pilotInfo;
                pilotInfo.teamId = Team.Read;
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

        BattleSetting();
        PilotSetting();

        MsStartPosition();

        Invoke("StanbyProsess", stanbyChangeGoTime);
    }

    /// <summary>
    /// 更新イベント
    /// </summary>
    private void Update()
    {
        BattleProsess();
    }

    /// <summary>
    /// バトル中の処理
    /// </summary>
    void BattleProsess()
    {
        if (BattleEneCheck())
        {
            BattleEndProess();
        }
    }

    /// <summary>
    /// バトル終了しているかチェック
    /// </summary>
    /// <returns>
    /// true 終了
    /// false 続いている
    /// </returns>
    bool BattleEneCheck()
    {
        if (teamBlueCost <= 0 || teamRedCost<= 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// バトル終了処理
    /// </summary>
    void BattleEndProess()
    {
        foreach(BasePilot pilot in battlePilots)
        {
            pilot.StopProsess();
        }
    }

    /// <summary>
    /// スタンバイ処理
    /// </summary>
    void StanbyProsess()
    {
        battleEventUIControl.Stanby();
        Invoke("GoProsess", stanbyChangeGoTime);
    }

    /// <summary>
    /// ゴー処理
    /// </summary>
    void GoProsess()
    {
        battleEventUIControl.Go();
        Invoke("BattleStartProess", goChangeBattleStartTim);
    }

    /// <summary>
    /// バトルスタート処理
    /// </summary>
    void BattleStartProess()
    {
        battleEventUIControl.NoImg();
        foreach(BasePilot pilot in battlePilots)
        {
            pilot.StartProsess();
        }
    }

    /// <summary>
    /// バトルセッティング
    /// </summary>
    void BattleSetting()
    {
        // チームコスト設定
        teamRedCost = battleInfo.teamRedCost;
        teamBlueCost = battleInfo.teamBlueCost;
    }

    /// <summary>
    /// パイロットセッティング
    /// </summary>
    void PilotSetting()
    {
        for (int i = 0; i < battleInfo.pilotsInfo.Count; i++)
        {
            PilotInfo pilotInfo = battleInfo.pilotsInfo[i];
            if (pilotInfo.teamId == Team.None)
            {
                // 出場していない
                continue;
            }

            BasePilot pilot;
            // Human
            if (pilotInfo.playerType == PlayerType.Human)
            {
                pilot = Instantiate(gameManager.GetPilotPrefab(true));

            }
            // CPU
            else
            {
                pilot = Instantiate(gameManager.GetPilotPrefab(false));
            }

            // 機体選び
            BaseMs ms = null;
            switch (pilotInfo.useMs)
            {
                case MsList.Gundam:
                    ms = Instantiate(gameManager.GetMsPrefab((int)MsList.Gundam));
                    break;
            }
            pilot.SetMyMs(ms);
            ms.transform.parent = pilot.transform;

            // チーム追加
            if (pilotInfo.teamId == Team.Read)
            {
                redTeamPilots.Add(pilot);
            }
            else
            {
                blueTeamPilots.Add(pilot);
            }

            // リストに追加
            battlePilots.Add(pilot);
        }

        // 敵チームの設定
        foreach(BasePilot pilot in redTeamPilots)
        {
            foreach(BasePilot enemyPilot in blueTeamPilots)
            {
                pilot.SetEnemyPilot(enemyPilot);
            }
        }
        foreach (BasePilot pilot in blueTeamPilots)
        {
            foreach (BasePilot enemyPilot in redTeamPilots)
            {
                pilot.SetEnemyPilot(enemyPilot);
            }
        }


        // パイロットの初期化
        foreach (BasePilot pilot in battlePilots)
        {
            pilot.Initialize();
        }
    }

    /// <summary>
    /// 機体を最初の位置に設定
    /// </summary>
    void MsStartPosition()
    {
        respones = mapManager.responTrs;
        foreach(BasePilot pilot in redTeamPilots)
        {
            pilot.myMs.transform.SetPositionAndRotation(respones[0].position, respones[0].rotation);
        }
        foreach (BasePilot pilot in blueTeamPilots)
        {
            pilot.myMs.transform.SetPositionAndRotation(respones[1].position, respones[1].rotation);
        }
    }

}

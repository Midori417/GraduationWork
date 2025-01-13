using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// バトル管理クラス
/// </summary>
public class BattleManager : MonoBehaviour
{
    // 赤チームのコスト
    int teamRedCost;

    // 青チームのコスト
    int teamBlueCost;

    [SerializeField, Header("人間パイロットプレハブ")]
    private HumanPilot pfb_humanPilot;

    [SerializeField, Header("コンピュータパイロットプレハブ")]
    private CpuPilot pfb_cpuPilot;

    [SerializeField, Header("戦うパイロット")]
    private List<BasePilot> pilots = new List<BasePilot>();

    // 赤チームパイロット
    private List<BasePilot> redTeamPilots = new List<BasePilot>();

    // 青チームパイロット
    private List<BasePilot> blueTeamPilots = new List<BasePilot>();

    [SerializeField, Header("マップ")]
    private MapManager mapManager;

    [SerializeField, Header("機体のプレハブ")]
    private BaseMs[] pfb_ms;

    BattleInfo battleInfo;

    /// <summary>
    /// スタートイベント
    /// </summary>
    private void Start()
    {
        // バトル情報を取得
        //BattleInfo battleInfo = GameManager.instance.battleInfo;
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
                pilot = Instantiate(pfb_humanPilot);

            }
            // CPU
            else
            {
                pilot = Instantiate(pfb_cpuPilot);
            }

            // 機体選び
            BaseMs ms = null;
            switch (pilotInfo.useMs)
            {
                case MsList.Gundam:
                    ms = (Instantiate(pfb_ms[(int)MsList.Gundam]));
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
            pilots.Add(pilot);
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
        foreach (BasePilot pilot in pilots)
        {
            pilot.Initialize();
        }
    }
}

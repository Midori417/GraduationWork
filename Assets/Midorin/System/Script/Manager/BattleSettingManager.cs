using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// バトル設定画面管理クラス
/// </summary>
public class BattleSettingManager : MonoBehaviour
{
    [SerializeField, Header("フェードオブジェクト")]
    private FadeOut fadeOut;

    [SerializeField, Header("切り替えシーンの名前")]
    private string sceneName;

    [SerializeField, Header("Test(escapeを押したときのシーン)")]
    private string escapeSceneName;

    [SerializeField, Header("バトル情報")]
    private BattleInfo battleInfo;

    /// <summary>
    /// スタートイベント
    /// </summary>
    private void Start()
    {
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

    void Update()
    {
        BtnUpdate();
    }

    /// <summary>
    /// ボタンの更新
    /// </summary>
    void BtnUpdate()
    {
        // テスト用
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!fadeOut)
            {
                Debug.LogError("フェードオブジェクトが存在しません");
            }
            else
            {
                fadeOut.FadeStrt(escapeSceneName);
            }
        }

    }

    /// <summary>
    /// BattleStartボタンを押されたときの処理
    /// </summary>
    public void PushBattleStart()
    {
        if (!fadeOut)
        {
            Debug.LogError("フェードオブジェクトが存在しません");
            return;
        }

        if (!BattleInfoCheck())
        {
            Debug.Log("バトル情報が足りてないよ");
            return;
        }

        // ゲームマネージャーにバトル情報を伝える
        GameManager.instance.SetBattleInfo(battleInfo);

        fadeOut.FadeStrt(sceneName);
    }

    /// <summary>
    /// バトル情報が十分か
    /// </summary>
    /// <returns>
    /// true 十分
    /// false 不十分
    /// </returns>
    bool BattleInfoCheck()
    {
        // 2000以下の場合
        if (battleInfo.teamBlueCost < 2000 || battleInfo.teamRedCost < 2000)
        {
            Debug.Log("コストが足りていない");
            return false;
        }
        int teamRed = 0;
        int teamBlue = 0;
        bool noMs = false;
        foreach (PilotInfo pilotInfo in battleInfo.pilotsInfo)
        {
            // 出場しません
            if(pilotInfo.teamId == Team.None)
            {
                continue;
            }

            if (pilotInfo.teamId == Team.Read)
            {
                teamRed++;
            }
            else if (pilotInfo.teamId == Team.Blue)
            {
                teamBlue++;
            }

            if(pilotInfo.useMs == MsList.None)
            {
                noMs = true;
            }
        }

        // 機体が未設定のやつがいます
        if (noMs)
        {
            Debug.Log("機体が未設定");
            return false;
        }

        // プレイヤーの数が足りません
        if (teamRed + teamBlue < 2)
        {
            Debug.Log("プレイヤーが足りていない");
            return false;
        }
        // チームの必要人数が足りません
        if(teamBlue < 1 && teamRed < 1)
        {
            Debug.Log("チームに必要な人数が足りていない");
            return false;
        }

        return true;
    }
}

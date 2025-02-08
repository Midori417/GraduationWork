using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// バトル設定画面管理クラス
/// </summary>
public class BattleSettingManager : MonoBehaviour
{
    [SerializeField, Header("フェードオブジェクト")]
    private FadeOut _fadeOut;

    [SerializeField, Header("切り替えシーンの名前")]
    private string _sceneName;

    [SerializeField, Header("Test(escapeを押したときのシーン)")]
    private string _escapeSceneName;

    [SerializeField, Header("バトル情報")]
    private BattleInfo _battleInfo;

    [SerializeField]
    private AudioSource _bgmSouce;

    int _selctNum = 1;

    [SerializeField]
    private List<Image> _selectedImages;
    [SerializeField]
    private List<Button> _selectedButtons;

    Color _noramColor = new Color(0.4f, 0.4f, 0.4f);
    Color _hightColor = Color.white;
    bool isOn = false;

    /// <summary>
    /// スタートイベント
    /// </summary>
    private void Start()
    {
        // テスト
        _battleInfo.teamRedCost = GameManager.teamCostMax;
        _battleInfo.teamBlueCost = GameManager.teamCostMax;
        _battleInfo.time = 5;
        {
            PilotInfo pilotInfo;
            pilotInfo.teamId = Team.Red;
            pilotInfo.playerType = PlayerType.Human;
            pilotInfo.useMs = MsList.Gundam;
            _battleInfo.pilotsInfo.Add(pilotInfo);
        }
        {
            PilotInfo pilotInfo;
            pilotInfo.teamId = Team.Blue;
            pilotInfo.playerType = PlayerType.Cpu;
            pilotInfo.useMs = MsList.Gundam;
            _battleInfo.pilotsInfo.Add(pilotInfo);
        }

        {
            PilotInfo pilotInfo;
            pilotInfo.teamId = Team.None;
            pilotInfo.playerType = PlayerType.Cpu;
            pilotInfo.useMs = MsList.Gundam;
            _battleInfo.pilotsInfo.Add(pilotInfo);
        }
        {
            PilotInfo pilotInfo;
            pilotInfo.teamId = Team.None;
            pilotInfo.playerType = PlayerType.Cpu;
            pilotInfo.useMs = MsList.Gundam;
            _battleInfo.pilotsInfo.Add(pilotInfo);
        }


    }

    void Update()
    {
        ControlUpdate();
    }

    /// <summary>
    /// 操作の更新
    /// </summary>
    void ControlUpdate()
    {
        if (isOn) return;
        // 一個前に戻る
        if (Gamepad.current == null)
        {
            // テスト用
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _fadeOut.FadeStrt(Global._selectScene);
                isOn = true;
            }
            foreach (Button button in _selectedButtons)
            {
                button.enabled = true;
            }
        }
        else
        {
            if (Gamepad.current.crossButton.wasPressedThisFrame)
            {
                isOn = true;
                PushBattleStart();
            }
            else if (Gamepad.current.circleButton.wasPressedThisFrame)
            {
                isOn = true;
                _fadeOut.FadeStrt(Global._selectScene);
            }
            foreach(Button button in _selectedButtons)
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
    /// BattleStartボタンを押されたときの処理
    /// </summary>
    public void PushBattleStart()
    {
        if (!_fadeOut)
        {
            Debug.LogError("フェードオブジェクトが存在しません");
            return;
        }

        if (!BattleInfoCheck())
        {
            Debug.Log("バトル情報が足りてないよ");
            return;
        }
        _bgmSouce.Stop();
        GetComponent<AudioSource>().Play();

        // ゲームマネージャーにバトル情報を伝える
        GameManager.I.SetBattleInfo(_battleInfo);

        _fadeOut.FadeStrt(_sceneName);
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
        if (_battleInfo.teamBlueCost < 2000 || _battleInfo.teamRedCost < 2000)
        {
            Debug.Log("コストが足りていない");
            return false;
        }
        int teamRed = 0;
        int teamBlue = 0;
        bool noMs = false;
        foreach (PilotInfo pilotInfo in _battleInfo.pilotsInfo)
        {
            // 出場しません
            if (pilotInfo.teamId == Team.None)
            {
                continue;
            }

            if (pilotInfo.teamId == Team.Red)
            {
                teamRed++;
            }
            else if (pilotInfo.teamId == Team.Blue)
            {
                teamBlue++;
            }

            if (pilotInfo.useMs == MsList.None)
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
        if (teamBlue < 1 && teamRed < 1)
        {
            Debug.Log("チームに必要な人数が足りていない");
            return false;
        }

        return true;
    }
}

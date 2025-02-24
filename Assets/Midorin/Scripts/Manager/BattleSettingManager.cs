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

        _bgmSouce.Stop();
        GetComponent<AudioSource>().Play();

        _fadeOut.FadeStrt(_sceneName);
    }
}

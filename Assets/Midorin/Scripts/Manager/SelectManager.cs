using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// 選択画面管理クラス
/// </summary>
public class SelectManager : MonoBehaviour
{
    [SerializeField, Header("フェードオブジェクト")]
    private FadeOut _fadeOut;

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

    private void Update()
    {
        if (isOn) return;

        if (Gamepad.current != null)
        {
            Gamepad gamepad = Gamepad.current;
            // 上下の切り替え
            if (gamepad.dpad.up.wasPressedThisFrame)
            {
                _selctNum++;
            }
            if (gamepad.dpad.down.wasPressedThisFrame)
            {
                _selctNum--;
            }
            _selctNum = Mathf.Clamp(_selctNum, 0, 1);
            // 決定押したとき
            if (gamepad.crossButton.wasPressedThisFrame)
            {
                isOn = true;
                if (_selctNum == 1)
                {
                    PushBattle();
                }
                else
                {
                    PushExit();
                }
            }
            else if(gamepad.circleButton.wasPressedThisFrame)
            {
                _fadeOut.FadeStrt(Global._titleScene);
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
        else
        {
            foreach (Button button in _selectedButtons)
            {
                button.enabled = true;
            }
        }
    }

    /// <summary>
    /// Battleボタンを選択したときの処理
    /// </summary>
    public void PushBattle()
    {
        if (!_fadeOut)
        {
            Debug.LogError("フェードオブジェクトが存在しません");
            return;
        }
        _fadeOut.FadeStrt(Global._battleSettingScene);
        _bgmSouce.Stop();
        GetComponent<AudioSource>().Play();
    }

    /// <summary>
    /// Exitボタンが選択されたときの処理
    /// </summary>
    public void PushExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
    }
}

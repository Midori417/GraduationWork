using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 選択画面管理クラス
/// </summary>
public class SelectManager : MonoBehaviour
{
    [SerializeField, Header("フェードオブジェクト")]
    private FadeOut _fadeOut;

    [SerializeField, Header("Battleボタンをしたときのシーン切り替えの名前")]
    private string _battleBtnSceneName;

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
        _fadeOut.FadeStrt(_battleBtnSceneName);
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

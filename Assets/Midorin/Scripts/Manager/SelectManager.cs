using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 選択画面管理クラス
/// </summary>
public class SelectManager : MonoBehaviour
{
    [SerializeField, Header("フェードオブジェクト")]
    private FadeOut fadeOut;

    [SerializeField, Header("Battleボタンをしたときのシーン切り替えの名前")]
    private string battleBtnSceneName;

    /// <summary>
    /// Battleボタンを選択したときの処理
    /// </summary>
    public void PushBattle()
    {
        if (!fadeOut)
        {
            Debug.LogError("フェードオブジェクトが存在しません");
            return;
        }
        fadeOut.FadeStrt(battleBtnSceneName);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タイトル管理クラス
/// </summary>
public class TitleManager : MonoBehaviour
{
    [SerializeField, Header("フェードオブジェクト")]
    private FadeOut fadeOut;

    [SerializeField, Header("切り替えシーンの名前")]
    private string sceneName;

    /// <summary>
    /// pleaseボタンを押されたときの処理
    /// </summary>
    public void PushPleaseButton()
    {
        if(!fadeOut)
        {
            Debug.LogError("フェードオブジェクトが存在しません");
            return;
        }
        fadeOut.FadeStrt(sceneName);
    }
}

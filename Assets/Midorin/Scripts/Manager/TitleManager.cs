using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タイトル管理クラス
/// </summary>
public class TitleManager : MonoBehaviour
{
    [SerializeField, Header("フェードオブジェクト")]
    private FadeOut _fadeOut;

    [SerializeField, Header("切り替えシーンの名前")]
    private string _sceneName;

    /// <summary>
    /// pleaseボタンを押されたときの処理
    /// </summary>
    public void PushPleaseButton()
    {
        if(!_fadeOut)
        {
            Debug.LogError("フェードオブジェクトが存在しません");
            return;
        }
        _fadeOut.FadeStrt(_sceneName);
    }
}

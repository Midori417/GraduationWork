using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// バトルメインUI切り替え
/// 操作はバトルマネージャにさせる
/// </summary>
public class BattleEventUIControl : MonoBehaviour
{
    [SerializeField, Header("")]
    private Sprite stanby;

    [SerializeField, Header("")]
    private Sprite go;

    [SerializeField, Header("")]
    private Sprite win;

    [SerializeField, Header("")]
    private Sprite lose;

    private Image img;

    private void Start()
    {
        img = GetComponent<Image>();

        img.enabled = false;
    }

    /// <summary>
    /// 画像の表示と切り替え
    /// </summary>
    /// <param name="sprite"></param>
    private void ImgChange(Sprite sprite)
    {
        if (!sprite) return;
        img.enabled = true;
        img.sprite = sprite;
    }

    #region BattleManagerで操作する関数
    /// <summary>
    /// スタンバイ
    /// </summary>
    public void Stanby()
    {
        ImgChange(stanby);
    }

    /// <summary>
    /// ゴー
    /// </summary>
    public void Go()
    {
        ImgChange(go);
    }

    /// <summary>
    /// 勝利
    /// </summary>
    public void Win()
    {
        ImgChange(win);
    }

    /// <summary>
    /// 敗北
    /// </summary>
    public void Lose()
    {
        ImgChange(lose);
    }

    /// <summary>
    /// 画像を表示しない
    /// </summary>
    public void NoImg()
    {
        img.enabled = false;
    }
    #endregion
}

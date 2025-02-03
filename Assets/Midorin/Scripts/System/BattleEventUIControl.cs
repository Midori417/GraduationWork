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
    private Sprite _stanby;

    [SerializeField, Header("")]
    private Sprite _go;

    [SerializeField, Header("")]
    private Sprite _win;

    [SerializeField, Header("")]
    private Sprite _lose;

    private Image _img;

    /// <summary>
    /// 生成時に実行
    /// </summary>
    private void Awake()
    {
        _img = GetComponent<Image>();

        _img.enabled = false;
    }

    /// <summary>
    /// 画像の表示と切り替え
    /// </summary>
    /// <param name="sprite"></param>
    private void ImgChange(Sprite sprite)
    {
        if (!sprite) return;
        _img.enabled = true;
        _img.sprite = sprite;
    }

    #region BattleManagerで操作する関数
    /// <summary>
    /// スタンバイ
    /// </summary>
    public void Stanby()
    {
        ImgChange(_stanby);
    }

    /// <summary>
    /// ゴー
    /// </summary>
    public void Go()
    {
        ImgChange(_go);
    }

    /// <summary>
    /// 勝利
    /// </summary>
    public void Win()
    {
        ImgChange(_win);
    }

    /// <summary>
    /// 敗北
    /// </summary>
    public void Lose()
    {
        ImgChange(_lose);
    }

    /// <summary>
    /// 画像を表示しない
    /// </summary>
    public void NoImg()
    {
        _img.enabled = false;
    }
    #endregion
}

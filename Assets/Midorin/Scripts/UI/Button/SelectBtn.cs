using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ボタンの基底クラス
/// </summary>
public class SelectBtn : MonoBehaviour
{
    private Image _img;

    private Color _selectColor;
    private Color _noSelectColor = new Color(0.5f, 0.5f, 0.5f, 1);

    protected virtual void Awake()
    {
        _img = GetComponent<Image>();
        _selectColor = _img.color;
    }

    /// <summary>
    /// 選択されたときの処理
    /// </summary>
    public virtual void Select()
    {

    }

    /// <summary>
    /// 選択時のカラー
    /// </summary>
    public void SelectColor()
    {
        _img.color = _selectColor;
    }

    /// <summary>
    /// 非選択時のカラー
    /// </summary>
    public void NoSelectColor()
    {
        if(!_img)
        {
            _img = GetComponent<Image>();
            _selectColor = _img.color;
        }
        _img.color *= _noSelectColor;
    }
}

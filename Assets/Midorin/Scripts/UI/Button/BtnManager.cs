using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnManager : MonoBehaviour
{
    [SerializeField, Header("ボタン配列")]
    SelectBtn[] _buttons;

    // 選んでいるボタン
    private int _btnIndex = 0;

    /// <summary>
    ///  生成時に呼び出される
    /// </summary>
    private void Awake()
    {
        _buttons = new SelectBtn[transform.childCount];
        _buttons = GetComponentsInChildren<SelectBtn>();
        foreach (SelectBtn btn in _buttons)
        {
            btn.NoSelectColor();
        }
        _buttons[_btnIndex].SelectColor();
    }

    /// <summary>
    /// ボタンをリセット
    /// </summary>
    public void SelectReset()
    {
        _buttons[_btnIndex].NoSelectColor();
        _btnIndex = 0;
        _buttons[_btnIndex].SelectColor();
    }

    /// <summary>
    /// ボタンを選択
    /// </summary>
    public void SelectBtn()
    {
        _buttons[_btnIndex].Select();
    }

    /// <summary>
    /// ボタンを進める
    /// </summary>
    public void SelectAdd()
    {
        if(_btnIndex == _buttons.Length-1)
        {
            return;
        }
        _buttons[_btnIndex].NoSelectColor();
        _btnIndex++;
        _buttons[_btnIndex].SelectColor();
    }

    /// <summary>
    /// ボタンを戻す
    /// </summary>
    public void SelectDown()
    {
        if (_btnIndex == 0)
        {
            return;
        }
        _buttons[_btnIndex].NoSelectColor();
        _btnIndex--;
        _buttons[_btnIndex].SelectColor();
    }
}

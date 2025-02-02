using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI管理クラス
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField, Header("ブーストゲージ")]
    private Image _imgBoostGauge;

    [SerializeField, Header("自身の機体の体力")]
    private Text _txtHp;

    [SerializeField, Header("武装の弾")]
    private List<Text> _txtArmedValues;

    /// <summary>
    /// ブーストゲージの設定
    /// </summary>
    /// <param name="value">現在のブースト容量(0～1)</param>
    public void BoostGauge(float value)
    {
        if (_imgBoostGauge)
        {
            _imgBoostGauge.fillAmount = value;
        }
    }

    /// <summary>
    /// 武装の残弾を設定
    /// </summary>
    /// <param name="value"></param>
    public void ArmedValue(int index, int _value)
    {
        if(_txtArmedValues.Count-1 < index)
        {
            return;
        }

        if (_txtArmedValues[index])
        {
            _txtArmedValues[index].text = _value.ToString();
        }
    }

    /// <summary>
    /// 体力の設定
    /// </summary>
    /// <param name="value"></param>
    public void Hp(int value)
    {
        if (_txtHp)
        {
            _txtHp.text = value.ToString();
        }
    }
}

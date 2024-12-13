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
    private Image img_BoostGauge;

    [SerializeField, Header("自身の機体の体力")]
    private Text txt_Hp;

    [SerializeField, Header("武装の弾")]
    private Text txt_ArmedValue;

    /// <summary>
    /// ブーストゲージの設定
    /// </summary>
    /// <param name="value">現在のブースト容量(0～1)</param>
    public void BoostGauge(float value)
    {
        if (img_BoostGauge)
        {
            img_BoostGauge.material.SetFloat("_FillAmount", value);
        }
    }

    /// <summary>
    /// 武装の残弾を設定
    /// </summary>
    /// <param name="value"></param>
    public void ArmedValue(int _value)
    {
        if(txt_ArmedValue)
        {
            txt_ArmedValue.text = _value.ToString();
        }
    }

    /// <summary>
    /// 体力の設定
    /// </summary>
    /// <param name="value"></param>
    public void Hp(int value)
    {
        if (txt_Hp)
        {
            txt_Hp.text = value.ToString("000");
        }
    }
}

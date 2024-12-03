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

    [SerializeField, Header("耐久値")]
    private Text txt_DurabilityValue;

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
    /// 耐久値の設定
    /// </summary>
    /// <param name="value"></param>
    public void DurabilityValue(int value)
    {
        if (txt_DurabilityValue)
        {
            txt_DurabilityValue.text = value.ToString("000");
        }
    }
}

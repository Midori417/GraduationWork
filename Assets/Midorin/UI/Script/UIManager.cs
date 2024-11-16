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

    #region Test

    [SerializeField, Header("(Test)ブースト容量")]
    private float boost;

    [SerializeField, Header("ブースト最大量")]
    private float boostMax = 100;

    #endregion

    /// <summary>
    /// ブーストゲージの設定
    /// </summary>
    /// <param name="value">現在のブースト容量</param>
    public void BoostGauge(float value)
    {
        if (img_BoostGauge)
        {
            float fillAmout = Mathf.Clamp01((boostMax - (boostMax - value)) / boostMax);
            img_BoostGauge.material.SetFloat("_FillAmount", fillAmout);
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

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

    #region Test

    [SerializeField, Header("(Test)ブースト容量")]
    private float boost;

    [SerializeField, Header("ブースト最大量")]
    private float boostMax = 100;

    #endregion

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        BoostGauge(boost);
    }

    /// <summary>
    /// ブーストゲージの設定
    /// </summary>
    /// <param name="value">現在のブースト容量</param>
    void BoostGauge(float value)
    {
        float fillAmout = Mathf.Clamp01((boostMax - (boostMax - value)) / boostMax);
        img_BoostGauge.material.SetFloat("_FillAmount", fillAmout);
    }
}

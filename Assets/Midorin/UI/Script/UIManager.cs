using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI�Ǘ��N���X
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField, Header("�u�[�X�g�Q�[�W")]
    private Image img_BoostGauge;

    [SerializeField, Header("�ϋv�l")]
    private Text txt_DurabilityValue;

    #region Test

    [SerializeField, Header("(Test)�u�[�X�g�e��")]
    private float boost;

    [SerializeField, Header("�u�[�X�g�ő��")]
    private float boostMax = 100;

    #endregion

    /// <summary>
    /// �u�[�X�g�Q�[�W�̐ݒ�
    /// </summary>
    /// <param name="value">���݂̃u�[�X�g�e��</param>
    public void BoostGauge(float value)
    {
        if (img_BoostGauge)
        {
            float fillAmout = Mathf.Clamp01((boostMax - (boostMax - value)) / boostMax);
            img_BoostGauge.material.SetFloat("_FillAmount", fillAmout);
        }
    }

    /// <summary>
    /// �ϋv�l�̐ݒ�
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

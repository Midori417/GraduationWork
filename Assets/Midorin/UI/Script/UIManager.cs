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

    /// <summary>
    /// �u�[�X�g�Q�[�W�̐ݒ�
    /// </summary>
    /// <param name="value">���݂̃u�[�X�g�e��(0�`1)</param>
    public void BoostGauge(float value)
    {
        if (img_BoostGauge)
        {
            img_BoostGauge.material.SetFloat("_FillAmount", value);
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

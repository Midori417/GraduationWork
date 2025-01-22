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

    [SerializeField, Header("���g�̋@�̗̂̑�")]
    private Text txt_Hp;

    [SerializeField, Header("�����̒e")]
    private List<Text> txt_ArmedValues;

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
    /// �����̎c�e��ݒ�
    /// </summary>
    /// <param name="value"></param>
    public void ArmedValue(int index, int _value)
    {
        if(txt_ArmedValues.Count-1 < index)
        {
            return;
        }

        if (txt_ArmedValues[index])
        {
            txt_ArmedValues[index].text = _value.ToString();
        }
    }

    /// <summary>
    /// �̗͂̐ݒ�
    /// </summary>
    /// <param name="value"></param>
    public void Hp(int value)
    {
        if (txt_Hp)
        {
            txt_Hp.text = value.ToString();
        }
    }
}

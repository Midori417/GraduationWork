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

    #region Test

    [SerializeField, Header("(Test)�u�[�X�g�e��")]
    private float boost;

    [SerializeField, Header("�u�[�X�g�ő��")]
    private float boostMax = 100;

    #endregion

    /// <summary>
    /// �X�V
    /// </summary>
    private void Update()
    {
        BoostGauge(boost);
    }

    /// <summary>
    /// �u�[�X�g�Q�[�W�̐ݒ�
    /// </summary>
    /// <param name="value">���݂̃u�[�X�g�e��</param>
    void BoostGauge(float value)
    {
        float fillAmout = Mathf.Clamp01((boostMax - (boostMax - value)) / boostMax);
        img_BoostGauge.material.SetFloat("_FillAmount", fillAmout);
    }
}

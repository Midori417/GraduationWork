using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPilot : BasePilot
{
    private void Start()
    {
        var ms = gameObject.GetComponentInChildren<BaseMs>();
        SetMyMs(ms);
    }

    /// <summary>
    /// �X�V�C�x���g
    /// </summary>
    private void Update()
    {
        UIUpdate();
    }

    /// <summary>
    /// UI�̏�Ԃ��X�V
    /// </summary>
    void UIUpdate()
    {
        if(myUImanager)
        {
            myUImanager.BoostGauge(myMs.get_BoostCurrent);
            myUImanager.DurabilityValue(myMs.get_StrengthValue);
        }
    }
}

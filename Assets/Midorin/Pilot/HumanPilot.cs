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
    /// 更新イベント
    /// </summary>
    private void Update()
    {
        UIUpdate();
    }

    /// <summary>
    /// UIの状態を更新
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

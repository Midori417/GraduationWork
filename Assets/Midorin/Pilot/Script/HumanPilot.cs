using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 操作できるパイロット
/// </summary>
public class HumanPilot : BasePilot
{
    [SerializeField, Header("UI")]
    private UIManager uiManager;

    void Start()
    {
        base.Initialize();
        uiManager = GetComponentInChildren<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!myMs)
        {
            return;
        }

        UIUpdate();
    }

    /// <summary>
    /// UIの更新
    /// </summary>
    void UIUpdate()
    {
        if(!uiManager)
        {
            return;
        }
        uiManager.BoostGauge(myMs.boost01);
        uiManager.Hp(myMs.hp);
        uiManager.ArmedValue(myMs.uiArmedValue[0]);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ł���p�C���b�g
/// </summary>
public class HumanPilot : BasePilot
{
    [SerializeField, Header("���g�̋@��")]
    private BaseMs myMs;

    [SerializeField, Header("UI")]
    private UIManager uiManager;

    void Start()
    {
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
    /// UI�̍X�V
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

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

        ControlUpdate();
        UIUpdate();
    }

    /// <summary>
    /// 入力コントール
    /// </summary>
    void ControlUpdate()
    {
        // 仮キー入力
        myMs.moveAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        myMs.isJumpBtn = Input.GetKey(KeyCode.Space);
        myMs.isDashBtn = Input.GetKey(KeyCode.LeftShift);
        myMs.isMainShotBtn = Input.GetKeyDown(KeyCode.Mouse0);
        myMs.isSubShotBtn = Input.GetKeyDown(KeyCode.Alpha1);
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

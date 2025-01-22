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


    #region イベント

    void Start()
    {
        if (!uiManager)
        {
            uiManager = GetComponentInChildren<UIManager>();
        }
        uiManager.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!ProessCheck()) return;

        MsSetControl();
        UIProsess();
    }

    #endregion

    /// <summary>
    /// 処理が可能かチェック
    /// </summary>
    /// <returns></returns>
    bool ProessCheck()
    {
        if (!isProsess)
        {
            return false;
        }
        if (!myMs)
        {
            Debug.LogError("機体が存在しません");
            return false;
        }
        return true;
    }

    /// <summary>
    /// 処理開始
    /// </summary>
    public override void StartProsess()
    {
        base.StartProsess();
        uiManager.gameObject.SetActive(true);
    }

    /// <summary>
    /// 機体に入力を伝える
    /// </summary>
    void MsSetControl()
    {
        DestoryMsProsess();

        // 仮キー入力
        myMs.moveAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        myMs.isJumpBtn = Input.GetKey(KeyCode.Space);
        myMs.isDashBtn = Input.GetKey(KeyCode.LeftShift);
        myMs.isMainShotBtn = Input.GetKeyDown(KeyCode.Mouse0);
        myMs.isSubShotBtn = Input.GetKeyDown(KeyCode.E);
    }

    /// <summary>
    /// UI処理
    /// </summary>
    void UIProsess()
    {
        if (!uiManager)
        {
            return;
        }
        uiManager.BoostGauge(myMs.boost01);
        uiManager.Hp(myMs.hp);

        for (int i = 0; i < myMs.uiArmed.Count; i++)
        {
            uiManager.ArmedValue(i, myMs.uiArmed[i].amo);
        }
    }
}

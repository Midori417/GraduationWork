using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 操作できるパイロット
/// </summary>
public class HumanPilot : BasePilot
{
    [SerializeField, Header("UIマネージャー")]
    private UIManager _uiManager;

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    private void Update()
    {
        if (isStop) return;

        MsUpdate();
        UIUpdate();
    }

    /// <summary>
    /// オブジェクトの処理を開始
    /// </summary>
    public override void Play()
    {
        base.Play();
        _uiManager.gameObject.SetActive(true);
    }

    /// <summary>
    /// 機体の更新
    /// </summary>
    private void MsUpdate()
    {
        
    }

    /// <summary>
    /// UIの更新
    /// </summary>
    private void UIUpdate()
    {
        if (!_uiManager) return;

        _uiManager.Hp(myMs.hp);
        _uiManager.BoostGauge(myMs.boost01);
        _uiManager.StrengthGauge(team);

        for (int i = 0; i < myMs.amoCount; ++i)
        {
            _uiManager.ArmedValue(i, myMs.GetAmo(i));
        }
    }
}

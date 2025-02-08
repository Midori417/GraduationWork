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
        TargetUpdate();
    }

    /// <summary>
    /// オブジェクトの処理を開始
    /// </summary>
    public override void Play()
    {
        base.Play();
        _uiManager.Play();
    }

    /// <summary>
    /// オブジェクトの処理を停止
    /// </summary>
    public override void Stop()
    {
        base.Stop();
        _uiManager.Stop();
    }

    /// <summary>
    /// 機体の更新
    /// </summary>
    protected override void MsUpdate()
    {
        base.MsUpdate();
        MsInput();
    }

    /// <summary>
    /// 機体の入力
    /// </summary>
    private void MsInput()
    {
        msInput.Update();
        //msInput._move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //msInput._jump = Input.GetKey(KeyCode.Space);
        //msInput._dash = Input.GetKey(KeyCode.LeftShift);
        //msInput._mainShot = Input.GetKeyDown(KeyCode.Mouse0);
        //msInput._subShot = Input.GetKeyDown(KeyCode.E);
        //msInput._mainAttack = Input.GetKeyDown(KeyCode.Mouse1);
        //msInput._targetChange = Input.GetKeyDown(KeyCode.Mouse2);

        myMs.msInput = msInput;
    }

    /// <summary>
    /// UIの更新
    /// </summary>
    private void UIUpdate()
    {
        if (!_uiManager) return;

        _uiManager.SetHp((int)myMs.hp, myMs.hpRate);
        _uiManager.BoostGauge(myMs.boostRate);
        _uiManager.StrengthGauge(team);

        if (targetMs)
        {
            Vector3 pos = cameraManager.mainCamera.WorldToScreenPoint(targetMs.center.position);
            if (!targetMs.isDamageOk)
            {
                _uiManager.SetTargetMark(TargetType.Yellow, pos);
            }
            else if (myMs.isLookDistance)
            {
                _uiManager.SetTargetMark(TargetType.LookOn, pos);
            }
            else if (myMs.isRedDistance)
            {
                _uiManager.SetTargetMark(TargetType.Red, pos);
            }
            else
            {
                _uiManager.SetTargetMark(TargetType.Green, pos);
            }
        }
        // エネミー体力
        for (int i = 0; i < enemyPilots.Count; ++i)
        {
            BaseMs ms = enemyPilots[i].myMs;
            Vector3 pos = cameraManager.mainCamera.WorldToScreenPoint(ms.center.position);
            Vector3 viewportPosition = cameraManager.mainCamera.WorldToViewportPoint(ms.transform.position);

            // 画面内にあるかチェック
            bool isVisible = viewportPosition.z > 0 && viewportPosition.x > 0 && viewportPosition.x < 1 &&
                             viewportPosition.y > 0 && viewportPosition.y < 1;
            _uiManager.SetEnemHp(i, ms.hpRate, pos, isVisible);
        }
        if (teamPilot)
        {
            BaseMs ms = teamPilot.myMs;
            Vector3 pos = cameraManager.mainCamera.WorldToScreenPoint(ms.center.position);
            Vector3 viewportPosition = cameraManager.mainCamera.WorldToViewportPoint(ms.transform.position);

            // 画面内にあるかチェック
            bool isVisible = viewportPosition.z > 0 && viewportPosition.x > 0 && viewportPosition.x < 1 &&
                             viewportPosition.y > 0 && viewportPosition.y < 1;

            _uiManager.SetPartnerHpBar(ms.hpRate, pos, isVisible);
            _uiManager.SetPartnerHp(ms.hp, ms.hpRate);
        }

        for (int i = 0; i < myMs.amoCount; ++i)
        {
            _uiManager.ArmedValue(i, myMs.GetAmo(i), myMs.GetAmoRate(i));
        }
    }

    /// <summary>
    /// 勝敗を設定
    /// </summary>
    /// <param name="victory"></param>
    public override void SetVitory(Victory victory)
    {
        base.SetVitory(victory);
        if (!_uiManager) return;

        _uiManager.SetVictory(victory);
    }
}

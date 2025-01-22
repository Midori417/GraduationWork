using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ł���p�C���b�g
/// </summary>
public class HumanPilot : BasePilot
{
    [SerializeField, Header("UI")]
    private UIManager uiManager;


    #region �C�x���g

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
    /// �������\���`�F�b�N
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
            Debug.LogError("�@�̂����݂��܂���");
            return false;
        }
        return true;
    }

    /// <summary>
    /// �����J�n
    /// </summary>
    public override void StartProsess()
    {
        base.StartProsess();
        uiManager.gameObject.SetActive(true);
    }

    /// <summary>
    /// �@�̂ɓ��͂�`����
    /// </summary>
    void MsSetControl()
    {
        DestoryMsProsess();

        // ���L�[����
        myMs.moveAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        myMs.isJumpBtn = Input.GetKey(KeyCode.Space);
        myMs.isDashBtn = Input.GetKey(KeyCode.LeftShift);
        myMs.isMainShotBtn = Input.GetKeyDown(KeyCode.Mouse0);
        myMs.isSubShotBtn = Input.GetKeyDown(KeyCode.E);
    }

    /// <summary>
    /// UI����
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

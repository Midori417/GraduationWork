using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �o�g���ݒ��ʊǗ��N���X
/// </summary>
public class BattleSettingManager : MonoBehaviour
{
    [SerializeField, Header("�t�F�[�h�I�u�W�F�N�g")]
    private FadeOut fadeOut;

    [SerializeField, Header("�؂�ւ��V�[���̖��O")]
    private string sceneName;

    [SerializeField, Header("Test(escape���������Ƃ��̃V�[��)")]
    private string escapeSceneName;

    [SerializeField, Header("�o�g�����")]
    private BattleInfo battleInfo;

    /// <summary>
    /// �X�^�[�g�C�x���g
    /// </summary>
    private void Start()
    {
        // �e�X�g
        battleInfo.teamRedCost = GameManager.teamCostMax;
        battleInfo.teamBlueCost = GameManager.teamCostMax;
        battleInfo.time = 5;
        {
            PilotInfo pilotInfo;
            pilotInfo.teamId = Team.Read;
            pilotInfo.playerType = PlayerType.Human;
            pilotInfo.useMs = MsList.Gundam;
            battleInfo.pilotsInfo.Add(pilotInfo);
        }
        {
            PilotInfo pilotInfo;
            pilotInfo.teamId = Team.Blue;
            pilotInfo.playerType = PlayerType.Cpu;
            pilotInfo.useMs = MsList.Gundam;
            battleInfo.pilotsInfo.Add(pilotInfo);
        }

        {
            PilotInfo pilotInfo;
            pilotInfo.teamId = Team.None;
            pilotInfo.playerType = PlayerType.Cpu;
            pilotInfo.useMs = MsList.Gundam;
            battleInfo.pilotsInfo.Add(pilotInfo);
        }
        {
            PilotInfo pilotInfo;
            pilotInfo.teamId = Team.None;
            pilotInfo.playerType = PlayerType.Cpu;
            pilotInfo.useMs = MsList.Gundam;
            battleInfo.pilotsInfo.Add(pilotInfo);
        }


    }

    void Update()
    {
        BtnUpdate();
    }

    /// <summary>
    /// �{�^���̍X�V
    /// </summary>
    void BtnUpdate()
    {
        // �e�X�g�p
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!fadeOut)
            {
                Debug.LogError("�t�F�[�h�I�u�W�F�N�g�����݂��܂���");
            }
            else
            {
                fadeOut.FadeStrt(escapeSceneName);
            }
        }

    }

    /// <summary>
    /// BattleStart�{�^���������ꂽ�Ƃ��̏���
    /// </summary>
    public void PushBattleStart()
    {
        if (!fadeOut)
        {
            Debug.LogError("�t�F�[�h�I�u�W�F�N�g�����݂��܂���");
            return;
        }

        if (!BattleInfoCheck())
        {
            Debug.Log("�o�g����񂪑���ĂȂ���");
            return;
        }

        // �Q�[���}�l�[�W���[�Ƀo�g������`����
        GameManager.instance.SetBattleInfo(battleInfo);

        fadeOut.FadeStrt(sceneName);
    }

    /// <summary>
    /// �o�g����񂪏\����
    /// </summary>
    /// <returns>
    /// true �\��
    /// false �s�\��
    /// </returns>
    bool BattleInfoCheck()
    {
        // 2000�ȉ��̏ꍇ
        if (battleInfo.teamBlueCost < 2000 || battleInfo.teamRedCost < 2000)
        {
            Debug.Log("�R�X�g������Ă��Ȃ�");
            return false;
        }
        int teamRed = 0;
        int teamBlue = 0;
        bool noMs = false;
        foreach (PilotInfo pilotInfo in battleInfo.pilotsInfo)
        {
            // �o�ꂵ�܂���
            if(pilotInfo.teamId == Team.None)
            {
                continue;
            }

            if (pilotInfo.teamId == Team.Read)
            {
                teamRed++;
            }
            else if (pilotInfo.teamId == Team.Blue)
            {
                teamBlue++;
            }

            if(pilotInfo.useMs == MsList.None)
            {
                noMs = true;
            }
        }

        // �@�̂����ݒ�̂�����܂�
        if (noMs)
        {
            Debug.Log("�@�̂����ݒ�");
            return false;
        }

        // �v���C���[�̐�������܂���
        if (teamRed + teamBlue < 2)
        {
            Debug.Log("�v���C���[������Ă��Ȃ�");
            return false;
        }
        // �`�[���̕K�v�l��������܂���
        if(teamBlue < 1 && teamRed < 1)
        {
            Debug.Log("�`�[���ɕK�v�Ȑl��������Ă��Ȃ�");
            return false;
        }

        return true;
    }
}

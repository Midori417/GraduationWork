using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// �o�g���Ǘ��N���X
/// </summary>
public class BattleManager : MonoBehaviour
{
    // �ԃ`�[���̃R�X�g
    int teamRedCost;

    // �`�[���̃R�X�g
    int teamBlueCost;

    [SerializeField, Header("�l�ԃp�C���b�g�v���n�u")]
    private HumanPilot pfb_humanPilot;

    [SerializeField, Header("�R���s���[�^�p�C���b�g�v���n�u")]
    private CpuPilot pfb_cpuPilot;

    [SerializeField, Header("�키�p�C���b�g")]
    private List<BasePilot> pilots = new List<BasePilot>();

    // �ԃ`�[���p�C���b�g
    private List<BasePilot> redTeamPilots = new List<BasePilot>();

    // �`�[���p�C���b�g
    private List<BasePilot> blueTeamPilots = new List<BasePilot>();

    [SerializeField, Header("�}�b�v")]
    private MapManager mapManager;

    [SerializeField, Header("�@�̂̃v���n�u")]
    private BaseMs[] pfb_ms;

    BattleInfo battleInfo;

    /// <summary>
    /// �X�^�[�g�C�x���g
    /// </summary>
    private void Start()
    {
        // �o�g�������擾
        //BattleInfo battleInfo = GameManager.instance.battleInfo;
        {
            battleInfo.pilotsInfo = new List<PilotInfo>();
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

        BattleSetting();
        PilotSetting();
    }

    /// <summary>
    /// �o�g���Z�b�e�B���O
    /// </summary>
    void BattleSetting()
    {
        // �`�[���R�X�g�ݒ�
        teamRedCost = battleInfo.teamRedCost;
        teamBlueCost = battleInfo.teamBlueCost;
    }

    /// <summary>
    /// �p�C���b�g�Z�b�e�B���O
    /// </summary>
    void PilotSetting()
    {
        for (int i = 0; i < battleInfo.pilotsInfo.Count; i++)
        {
            PilotInfo pilotInfo = battleInfo.pilotsInfo[i];
            if (pilotInfo.teamId == Team.None)
            {
                // �o�ꂵ�Ă��Ȃ�
                continue;
            }

            BasePilot pilot;
            // Human
            if (pilotInfo.playerType == PlayerType.Human)
            {
                pilot = Instantiate(pfb_humanPilot);

            }
            // CPU
            else
            {
                pilot = Instantiate(pfb_cpuPilot);
            }

            // �@�̑I��
            BaseMs ms = null;
            switch (pilotInfo.useMs)
            {
                case MsList.Gundam:
                    ms = (Instantiate(pfb_ms[(int)MsList.Gundam]));
                    break;
            }
            pilot.SetMyMs(ms);
            ms.transform.parent = pilot.transform;

            // �`�[���ǉ�
            if (pilotInfo.teamId == Team.Read)
            {
                redTeamPilots.Add(pilot);
            }
            else
            {
                blueTeamPilots.Add(pilot);
            }

            // ���X�g�ɒǉ�
            pilots.Add(pilot);
        }

        // �G�`�[���̐ݒ�
        foreach(BasePilot pilot in redTeamPilots)
        {
            foreach(BasePilot enemyPilot in blueTeamPilots)
            {
                pilot.SetEnemyPilot(enemyPilot);
            }
        }
        foreach (BasePilot pilot in blueTeamPilots)
        {
            foreach (BasePilot enemyPilot in redTeamPilots)
            {
                pilot.SetEnemyPilot(enemyPilot);
            }
        }


        // �p�C���b�g�̏�����
        foreach (BasePilot pilot in pilots)
        {
            pilot.Initialize();
        }
    }
}

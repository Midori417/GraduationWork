using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// �o�g���Ǘ��N���X
/// </summary>
public class BattleManager : MonoBehaviour
{
    [SerializeField, Header("���܂肩��X�^���o�C������܂�")]
    private float startChangeStanbyTime;

    [SerializeField, Header("�X�^���o�C����S�[������܂�")]
    private float stanbyChangeGoTime;

    [SerializeField, Header("�S�[����o�g���X�^�[�g����܂�")]
    private float goChangeBattleStartTim;

    // �ԃ`�[���̃R�X�g
    int teamRedCost;

    // �`�[���̃R�X�g
    int teamBlueCost;

    [SerializeField, Header("�키�p�C���b�g")]
    private List<BasePilot> battlePilots = new List<BasePilot>();

    // �ԃ`�[���p�C���b�g
    private List<BasePilot> redTeamPilots = new List<BasePilot>();

    // �`�[���p�C���b�g
    private List<BasePilot> blueTeamPilots = new List<BasePilot>();

    [SerializeField, Header("�}�b�v")]
    private MapManager mapManager;
    private List<Transform> respones = new List<Transform>();

    [SerializeField, Header("�o�g��UI�R���g���[��")]
    private BattleEventUIControl battleEventUIControl;

    BattleInfo battleInfo;

    [SerializeField]
    GameManager gameManager;

    /// <summary>
    /// �X�^�[�g�C�x���g
    /// </summary>
    private void Start()
    {
        // �o�g�������擾
        //BattleInfo battleInfo = GameManager.instance.battleInfo;
        gameManager = GameManager.instance;
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

        MsStartPosition();

        Invoke("StanbyProsess", stanbyChangeGoTime);
    }

    /// <summary>
    /// �X�V�C�x���g
    /// </summary>
    private void Update()
    {
        BattleProsess();
    }

    /// <summary>
    /// �o�g�����̏���
    /// </summary>
    void BattleProsess()
    {
        if (BattleEneCheck())
        {
            BattleEndProess();
        }
    }

    /// <summary>
    /// �o�g���I�����Ă��邩�`�F�b�N
    /// </summary>
    /// <returns>
    /// true �I��
    /// false �����Ă���
    /// </returns>
    bool BattleEneCheck()
    {
        if (teamBlueCost <= 0 || teamRedCost<= 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// �o�g���I������
    /// </summary>
    void BattleEndProess()
    {
        foreach(BasePilot pilot in battlePilots)
        {
            pilot.StopProsess();
        }
    }

    /// <summary>
    /// �X�^���o�C����
    /// </summary>
    void StanbyProsess()
    {
        battleEventUIControl.Stanby();
        Invoke("GoProsess", stanbyChangeGoTime);
    }

    /// <summary>
    /// �S�[����
    /// </summary>
    void GoProsess()
    {
        battleEventUIControl.Go();
        Invoke("BattleStartProess", goChangeBattleStartTim);
    }

    /// <summary>
    /// �o�g���X�^�[�g����
    /// </summary>
    void BattleStartProess()
    {
        battleEventUIControl.NoImg();
        foreach(BasePilot pilot in battlePilots)
        {
            pilot.StartProsess();
        }
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
                pilot = Instantiate(gameManager.GetPilotPrefab(true));

            }
            // CPU
            else
            {
                pilot = Instantiate(gameManager.GetPilotPrefab(false));
            }

            // �@�̑I��
            BaseMs ms = null;
            switch (pilotInfo.useMs)
            {
                case MsList.Gundam:
                    ms = Instantiate(gameManager.GetMsPrefab((int)MsList.Gundam));
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
            battlePilots.Add(pilot);
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
        foreach (BasePilot pilot in battlePilots)
        {
            pilot.Initialize();
        }
    }

    /// <summary>
    /// �@�̂��ŏ��̈ʒu�ɐݒ�
    /// </summary>
    void MsStartPosition()
    {
        respones = mapManager.responTrs;
        foreach(BasePilot pilot in redTeamPilots)
        {
            pilot.myMs.transform.SetPositionAndRotation(respones[0].position, respones[0].rotation);
        }
        foreach (BasePilot pilot in blueTeamPilots)
        {
            pilot.myMs.transform.SetPositionAndRotation(respones[1].position, respones[1].rotation);
        }
    }

}

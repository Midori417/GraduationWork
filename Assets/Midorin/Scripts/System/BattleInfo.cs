using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �`�[��
/// </summary>
public enum Team
{
    None=0,
    Read=1,
    Blue=2,
}

/// <summary>
/// �v���C���[�̃^�C�v
/// </summary>
public enum PlayerType
{
    Human,
    Cpu,
}

/// <summary>
/// �@�̃��X�g
/// </summary>
public enum MsList
{
    Gundam,
    None,
}

/// <summary>
/// �p�C���b�g���
/// </summary>
[System.Serializable]
public struct PilotInfo
{
    // �`�[��ID
    public Team teamId;

    // �v���C���[�^�C�v
    public PlayerType playerType;

    // �d�l�@��
    public MsList useMs;
}

/// <summary>
/// �o�g�����
/// </summary>
[System.Serializable]
public struct BattleInfo
{
    // �ԃ`�[���̃R�X�g
    public int teamRedCost;

    // �`�[���̃R�X�g
    public int teamBlueCost;

    // ��������
    public int time;

    // �p�C���b�g���z��
    public List<PilotInfo> pilotsInfo;
}
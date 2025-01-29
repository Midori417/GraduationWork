using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Q�[���S�̊Ǘ��N���X
/// </summary>
public class GameManager : MonoBehaviour
{
    // �V���O���g���N���X�ɂ���
    public static GameManager instance
    {
        get;
        private set;
    }

    // �o�g�����
    public BattleInfo battleInfo
    {
        get;
        private set;
    }

    // �`�[���̃R�X�g�̍ő�l
    public static readonly int teamCostMax = 6000;

    [SerializeField, Header("�@�̂̃v���n�u")]
    private List<BaseMs> _pfb_ms;

    [SerializeField, Header("�l�ԃp�C���b�g�v���n�u")]
    private HumanPilot pfb_humanPilot;

    [SerializeField, Header("�R���s���[�^�p�C���b�g�v���n�u")]
    private CpuPilot pfb_cpuPilot;

    /// <summary>
    /// �������̃C�x���g
    /// </summary>
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    /// <summary>
    /// �o�g������ݒ�
    /// </summary>
    /// <param name="_battleInfo"></param>
    public void SetBattleInfo(BattleInfo _battleInfo)
    {
        battleInfo = _battleInfo;
    }

    /// <summary>
    /// �@�̐݌v�}���擾
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public BaseMs GetMsPrefab(int index)
    {
        if (_pfb_ms.Count - 1 < index)
        {
            return null; 
        }
        return _pfb_ms[index];
    }

    /// <summary>
    /// �p�C���b�g�݌v�}���擾
    /// </summary>
    /// <param name="index"></param>
    /// <returns>
    /// true �l��
    /// false CPU
    /// </returns>
    public BasePilot GetPilotPrefab(bool inputType)
    {
        if(inputType)
        {
            return pfb_humanPilot;
        }
        else
        {
            return pfb_cpuPilot;
        }
    }
}

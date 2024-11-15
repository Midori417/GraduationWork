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
}

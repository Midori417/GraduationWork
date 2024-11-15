using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �o�g���Ǘ��N���X
/// </summary>
public class BttleManager : MonoBehaviour
{
    // �ԃ`�[���̃R�X�g
    int teamRedCost = new int();

    // �`�[���̃R�X�g
    int teamBlueCost = new int();

    [SerializeField, Header("�키�p�C���b�g")]
    private List<BasePilot> pilots;

    /// <summary>
    /// �X�^�[�g�C�x���g
    /// </summary>
    private void Start()
    {
        // �o�g�������擾
        BattleInfo battleInfo = GameManager.instance.battleInfo;
        teamRedCost = battleInfo.teamRedCost;
        teamBlueCost = battleInfo.teamBlueCost;
    }
}

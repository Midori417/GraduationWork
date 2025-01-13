using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �p�C���b�g�̊��N���X
/// </summary>
public class BasePilot : MonoBehaviour
{
    [SerializeField, Header("���g�̃J����")]
    private CameraManager _myCameraManager;

    [SerializeField, Header("���g�̋@��")]
    private BaseMs _myMs;

    // �G�`�[���̃p�C���b�g
    private List<BasePilot> enemyTeamPilots = new List<BasePilot>();

    [SerializeField, Header("�^�[�Q�b�g�p�C���b�g")]
    private BasePilot targetPilot;

    public BaseMs myMs
    { get { return _myMs; } }

    public BaseMs targetMs
    { get { return targetPilot.myMs; } }

    /// <summary>
    /// �@�̂�ݒ�
    /// BattleManager�ŌĂяo��
    /// </summary>
    /// <param name="myMs"></param>
    public void SetMyMs(BaseMs myMs)
    {
        _myMs = myMs;
    }

    /// <summary>
    /// �G�l�~�[�`�[���ɒǉ�
    /// </summary>
    /// <param name="enemyPilot"></param>
    public void SetEnemyPilot(BasePilot enemyPilot)
    {
        enemyTeamPilots.Add(enemyPilot);
    }

    /// <summary>
    /// ����������
    /// </summary>
    public virtual void Initialize()
    {
        _myMs.SetMyCamera(_myCameraManager.mainCamera.transform);

        targetPilot = enemyTeamPilots[0];
        _myMs.SetTargetMs(targetMs);

        _myMs.Initialize();
    }
}

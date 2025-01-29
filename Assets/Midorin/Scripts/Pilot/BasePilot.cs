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

    // ���g�̋@��
    public BaseMs myMs
    { get; private set; }

    [SerializeField, Header("�^�[�Q�b�g�p�C���b�g")]
    private BasePilot targetPilot;
    // �^�[�Q�b�g�@��
    public BaseMs targetMs
    { get { return targetPilot.myMs; } }

    // �����`�[���̃p�C���b�g
    private BasePilot teamPilots;

    // �G�`�[���̃p�C���b�g
    private List<BasePilot> enemyTeamPilots = new List<BasePilot>();

    [SerializeField, Header("���������邩")]
    private bool _isProsess;
    protected bool isProsess
    { get { return _isProsess; } }

    // �����ʒu
    private List<Transform> removePos = new List<Transform>();

    // true�Ȃ�@�̂̃����[�u�\
    private bool isRemoveMs = false;

    //[SerializeField, Header("��������")]
    private float removeTime = 5;

    #region �����R���g���[���֐�

    /// <summary>
    /// �������J�n
    /// </summary>
    virtual public void StartProsess()
    {
        _isProsess = true;
    }

    /// <summary>
    /// �������~
    /// </summary>
    virtual public void StopProsess()
    {
        _isProsess = false;
    }

    #endregion

    /// <summary>
    /// ����������
    /// </summary>
    public virtual void Initialize()
    {
        myMs.SetMyCamera(_myCameraManager.mainCamera.transform);

        targetPilot = enemyTeamPilots[0];
        myMs.SetTargetMs(targetMs);

        myMs.Initialize();

        // �J�����̏����ݒ�
        List<Transform> enemyMses = new List<Transform>();
        foreach (BasePilot pilot in enemyTeamPilots)
        {
            enemyMses.Add(pilot.myMs.center);
        }

        _myCameraManager.Initialize(myMs.center, enemyMses);
    }

    /// <summary>
    /// �j�󂳂ꂽ�@�̂̏���
    /// </summary>
    protected void DestoryMsProsess()
    {
        if (myMs.hp <= 0)
        {
            if (!isRemoveMs)
            {
                isRemoveMs = true;
                Invoke("RemoveMs", removeTime);
            }
        }
    }

    /// <summary>
    /// �@�̂𕜌�
    /// </summary>
    private void RemoveMs()
    {
        // �����ʒu
        if (removePos.Count > 0)
        {
            int index = Random.Range(0, removePos.Count);
            myMs.transform.SetPositionAndRotation(removePos[index].position, removePos[index].rotation);
        }

        myMs.Remove();
        isRemoveMs = false;
        myMs.gameObject.SetActive(true);
    }

    /// <summary>
    /// �@�̂�ݒ�
    /// BattleManager�ŌĂяo��
    /// </summary>
    /// <param name="myMs"></param>
    public void SetMyMs(BaseMs _myMs)
    {
        myMs = _myMs;
    }

    /// <summary>
    /// ���`�[���̃p�C���b�g��ݒ�
    /// </summary>
    /// <param name="_teamPilot"></param>
    public void SetTeamPilot(BasePilot _teamPilot)
    {
        teamPilots = _teamPilot;
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
    /// �����ʒu��ݒ�
    /// </summary>
    /// <param name="transforms"></param>
    public void SetRemovePos(List<Transform> transforms)
    {
        removePos = transforms;
    }
}

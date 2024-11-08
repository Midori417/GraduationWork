using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �p�C���b�g�̊��N���X
/// </summary>
public class BasePilot : MonoBehaviour
{
    [SerializeField, Header("���g�̋@��")]
    private BaseMS myMs;

    [SerializeField, Header("���g�̃J����")]
    private GameObject myCamera;

    [SerializeField, Header("���g��UI")]
    private UIManager myUImanager;

    [SerializeField, Header("����`�[���̃p�C���b�g")]
    private BasePilot[] enemyPilots;

    [SerializeField, Header("�^�[�Q�b�g�p�C���b�g")]
    private BasePilot targetPilot;

    [System.Serializable]
    public struct MsInput
    {
        [Header("�ړ�����")]
        public Vector2 moveAxis;

        [Header("�W�����v����")]
        public bool jumpBtn;

        [Header("�_�b�V������")]
        public bool dashBtn;
    }

}

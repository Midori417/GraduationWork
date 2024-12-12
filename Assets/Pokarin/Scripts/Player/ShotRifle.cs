using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotRifle : MonoBehaviour
{
    /// <summary> �e�p�v���n�u </summary>
    [Header("�e�p�v���n�u")]
    [SerializeField] private RifleBullet bulletPrefab;

    /// <summary> �e�����ʒu </summary>
    [Header("�e�����ʒu")]
    [SerializeField] private Transform bulletSpawner;

    [Header("�g�p����o�[�`�����J����")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    /// <summary> �v���C���[�p�A�j���[�^�[ </summary>
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // �E�N���b�N�Ŏˌ�
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // �f�o�b�O�p
            DebugLog();

            // �ˌ��p�A�j���[�V�������Đ�
            if (animator)
            {
                animator.SetTrigger("BeumRifleShot");
            }
        }
    }

    /// <summary>
    /// �f�o�b�O�p
    /// </summary>
    private void DebugLog()
    {
        if (!bulletPrefab)
        {
            Debug.Log("�e�p�v���n�u���ݒ肳��Ă��܂���");
        }

        if (!bulletSpawner)
        {
            Debug.Log("�e�����ʒu���ݒ肳��Ă��܂���");
        }

        if (!animator)
        {
            Debug.Log("�A�j���[�^�[���ǉ�����Ă��܂���");
        }
    }

    /// <summary>
    /// �ˌ��A�j���[�V�����J�n�̏���
    /// </summary>
    private void StartRifleShotAnim()
    {
        // �e����
        if (bulletPrefab && bulletSpawner)
        {
            RifleBullet bullet = Instantiate(bulletPrefab, bulletSpawner.position, Quaternion.identity);
            bullet.target = virtualCamera.LookAt;
        }
    }
}

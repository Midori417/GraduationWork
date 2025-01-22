using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �@�̂̊�{�R���|�[�l���g
/// </summary>
public class BaseMs : MonoBehaviour
{
    [SerializeField, Header("���g�̃Z���^�[�ʒu")]
    private Transform _center;

    // ���J����
    private Transform _myCamera;

    // �^�[�Q�b�g�@��
    private BaseMs _targetMs;

    // �n�ʔ���R���|�[�l���g
    public GroundCheck groundCheck
    {
        get;
        private set;
    }

    // �̗�
    private int _hp;

    [SerializeField, Header("�ő�̗�")]
    private int _hpMax;

    // �_�E���l
    protected float downValue = 0;

    // true�Ȃ�_�E����
    protected bool isDown = false;

    [System.Serializable]
    private struct BoostParamater
    {
        // �G�l���M�[�̍ő��
        private static float max = 100;

        // ���݂̃G�l���M�[��
        private float current;

        // ���݂̃G�l���M�[�̊���(0�`1)
        public float current01
        { get { return Mathf.Clamp01((max - (max - current)) / max); } }

        // �`���[�W�^�C�}�[
        private float chargeTimer;

        [Header("�`���[�W���b�N����")]
        public float chargeLockTime;

        [Header("�I�[�o�[�q�[�g�����Ƃ��̃`���[�W���b�N����")]
        public float overHeartChargeLockTime;

        // true�Ȃ�g�p��
        private bool isUse;

        /// <summary>
        /// �u�[�X�g�p�����[�^�̏�����
        /// </summary>
        public void Initialize()
        {
            current = max;
        }

        /// <summary>
        /// �`���[�W����
        /// </summary>
        public void Charge()
        {
            // �g�p���̓`���[�W���s��Ȃ�
            if (isUse)
            {
                return;
            }

            // �`���[�W�^�C�}�[��0�ȏ゠��Ό��炷
            if (chargeTimer > 0)
            {
                chargeTimer -= Time.deltaTime;
            }
            else
            {
                // �G�l���M�[���񕜂���
                current = max;
            }

            // �l��␳
            chargeTimer = Mathf.Clamp(chargeTimer, 0, overHeartChargeLockTime);
            current = Mathf.Clamp(current, 0, max);
        }

        /// <summary>
        /// �u�[�X�g�̏���
        /// </summary>
        /// <param name="value">�����</param>
        public void UseBoost(float value)
        {
            // 0�ȉ��Ȃ珈�����s��Ȃ�
            if (current <= 0)
            {
                return;
            }

            // ����
            current -= value * Time.deltaTime;

            // �`���[�W�^�C�������Ă���
            if (current > 0)
            {
                chargeTimer = chargeLockTime;
            }
            // 0�ȉ��Ȃ�I�[�o�[�q�[�g���
            else
            {
                chargeTimer = overHeartChargeLockTime;
            }

            // �l��␳
            current = Mathf.Clamp(current, 0, max);
        }
    }
    [SerializeField, Header("�u�[�X�g�p�����[�^")]
    private BoostParamater boostParamater;

    // ������
    public Vector2 moveAxis;
    public bool isJumpBtn;
    public bool isDashBtn;
    public bool isMainShotBtn;
    public bool isSubShotBtn;

    // true�Ȃ�_���[�W���󂯂�
    protected bool isDamageOk = true;

    #region  ���ɓ`����

    public Rigidbody rb
    { get; private set; }
    public Animator animator
    { get; private set; }
    public SkinnedMeshRenderer meshRenderer
    { get; private set; }

    // ���g�̃J����
    public Transform myCamera
    { get { return _myCamera; } }

    // �^�[�Q�b�g�@��
    public BaseMs targetMs
    { get { return _targetMs; } }

    // �Z���^�[
    public Transform center
    { get { return _center; } }

    // �̗�
    public int hp
    { get { return _hp; } }

    // �ő�̗�
    public int hpMax
    { get { return _hpMax; } }

    // ���݂̃G�l���M�[�̊���(0�`1)
    public float boost01
    { get { return boostParamater.current01; } }

    // UI�ɕ\������e
    public List<BaseMsAmoParts> uiArmed;

    #endregion

    /// <summary>
    /// ������
    /// </summary>
    public virtual void Initialize()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
        if (!animator) animator = GetComponent<Animator>();
        if (!groundCheck) groundCheck = GetComponentInChildren<GroundCheck>();
        if (!meshRenderer)
        {
            meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            // ���b�V�������_���[�}�e���A���𕡐�
            Material[] mats = new Material[meshRenderer.materials.Length];
            for (int i = 0; i < meshRenderer.materials.Length; i++)
            {
                mats[i] = new Material(meshRenderer.materials[i]);
                meshRenderer.materials[i] = mats[i];
            }
        }

        boostParamater.Initialize();

        // �̗͂�ݒ�
        _hp = hpMax;
    }

    /// <summary>
    /// ��������
    /// </summary>
    public virtual void Remove()
    {
        Initialize();
        isDamageOk = true;
    }

    /// <summary>
    /// ���b�V���Ԕ���
    /// </summary>
    protected void MeshDamage()
    {
        foreach (Material mat in meshRenderer.materials)
        {
            mat.EnableKeyword("_EMISSION");
        }
    }

    /// <summary>
    /// ���b�V�������ɖ߂�
    /// </summary>
    protected void MeshRemove()
    {
        foreach (Material mat in meshRenderer.materials)
        {
            mat.DisableKeyword("_EMISSION");
        }
    }

    /// <summary>
    ///�����ɕK�v�Ȃ��̂�������Ă��邩�`�F�b�N
    /// </summary>
    /// <returns>
    /// false ������Ă���
    /// true ������Ă��Ȃ�
    /// </returns>
    protected virtual void ProsessCheck()
    {
        if (!rb)
        {
            Debug.LogError("Rigidbody�����݂��܂���");
            return;
        }
        if (!animator)
        {
            Debug.LogError("Animator�����݂��܂���");
            return;
        }
        if (!groundCheck)
        {
            Debug.LogError("GroundCheck�����݂��܂���");
            return;
        }
        if (!center)
        {
            Debug.LogError("Center�����݂��܂���");
            return;
        }
    }

    /// <summary>
    /// �_���[�W��^����
    /// </summary>
    /// <param name="damage"></param>
    public virtual bool Damage(int damage, int _downValue, Vector3 bulletPos)
    {
        // �_���[�W�����s��
        if (!isDamageOk) return false;

        _hp += damage;
        _hp = Mathf.Clamp(_hp, 0, hpMax);
        this.downValue += _downValue;
        return true;
    }

    /// <summary>
    /// �j�󂳂�Ă��邩�`�F�b�N
    /// </summary>
    /// <returns>
    /// true �j�󂳂�Ă���
    /// </returns>
    public bool DestroyCheck()
    {
        if (hp <= 0)
        {
            return true;
        }
        return false;
    }

    #region �u�[�X�g�֌W

    /// <summary>
    /// �u�[�X�g�G�l���M�[�̃`���[�W
    /// </summary>
    protected void BoostCharge()
    {
        if (!groundCheck.isGround)
        {
            return;
        }
        boostParamater.Charge();
    }

    /// <summary>
    /// �u�[�X�g�̏���
    /// </summary>
    /// <param name="value">�����</param>
    public void UseBoost(float value)
    {
        boostParamater.UseBoost(value);
    }

    #endregion

    /// <summary>
    /// �^�[�Q�b�g�@�̂�ݒ�
    /// </summary>
    /// <param name="target"></param>
    public void SetTargetMs(BaseMs target)
    {
        _targetMs = target;
    }

    /// <summary>
    /// ���g�̃J������ݒ�
    /// </summary>
    /// <param name="myCameraTrs"></param>
    public void SetMyCamera(Transform myCameraTrs)
    {
        _myCamera = myCameraTrs;
    }
}

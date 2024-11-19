using UnityEngine;

/// <summary>
/// ���R���|�[�l���g
/// </summary>
public class P_BaseMs : MonoBehaviour
{

    [Header("�J�����g�����X�t�H�[���R���|�[�l���g")]
    public Transform cameraTrs;

    [Header("�p�C���b�g�̓���")]
    protected PilotInput pilotInput;

    [SerializeField, Header("���W�b�h�{�f�B�R���|�[�l���g")]
    protected Rigidbody rb;

    [SerializeField, Header("�A�j���[�^�R���|�[�l���g")]
    protected Animator anim;

    [SerializeField, Header("�n�ʐݒu����R���|�[�l���g")]
    private GroundCheck groundCheck;

    [SerializeField, Header("�o�[�j�A�G�t�F�N�g")]
    private GameObject FireRoketEffect;

    [SerializeField, Header("�p�[�e�B�N���V�X�e��")]
    private new ParticleSystem particleSystem;

    // �n�ʂɂ��Ă��邩�擾
    public bool isGround
    {
        get
        {
            if (groundCheck)
            {
                return groundCheck.isGround;
            }
            else
            {
                return false;
            }
        }
    }

    public bool olsIsGround
    {
        get
        {
            if (groundCheck)
            {
                return groundCheck.oldIsGround;
            }
            else
            {
                return false;
            }
        }

    }

    protected bool isStop;

    protected int maxHp;

    [SerializeField, Header("���݂̗̑�")]
    protected int hp;
    public int HP
    {
        get
        {
            return hp;
        }
    }

    [SerializeField, Header("��͒l")]
    protected int strengthValue;
    public int StrengthValue
    {
        get
        {
            return strengthValue;
        }
    }

    /// <summary>
    /// �u�[�X�g�p�����[�^
    /// </summary>
    [System.Serializable]
    protected struct BoostParamter
    {
        [Header("�ő�u�[�X�g��")]
        public float max;

        [Header("���݂̃u�[�X�g��")]
        public float current;

        [Header("�`���[�W���x")]
        public float chargeSpeed;

        // �`���[�W�^�C�}�[
        public float timer;

        [Header("�`���[�W�܂ł̎���")]
        public float chargeTime;

        [Header("true�Ȃ�`���[�W�����b�N����")]
        public bool chargeLock;
    }
    [SerializeField, Header("�u�[�X�g�p�����[�^")]
    protected BoostParamter boostParamater;
    public float BoostFill
    {
        get
        {
            return Mathf.Clamp01((boostParamater.max - (boostParamater.max - boostParamater.current)) / boostParamater.max);
        }
    }

    /// <summary>
    /// �u�[�X�g�p�����[�^�̏�����
    /// </summary>
    protected void BoostGaugeInit()
    {
        boostParamater.max = 100;
        boostParamater.current = boostParamater.max;
        boostParamater.chargeTime = 0.2f;
        boostParamater.chargeSpeed = 100;
    }

    public void Stop()
    {
        isStop = false;
    }

    /// <summary>
    /// �K�v�ȃR���|�[�l���g���擾����
    /// </summary>
    /// <returns></returns>
    public bool UseGetComponent()
    {
        if (!cameraTrs)
        {
            Debug.Log("�J�����̃g�����X�t�H�[�����擾�ł��܂���");
            return false;
        }
        if (!rb)
        {
            rb = GetComponent<Rigidbody>();
            if (!rb)
            {
                Debug.Log(gameObject.name + "Rigidbody���擾�ł��܂���");
                return false;
            }
        }
        if (!anim)
        {
            anim = GetComponent<Animator>();
            if (!anim)
            {
                Debug.Log(gameObject.name + "Animator���擾�ł��܂���");
                return false;
            }
        }
        if (!groundCheck)
        {
            groundCheck = GetComponentInChildren<GroundCheck>();
            if (!groundCheck)
            {
                Debug.Log(gameObject.name + "GroundCheck���擾�ł��܂���");
                return false;
            }
        }
        if (!pilotInput)
        {
            pilotInput = GetComponentInParent<PilotInput>();
            if (!pilotInput)
            {
                Debug.Log(gameObject.name + "PilotInput���擾�ł��܂���");
                return false;
            }
        }
        if (!FireRoketEffect)
        {
            particleSystem = FireRoketEffect.GetComponent<ParticleSystem>();
            if (!FireRoketEffect)
            {
                Debug.Log(gameObject.name + "FireRoketEffect���擾�ł��܂���");
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// �u�[�X�g�Q�[�W�̃`���[�W����
    /// </summary>
    protected void BoostGaugeChage()
    {
        if (boostParamater.timer > 0 && isGround)
        {
            boostParamater.timer -= Time.deltaTime;
            if(boostParamater.timer <= 0)
            {
                boostParamater.chargeLock = false;
            }
        }

        if(!boostParamater.chargeLock)
        {
            if (boostParamater.current < boostParamater.max)
            {
                boostParamater.current += boostParamater.chargeSpeed;
            }
        }
    }

    /// <summary>
    /// �u�[�X�g�Q�[�W���g�p����
    /// </summary>
    /// <param name="useBoost">�g�p��</param>
    protected void UseBoostGauge(float useBoost)
    {
        if (boostParamater.current > 0)
        {
            boostParamater.current -= useBoost * Time.deltaTime;
            boostParamater.timer = boostParamater.chargeTime;
            boostParamater.chargeLock = true;
        }
    }

    // �G�t�F�N�g���~
    protected void StopEffect()
    {
        //if (!particleSystem && particleSystem.isPlaying)
        //{
        //    particleSystem.Stop();
        //}
    }

    // �G�t�F�N�g���J�n
    public void PlayEffect()
    {
        //if (!particleSystem && !particleSystem.isPlaying)
        //{
        //    particleSystem.Play();
        //}
    }

    /// <summary>
    /// �J�����̐��ʂ���Ɉړ��������v�Z����
    /// </summary>
    /// <returns></returns>
    protected Vector3 MoveForward(Vector2 moveAxis)
    {
        if (!cameraTrs)
        {
            return Vector3.zero;
        }
        Vector3 cameraForward = Vector3.Scale(cameraTrs.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveForward = cameraForward * moveAxis.y + cameraTrs.right * moveAxis.x;

        return moveForward;
    }
}
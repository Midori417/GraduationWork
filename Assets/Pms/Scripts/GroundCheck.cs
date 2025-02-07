using UnityEngine;

/// <summary>
/// �n�ʃ`�F�b�N�R���|�[�l���g
/// </summary>
public class GroundCheck : MonoBehaviour
{
    [SerializeField, Header("�n�ʂ𔻒肷��^�C�v")]
    private string[] groundTypes;

    // true�Ȃ�n�ʂɂ��Ă���
    public bool isGround
    {
        get;
        private set;
    }

    /// <summary>
    /// �O��̃t���[�����n���Ă��邩
    /// </summary>
    public bool oldIsGround
    {
        get;
        private set;
    }

    private void LateUpdate()
    {
        oldIsGround = isGround;
    }

    /// <summary>
    /// �G�ꂽ�Ƃ��ɌĂяo��
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        // �n�ʂɂ��Ă����ԂȂ̂ŏ������s��Ȃ�
        if (isGround) return;

        foreach (string type in groundTypes)
        {
            if (other.tag == type)
            {
                isGround = true;
            }
        }
    }

    /// <summary>
    /// ���ꂽ�Ƃ��ɌĂяo��
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        // �n�ʂɂ��Ă��Ȃ���ԂȂ̂ŏ������s��Ȃ�
        if (!isGround) return;

        foreach (string type in groundTypes)
        {
            if (other.tag == type)
            {
                isGround = false;
            }
        }
    }
}

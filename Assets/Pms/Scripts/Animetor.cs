using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animetor : MonoBehaviour
{
    [SerializeField, Header("�A�j���[�^�R���|�[�l���g")]
    protected Animator anim;

    void Start()
    {
        
    }

    /// <summary>
    /// �K�v�ȃR���|�[�l���g���擾����
    /// </summary>
    /// <returns></returns>
    public bool UseGetComponent()
    {
        if (!anim)
        {
            anim = GetComponent<Animator>();
            if (!anim)
            {
                Debug.Log(gameObject.name + "Animator���擾�ł��܂���");
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// �ړ�����
    /// </summary>
    private void Move(Vector2 moveAxis)
    {
        //if (!isGround)
        //{
            anim.SetBool("Move", false);
        //   return;
        //}

        //Vector3 moveFoward = MoveForward(moveAxis);

        // �i�s�����ɕ�Ԃ��Ȃ����]
        //if (moveFoward != Vector3.zero)
        //{
        //    Quaternion rotation = Quaternion.LookRotation(moveFoward);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, moveParamater.rotationSpeed);
        //    rb.velocity = transform.forward * moveParamater.speed;
            anim.SetBool("Move", true);
        //}
        //else
        //{
        //    rb.velocity = Vector3.zero;
            anim.SetBool("Move", false);
        //}
    }

    /// <summary>
    /// �W�����v����
    /// </summary>
    private void Jump(Vector2 moveAxis, bool isJump)
    {
        //anim.SetBool("Jump", jumpParamater.isNow);
    }

    /// <summary>
    /// �_�b�V������
    /// </summary>
    private void Dash(Vector2 moveAxis, bool isDash)
    {
        //anim.SetBool("Dash", dashParamater.isNow);
    }

    private void BeumRifle()
    {
        //if (pilotInput.GetAttackBtn(PilotInput.BtnState.Down))
        //{
        //    beumRifleParamater.lookTaget = true;
            anim.SetTrigger("Shot");
        //}
    }

    void Update()
    {
        //if (!olsIsGround && isGround)
        //{
        anim.SetTrigger("Ground");
        //    isStop = true;
        //    rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, jumpParamater.inertia);
        //}
        //if (isStop)
        //{
        //    return;
        //}
    }
}

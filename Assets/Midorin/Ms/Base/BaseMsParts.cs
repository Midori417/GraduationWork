using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �@�̃p�[�c�̃x�[�X�R���|�[�l���g
/// </summary>
public class BaseMsParts : MonoBehaviour
{
    // ���C���@�̃R���|�[�l���g
    private Gundam _mainMs;

    private Rigidbody _rb;
    private Animator _animator;

    // ���C���@�̃R���|�[�l���g
    protected Gundam mainMs
    { get { return _mainMs; } }

    protected Rigidbody rb
    { get { return _rb; } }
    protected Animator Animator 
    { get { return _animator; } }

    protected Transform targetMs
    { 
        get
        {
            if(!_mainMs)
            {
                Debug.LogError("���C���@�̂��s��");
                return null;
            }
            if(!_mainMs.targetMs)
            {
                Debug.LogError("�^�[�Q�b�g�@�̂��s��");
                return null;
            }
            return _mainMs.targetMs.transform;
        }
    }

    /// <summary>
    /// ������
    /// </summary>
    /// <returns>
    /// true    ����������
    /// faklse  ���������s
    /// </returns>
    public virtual bool Initalize() 
    {
        _mainMs = GetComponent<Gundam>();
        if (!mainMs)
        {
            _mainMs = GetComponentInParent<Gundam>();

            if (!mainMs)
            {
                Debug.LogError("���C���R���|�[�l���g���擾�ł��܂���");
                return false;
            }
        }
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        return true;
    }
}

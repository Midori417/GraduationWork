using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �^�C�g���Ǘ��N���X
/// </summary>
public class TitleManager : MonoBehaviour
{
    [SerializeField, Header("�t�F�[�h�I�u�W�F�N�g")]
    private FadeOut fadeOut;

    [SerializeField, Header("�؂�ւ��V�[���̖��O")]
    private string sceneName;

    /// <summary>
    /// please�{�^���������ꂽ�Ƃ��̏���
    /// </summary>
    public void PushPleaseButton()
    {
        if(!fadeOut)
        {
            Debug.LogError("�t�F�[�h�I�u�W�F�N�g�����݂��܂���");
            return;
        }
        fadeOut.FadeStrt(sceneName);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �I����ʊǗ��N���X
/// </summary>
public class SelectManager : MonoBehaviour
{
    [SerializeField, Header("�t�F�[�h�I�u�W�F�N�g")]
    private FadeOut fadeOut;

    [SerializeField, Header("Battle�{�^���������Ƃ��̃V�[���؂�ւ��̖��O")]
    private string battleBtnSceneName;

    /// <summary>
    /// Battle�{�^����I�������Ƃ��̏���
    /// </summary>
    public void PushBattle()
    {
        if (!fadeOut)
        {
            Debug.LogError("�t�F�[�h�I�u�W�F�N�g�����݂��܂���");
            return;
        }
        fadeOut.FadeStrt(battleBtnSceneName);
    }

    /// <summary>
    /// Exit�{�^�����I�����ꂽ�Ƃ��̏���
    /// </summary>
    public void PushExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
    Application.Quit();//�Q�[���v���C�I��
#endif
    }
}

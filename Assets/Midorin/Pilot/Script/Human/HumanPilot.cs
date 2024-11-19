using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �l�ԃp�C���b�g
/// </summary>
public class HumanPilot : BasePilot
{
    [SerializeField]
    private PilotInput input;

    private void Start()
    {
        if (myMs)
        {
            myMs.SetPilotInput(input);
        }
        else
        {
            BaseMs ms = GetComponentInChildren<BaseMs>();
            SetMyMs(ms);
            myMs.SetPilotInput(input);
            myMs.SetCameraTrs(myCamera.transform);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 人間パイロット
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

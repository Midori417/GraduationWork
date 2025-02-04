using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CPUパイロット
/// </summary>
public class CpuPilot : BasePilot
{

    private void Update()
    {
        MsUpdate();
        msInput._mainShot = true;

        myMs.msInput = msInput;
    }
}

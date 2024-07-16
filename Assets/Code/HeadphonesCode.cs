using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadphonesCode : MonoBehaviour
{
    public float targetDelay;
    public float max;

    // Start is called before the first frame update
    void Start()
    {
        SettingsManager1.matrixFlashTime = targetDelay;
        SettingsManager1.matrixMinDelay = targetDelay;
        SettingsManager1.matrixMaxDelay = max;

        //dont judge me, im in rush
    }
}

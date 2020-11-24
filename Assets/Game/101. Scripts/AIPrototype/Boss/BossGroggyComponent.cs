using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGroggyComponent : MonoBehaviour
{
    public AIMaster aiMaster;

    public float groggy;
    public float setMaxGroggy;
    public float groggyRecoverySpeed;
    [Range(1, 100)]
    public float groggyRecoveryInterval;

    private float decreaseStartTimer = 0f;
    private float groggyRecoverySpeedTimer = 0f;
    private bool startDecrease = false;

    public float maxStandByTime;

    private void Start()
    {
        if (aiMaster == null)
        {
            aiMaster = GetComponent<AIMaster>();
        }
    }

    private void Update()
    {
        if (groggy >= 0f)
        {
            if (decreaseStartTimer < maxStandByTime)
            {
                decreaseStartTimer += Time.deltaTime;
            }
            else
            {
                groggyRecoverySpeedTimer += Time.deltaTime;
                if (groggyRecoverySpeedTimer >= groggyRecoverySpeed)
                {
                    groggy -= groggy * 1 / groggyRecoveryInterval;
                    groggyRecoverySpeedTimer = 0f;
                }
            }
        }
    }

    public void CalculateGroggyStatus()
    {
        decreaseStartTimer = -0.1f;

        if (groggy >= setMaxGroggy)
        {
            groggy = 0;
            aiMaster.anim.SetTrigger("isGroggy");
        }
    }
}

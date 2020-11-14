using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamageTriggerManager : MonoBehaviour
{
    public DamageTrigger[] damageTriggers;

    public void DamageTrigger_StartTrigger()
    {
        for (int i = 0; i < damageTriggers.Length; i++)
        {
            damageTriggers[i].StartTrigger();
        }
    }

    public void DamageTrigger_EndTrigger()
    {
        for (int i = 0; i < damageTriggers.Length; i++)
        {
            damageTriggers[i].EndTrigger();
        }
    }
}

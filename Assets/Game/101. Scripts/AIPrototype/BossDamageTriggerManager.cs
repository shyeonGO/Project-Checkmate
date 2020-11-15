using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamageTriggerManager : MonoBehaviour
{
    public BossWeaponDamageTrigger[] damageTriggers;
    public double setAttackDamage;

    public void DamageTrigger_StartTrigger()
    {
        for (int i = 0; i < damageTriggers.Length; i++)
        {
            damageTriggers[i].damage = setAttackDamage;
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

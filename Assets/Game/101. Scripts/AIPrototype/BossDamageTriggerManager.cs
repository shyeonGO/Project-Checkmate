// 마지막 Element는 무조건 Growl
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamageTriggerManager : MonoBehaviour
{
    public BossWeaponDamageTrigger[] damageTriggers;
    public BossGrowlDamageTrigger growlTrigger;
    public double setAttackDamage;

    public void DamageTrigger_StartTrigger()
    {
        for (int i = 0; i < damageTriggers.Length - 1; i++)
        {
            damageTriggers[i].damage = setAttackDamage;
            damageTriggers[i].StartTrigger();
        }
    }

    public void DamageTrigger_EndTrigger()
    {
        for (int i = 0; i < damageTriggers.Length - 1; i++)
        {
            damageTriggers[i].EndTrigger();
        }
    }

    public void GrowlTrigger_StartTrigger()
    {
        growlTrigger.damage = setAttackDamage;
        growlTrigger.StartTrigger();
    }

    public void GrowlTrigger_EndTrigger()
    {
        growlTrigger.EndTrigger();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGrowlDamageTrigger : DamageTrigger
{
    [HideInInspector]
    public double damage;
    public override double GetDamage()
    {
        Debug.Log(name + " Growl Damage : " + damage.ToString());
        return damage;
    }
}

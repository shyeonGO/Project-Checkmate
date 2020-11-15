using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeaponDamageTrigger : DamageTrigger
{
    [HideInInspector]
    public double damage;
    public override double GetDamage()
    {
        Debug.Log(name + " Transfer Damage : " + damage.ToString());
        return damage;
    }
}

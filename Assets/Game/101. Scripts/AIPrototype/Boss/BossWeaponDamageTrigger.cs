using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeaponDamageTrigger : DamageTrigger
{
    [HideInInspector]
    public double damage;
    public override DamageData GetDamageData()
    {
        var damageData = new DamageData()
        {
            Trigger = this,
            Damage = damage
        };
        Debug.Log(name + " Transfer Damage : " + damageData);
        return damageData;
    }
}

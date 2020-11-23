using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGrowlDamageTrigger : DamageTrigger
{
    [HideInInspector]
    public double damage;
    public ParticleSystem growlParticle;
    public override DamageData GetDamageData()
    {
        var damageData = new DamageData()
        {
            Trigger = this,
            Damage = damage
        };
        Debug.Log(name + " Growl Damage : " + damageData);
        return damageData;
    }
}

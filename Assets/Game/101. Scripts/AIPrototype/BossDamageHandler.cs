using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossDamageHandler : DamageHandler
{
    [SerializeField]
    private AIMaster aiMaster;
    public UnityEvent damageEvent;

    private void Awake()
    {
        if (aiMaster == null)
        {
            aiMaster = GetComponent<AIMaster>();
        }
    }

    public override void DamageHandle(double damage)
    {
        Debug.Log($"Boss Damage Handle : {damage}");
        aiMaster.healthPoint -= (float)damage;
        damageEvent.Invoke();
    }
}

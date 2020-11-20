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

    public override void DamageHandle(DamageData damageData)
    {
        Debug.Log($"Boss Damage Handle : {damageData}");
        aiMaster.healthPoint -= (float)damageData.Damage;
        aiMaster.groggy += (float)damageData.GroggyPoint;
        damageEvent.Invoke();
    }
}

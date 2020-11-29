using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterDamageHandler : DamageHandler
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
        damageEvent.Invoke();
    }

    /// <summary>
    /// 옵션. 있어도 그만 없어도 그만
    /// 쓰고싶다면 이벤트에 넣어서 쓸 수 있음
    /// </summary>
    public void ImpactMotion()
    {
        aiMaster.anim.SetTrigger("impact");
    }
}

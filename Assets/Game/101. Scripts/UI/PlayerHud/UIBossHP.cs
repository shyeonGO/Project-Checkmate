using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBossHP : UIHPBaseClass
{
    public AIMaster aiMaster;
    private BossDamageHandler damageEvent;

    protected override void Start()
    {
        maxHp = aiMaster.healthPoint;
        currentHp = maxHp;

        damageEvent = aiMaster.GetComponent<BossDamageHandler>();
        damageEvent.damageEvent.AddListener(UpdateHpBar);
    }

    protected override void SetCurrentHpBarSetting()
    {
        currentHp = aiMaster.healthPoint;
    }
}

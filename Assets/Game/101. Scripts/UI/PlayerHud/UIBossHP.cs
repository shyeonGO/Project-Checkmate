using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBossHP : UIHPBaseClass
{
    public AIMaster aiMaster;

    protected override void Start()
    {
        maxHp = aiMaster.healthPoint;
        currentHp = maxHp;
    }

    protected override void SetCurrentHpBarSetting()
    {
        currentHp = aiMaster.healthPoint;
    }
}

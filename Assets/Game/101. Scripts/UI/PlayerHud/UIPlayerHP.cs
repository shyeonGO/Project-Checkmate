using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHP : UIHPBaseClass
{
    public PlayerCharacterStatus player;

    protected override void Start()
    {
        maxHp = player.Hp;
        currentHp = maxHp;
    }

    protected override void SetCurrentHpBarSetting()
    {
        currentHp = player.Hp;
    }
}

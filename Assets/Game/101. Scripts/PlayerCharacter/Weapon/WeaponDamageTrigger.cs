using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamageTrigger : DamageTrigger
{
    [SerializeField] WeaponData weaponData;
    [SerializeField] AudioSource hitAudio;

    PlayerCharacterBehaviour playerCharacterBehaviour;
    public WeaponData WeaponData
    {
        get => weaponData;
        set => weaponData = value;
    }

    public PlayerCharacterBehaviour PlayerCharacterBehaviour
    {
        get => playerCharacterBehaviour;
        set => playerCharacterBehaviour = value;
    }

    protected override void DealingDamage(DamageHandler damageHandler)
    {
        if (hitAudio != null)
            hitAudio.Play();
        Debug.Log($"무기 '{weaponData.WeaponName}' 공격");
        base.DealingDamage(damageHandler);
    }

    public override DamageData GetDamageData()
    {
        //double damage = PlayerCharacterBehaviour.CurrentIsLastAttack || PlayerCharacterBehaviour.CurrentIsLastAttack ?
        //                (PlayerCharacterBehaviour.CurrentIsSwitchingAttack ? WeaponData.LinkedLastAttackDamage : WeaponData.LastAttackDamage) :
        //                (PlayerCharacterBehaviour.CurrentIsSwitchingAttack ? WeaponData.LinkedAttackDamage : WeaponData.AttackDamage);
        double damage = 0;
        if (PlayerCharacterBehaviour.IsLastAttackRecently || PlayerCharacterBehaviour.IsLastAttackInNext)
        {
            if (PlayerCharacterBehaviour.IsSwitchingAttackInCurrent)
            {
                Debug.Log("막타 스위칭 공격");
                damage = WeaponData.LinkedLastAttackDamage;
            }
            else
            {
                Debug.Log("막타 공격");
                damage = WeaponData.LastAttackDamage;
            }
        }
        else
        {
            if (PlayerCharacterBehaviour.IsSwitchingAttackInCurrent)
            {
                Debug.Log("스위칭 공격");
                damage = WeaponData.LinkedAttackDamage;
            }
            else
            {
                Debug.Log("일반 공격");
                damage = WeaponData.AttackDamage;
            }
        }
        //PlayerCharacterBehaviour.CurrentAttackIsLast || PlayerCharacterBehaviour.CurrentAttackIsLast ? WeaponData.LastAttackDamage: WeaponData.AttackDamage
        DamageData damageData = new DamageData()
        {
            Trigger = this,
            Damage = damage,
            GroggyPoint = WeaponData.GroggyPointIncreaseByHit
        };

        Debug.Log($"무기 '{weaponData.WeaponName}' GetDamage {damageData}");
        return damageData;
    }
}

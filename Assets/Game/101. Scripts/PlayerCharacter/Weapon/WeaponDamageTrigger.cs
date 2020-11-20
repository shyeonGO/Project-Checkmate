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
        // TODO: PlayerCharacterBehavior에서 공격 상태 데이터를 전달받아 그에 맞는 데미지 전달 예정
        DamageData damageData = new DamageData()
        {
            Trigger = this,
            Damage = WeaponData.AttackDamage,
            GroggyPoint = WeaponData.GroggyPointIncreaseByHit
        };

        Debug.Log($"무기 '{weaponData.WeaponName}' GetDamage {damageData}");
        return damageData;
    }
}

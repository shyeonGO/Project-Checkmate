using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;

public class PlayerCharacterStatus : MonoBehaviour
{
    [Header("현재 상태")]
    [SerializeField] double hp = 100;
    [SerializeField] double stamina = 100;
    [SerializeField] double switchPoint = 100;
    [Header("무기")]
    [SerializeField] WeaponData weaponSlot1;
    [SerializeField] WeaponData weaponSlot2;
    [SerializeField] WeaponData weaponSlot3;
    [SerializeField] WeaponData weaponSlot4;
    [Header("컴포넌트 및 데이터")]
    [SerializeField] PlayerHud playerHud;
    [SerializeField] PlayerCharacterData data;

    public double Hp
    {
        get => this.hp;
        set
        {
            value = Mathx.Clamp(value, 0, data.MaxHp);
            this.hp = value;
            playerHud.HP = value;
        }
    }
    public double Stamina
    {
        get => this.stamina;
        set
        {
            value = Mathx.Clamp(value, 0, data.MaxStamina);
            this.stamina = value;
            playerHud.Stamina = value;
        }
    }
    public double SwitchPoint
    {
        get => this.switchPoint;
        set
        {
            value = Mathx.Clamp(value, 0, data.MaxSwitchingPoint);
            this.switchPoint = value;
            playerHud.SwitchPoint = value;
        }
    }
    public PlayerCharacterData Data
    {
        get => this.data;
        set => this.data = value;
    }
    public WeaponData WeaponSlot1
    {
        get => this.weaponSlot1;
        set => this.weaponSlot1 = value;
    }
    public WeaponData WeaponSlot2
    {
        get => this.weaponSlot2;
        set => this.weaponSlot2 = value;
    }
    public WeaponData WeaponSlot3
    {
        get => this.weaponSlot3;
        set => this.weaponSlot3 = value;
    }
    public WeaponData WeaponSlot4
    {
        get => this.weaponSlot4;
        set => this.weaponSlot4 = value;
    }

    private void Start()
    {
        if (!playerHud.IsNull())
        {
            playerHud.HP = this.hp;
            playerHud.Stamina = this.stamina;
            playerHud.SwitchPoint = this.switchPoint;
        }
    }
}
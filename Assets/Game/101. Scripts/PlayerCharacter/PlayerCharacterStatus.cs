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
    [SerializeField] int currentWeaponSlotIndex;
    [SerializeField] int sortedWeaponSlotCount;
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
    public WeaponData GetWeaponSlot(int index)
    {
        if (index == 0)
            return null;

        switch (index)
        {
            case 1:
                return weaponSlot1;
            case 2:
                return weaponSlot2;
            case 3:
                return weaponSlot3;
            case 4:
                return weaponSlot4;
        }

        return null;
    }
    public void SetWeaponSlot(int index, WeaponData value)
    {
        if (index == 0)
            return;

        switch (index)
        {
            case 1:
                weaponSlot1 = value;
                break;
            case 2:
                weaponSlot2 = value;
                break;
            case 3:
                weaponSlot3 = value;
                break;
            case 4:
                weaponSlot4 = value;
                break;
        }
        
        SortWeaponSlot();
    }
    /// <summary>
    /// 무기 슬롯에 빈공간이 없도록 정렬합니다.
    /// </summary>
    public void SortWeaponSlot()
    {
        WeaponData internalWeaponSlot1 = null;
        WeaponData internalWeaponSlot2 = null;
        WeaponData internalWeaponSlot3 = null;
        WeaponData internalWeaponSlot4 = null;
        int internalIndex = 0;
        for (int i = 1; i <= 4; i++)
        {
            var value = GetWeaponSlot(i);

            if (value != null)
            {
                internalIndex += 1;
                switch (internalIndex)
                {
                    case 1:
                        internalWeaponSlot1 = value;
                        break;
                    case 2:
                        internalWeaponSlot2 = value;
                        break;
                    case 3:
                        internalWeaponSlot3 = value;
                        break;
                    case 4:
                        internalWeaponSlot4 = value;
                        break;
                }
            }
        }

        weaponSlot1 = internalWeaponSlot1;
        weaponSlot2 = internalWeaponSlot2;
        weaponSlot3 = internalWeaponSlot3;
        weaponSlot4 = internalWeaponSlot4;

        sortedWeaponSlotCount = internalIndex;
    }
    public int CurrentWeaponSlotIndex
    {
        get => this.currentWeaponSlotIndex;
        set => this.currentWeaponSlotIndex = value;
    }

    public int SortedWeaponSlotCount => sortedWeaponSlotCount;
    public WeaponData WeaponSlot1
    {
        get => this.weaponSlot1;
        set
        {
            this.weaponSlot1 = value;
            SortWeaponSlot();
        }
    }
    public WeaponData WeaponSlot2
    {
        get => this.weaponSlot2;
        set
        {
            this.weaponSlot2 = value;
            SortWeaponSlot();
        }
    }
    public WeaponData WeaponSlot3
    {
        get => this.weaponSlot3;
        set
        {
            this.weaponSlot3 = value;
            SortWeaponSlot();
        }
    }
    public WeaponData WeaponSlot4
    {
        get => this.weaponSlot4;
        set
        {
            this.weaponSlot4 = value;
            SortWeaponSlot();
        }
    }

    private void Start()
    {
        if (!playerHud.IsNull())
        {
            playerHud.HP = this.hp;
            playerHud.Stamina = this.stamina;
            playerHud.SwitchPoint = this.switchPoint;
        }
        SortWeaponSlot();
    }
}
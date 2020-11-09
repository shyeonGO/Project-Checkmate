using UnityEngine;
using System.Collections;

public class PlayerCharacterStatus : MonoBehaviour
{
    [SerializeField] PlayerCharacterData data;
    [SerializeField] double maxHp = 100;
    [SerializeField] double hp = 100;

    [SerializeField] double maxStamina = 100;
    [SerializeField] double stamina = 100;

    public double MaxHp
    {
        get => this.maxHp;
        set => this.maxHp = value;
    }
    public double Hp
    {
        get => this.hp;
        set => this.hp = value;
    }
    public double MaxStamina
    {
        get => this.maxStamina;
        set => this.maxStamina = value;
    }
    public double Stamina
    {
        get => this.stamina;
        set => this.stamina = value;
    }
    internal PlayerCharacterData Data
    {
        get => this.data;
        set => this.data = value;
    }
}
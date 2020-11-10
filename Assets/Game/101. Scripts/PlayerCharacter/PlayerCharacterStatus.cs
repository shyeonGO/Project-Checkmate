using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class PlayerCharacterStatus : MonoBehaviour
{
    [SerializeField] DummyPlayerHud dummyPlayerHud;
    [SerializeField] PlayerCharacterData data;
    [SerializeField] double maxHp = 100;
    [SerializeField] double hp = 100;

    [SerializeField] double maxStamina = 100;
    [SerializeField] double stamina = 100;

    public double MaxHp
    {
        get => this.maxHp;
        set
        {
            this.maxHp = value;
        }
    }
    public double Hp
    {
        get => this.hp;
        set
        {
            this.hp = value;
            dummyPlayerHud.HP = value;
        }
    }
    public double MaxStamina
    {
        get => this.maxStamina;
        set
        {
            this.maxStamina = value;
        }
    }
    public double Stamina
    {
        get => this.stamina;
        set
        {
            this.stamina = value;
            dummyPlayerHud.Stamina = value;
        }
    }
    public PlayerCharacterData Data
    {
        get => this.data;
        set => this.data = value;
    }

    private void Start()
    {
        dummyPlayerHud.HP = this.hp;
        dummyPlayerHud.Stamina = this.stamina;
    }
}
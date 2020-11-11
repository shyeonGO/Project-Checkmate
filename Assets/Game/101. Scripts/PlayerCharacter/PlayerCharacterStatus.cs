using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class PlayerCharacterStatus : MonoBehaviour
{
    [SerializeField] PlayerHud playerHud;
    [SerializeField] PlayerCharacterData data;
    [SerializeField] double hp = 100;
    [SerializeField] double stamina = 100;
    [SerializeField] double switchPoint = 100;

    public double Hp
    {
        get => this.hp;
        set
        {
            this.hp = value;
            playerHud.HP = value;
        }
    }
    public double Stamina
    {
        get => this.stamina;
        set
        {
            this.stamina = value;
            playerHud.Stamina = value;
        }
    }
    public PlayerCharacterData Data
    {
        get => this.data;
        set => this.data = value;
    }

    private void Start()
    {
        playerHud.HP = this.hp;
        playerHud.Stamina = this.stamina;
    }
}
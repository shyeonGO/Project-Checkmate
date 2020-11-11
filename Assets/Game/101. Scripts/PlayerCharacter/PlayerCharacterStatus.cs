using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class PlayerCharacterStatus : MonoBehaviour
{
    [Header("현재 상태")]
    [SerializeField] double hp = 100;
    [SerializeField] double stamina = 100;
    [SerializeField] double switchPoint = 100;
    [Header("컴포넌트 및 데이터")]
    [SerializeField] PlayerHud playerHud;
    [SerializeField] PlayerCharacterData data;

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
    public double SwitchPoint
    {
        get => this.switchPoint;
        set => this.switchPoint = value;
    }
    public PlayerCharacterData Data
    {
        get => this.data;
        set => this.data = value;
    }

    private void Start()
    {
        if (!playerHud.IsNull())
        {
            playerHud.HP = this.hp;
            playerHud.Stamina = this.stamina;
        }
    }
}
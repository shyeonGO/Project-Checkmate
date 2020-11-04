using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class PlayerCharacterData : MonoBehaviour
{
    #region 인스펙터 변수
    [SerializeField] int hp = 100;

    [SerializeField] int stamina = 100;

    [Range(0, 10)]
    [SerializeField] float speed = 1;

    [SerializeField] float noDamageTimeByHit;

    [SerializeField] int maxSwitchingPoints = 100;

    [SerializeField] int switchingPointsMinRequirements;

    [SerializeField] int switchingCoolTime;

    [SerializeField] int staminaRecoveryDelay;

    [SerializeField] int staminaRecoveryPerSecond;

    [SerializeField] int sprintStaminaConsumption;

    [SerializeField] int evationStamianConsumption;

    [SerializeField] int parryingSwitchingPointProduction;
    #endregion


    #region 인스펙터 프로퍼티
    public int Hp
    {
        get => this.hp;
        set => this.hp = value;
    }
    public int Stamina
    {
        get => this.stamina;
        set => this.stamina = value;
    }
    public float Speed
    {
        get => this.speed;
        set => this.speed = value;
    }
    public float NoDamageTimeByHit => this.noDamageTimeByHit;
    public int MaxSwitchingPoints => this.maxSwitchingPoints;
    public int SwitchingPointsMinRequirements => this.switchingPointsMinRequirements;
    public int SwitchingCoolTime => this.switchingCoolTime;
    public int StaminaRecoveryDelay => this.staminaRecoveryDelay;
    public int StaminaRecoveryPerSecond => this.staminaRecoveryPerSecond;
    public int SprintStaminaConsumption => this.sprintStaminaConsumption;
    public int EvationStamianConsumption => this.evationStamianConsumption;
    public int ParryingSwitchingPointProduction => this.parryingSwitchingPointProduction;
    #endregion
}
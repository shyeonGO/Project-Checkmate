using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Character Data", menuName ="Game Data/Player Character Data")]
public class PlayerCharacterData : ScriptableObject
{
    [Tooltip("최대 체력")]
    public double MaxHp = 100;

    [Tooltip("최대 행동력")]
    public double MaxStamina = 100;

    [Tooltip("최대 스위치 포인트")]
    public double MaxSwitchingPoint = 100;

    [Tooltip("최대 상태 포인트")]
    public double MaxStatePoint = 100;

    [Tooltip("피격시 무적 시간")]
    public float NoDamageTimeByBeaten = 1;

    [Tooltip("스위칭 포인트 최소 요구량")]
    public double SwitchingPointsMinRequirements = 30;

    [Tooltip("스위칭 쿨타임")]
    public float SwitchingCoolTime = 5;

    [Tooltip("행동력 회복 딜레이")]
    public float StaminaRecoveryDelay = 1;

    [Tooltip("초당 행동력 회복량")]
    public double StaminaRecoveryPerSecond = 20;

    [Tooltip("전력 질주 행동력 소모량")]
    public double StaminaConsumptionBySprint = 10;

    [Tooltip("회피 행동력 소모량")]
    public double StamianConsumptionByEvation = 25;

    [Tooltip("패링 스위칭 포인트 상승량")]
    public double SwitchingPointProductionByParrying = 30;

    [Tooltip("패링 그로기 포인트 상승량")]
    public double GroggyPointProductionByParrying = 20;
}
using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "Game Data/Weapon Data")]
public class WeaponData : ScriptableObject
{
    /**용어 정리**********************
     * ByAttack : 공격시
     * ByHit : 적중시
     *********************************/

    private void Awake()
    {
        if (ID == 0)
        {
            ID = UnityEngine.Random.Range(1, int.MaxValue);
        }
    }
    [Tooltip("건들지 마세요!")]
    public int ID;

    [Space]
    public GameObject WeaponPrefab;

    public string WeaponName;

    public WeaponType Type;

    [Tooltip("공격 데미지")]
    public double AttackDamage = 30;

    [Tooltip("강공격 데미지")]
    public double PowerAttackDamage = 40;

    [Tooltip("스위칭 연계 공격 데미지")]
    public double LinkedAttackDamage = 30;

    [Tooltip("스위칭 연계 강공격 데미지")]
    public double LinkedPowerAttackDamage = 40;

    /// <summary>
    /// 공격 애니메이션 속도
    /// </summary>
    [Tooltip("공격 속도")]
    public float AttackSpeed = 1;

    [Tooltip("회피시 무적 시간")]
    public double NoDamageTimeByEvasion;

    [Tooltip("회피 거리")]
    public double EvasionDistance;

    [Tooltip("적중시 스위칭 포인트 증가량")]
    public double SwitchingPointIncreaseByHit;

    [Tooltip("적중시 그로기 포인트 증가량")]
    public double GroggyPointIncreaseByHit;

    [Tooltip("공격시 스태미너 감소량")]
    public double StaminaDecreaseByAttack;
}
using System;
using System.Linq;
using UnityEngine;

class PlayerCharacterEquipment : MonoBehaviour
{
    [Header("오브젝트 설정")]
    //[SerializeField] DamageTriggerManager damageTriggerManager;
    [SerializeField] Transform weaponJoint;
    [SerializeField] Transform currentWeaponTransform;

    [Header("데이터")]
    [SerializeField] WeaponData weaponData;

    PlayerCharacterBehaviour playerCharacterBehaviour;

    DamageTrigger[] damageTriggers = Array.Empty<DamageTrigger>();

    private void Awake()
    {
        playerCharacterBehaviour = GetComponent<PlayerCharacterBehaviour>();

        WeaponData = WeaponData;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Correctness", "UNT0008:Null propagation on Unity objects", Justification = "<보류 중>")]
    public WeaponData WeaponData
    {
        get => weaponData;
        set
        {
            var weaponData = this.weaponData.ToReferenceNull();
            value = value.ToReferenceNull();

            weaponData = value;

            // 기존 무기 제거
            if (currentWeaponTransform != null)
            {
                //currentWeaponTransform.gameObject.SetActive(false);
                var currentWeaponObject = currentWeaponTransform.gameObject;

                damageTriggers = null;
                Destroy(currentWeaponObject);
            }

            // 무기 오브젝트 생성
            if (weaponData != null)
            {
                var currentWeaponObject = Instantiate(weaponData.WeaponPrefab);

                // 부모 설정
                currentWeaponTransform = currentWeaponObject.transform;
                currentWeaponTransform.parent = weaponJoint;
                currentWeaponTransform.Initialize();

                damageTriggers = currentWeaponObject.GetComponents<DamageTrigger>();

                foreach (var damageTrigger in damageTriggers)
                {
                    if (damageTrigger is WeaponDamageTrigger weaponDamageTrigger)
                    {
                        weaponDamageTrigger.PlayerCharacterBehaviour = playerCharacterBehaviour;
                    }
                }
            }
        } // end of setter
    } // end of WeaponData

    public void StartTrigger()
    {
        foreach (var damageTrigger in damageTriggers)
        {
            damageTrigger.StartTrigger();
        }
    }

    public void EndTrigger()
    {
        foreach (var damageTrigger in damageTriggers)
        {
            damageTrigger.EndTrigger();
        }
    }

    void DamageTrigger_StartTrigger()
    {
        StartTrigger();
    }

    void DamageTrigger_EndTrigger()
    {
        EndTrigger();
    }
}
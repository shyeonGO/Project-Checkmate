using System;
using System.Linq;
using UnityEngine;

class PlayerCharacterEquipment : MonoBehaviour
{
    [Header("오브젝트 설정")]
    [SerializeField] Transform weaponJoint;
    [SerializeField] Transform currentWeaponTransform;
    [SerializeField] PlayerCharacterBehaviour playerCharacterBehaviour;

    [Header("데이터")]
    [SerializeField] WeaponData weaponData;

    [Header("애니메이션")]
    [SerializeField] int WeaponType1LayerIndex = 1;
    [SerializeField] int WeaponType2LayerIndex = 2;

    DamageTrigger[] damageTriggers = Array.Empty<DamageTrigger>();

    private void Awake()
    {
        if (playerCharacterBehaviour == null)
            playerCharacterBehaviour = GetComponent<PlayerCharacterBehaviour>();
    }

    private void Start()
    {
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

                // 애니메이터 상태 설정
                SetWeaponTypeForAnimator(WeaponData.Type);
                //playerCharacterBehaviour.Animator.SetInteger("weaponType", (int)weaponData.Type);
            }
        } // end of setter
    } // end of WeaponData

    void SetWeaponTypeForAnimator(WeaponType type)
    {
        var animator = playerCharacterBehaviour.Animator;
        switch (type)
        {
            default:
                animator.SetLayerWeight(WeaponType1LayerIndex, 0);
                animator.SetLayerWeight(WeaponType2LayerIndex, 0);
                break;
            case WeaponType.OneHanded:
                animator.SetLayerWeight(WeaponType1LayerIndex, 1);
                animator.SetLayerWeight(WeaponType2LayerIndex, 0);
                break;
            case WeaponType.Rapier:
                animator.SetLayerWeight(WeaponType1LayerIndex, 0);
                animator.SetLayerWeight(WeaponType2LayerIndex, 1);
                break;
        }
    }

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
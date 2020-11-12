using System.Linq;
using UnityEngine;

class PlayerCharacterEquipment : MonoBehaviour
{
    [Header("오브젝트 설정")]
    [SerializeField] DamageTriggerManager damageTriggerManager;
    [SerializeField] Transform weaponJoint;
    [SerializeField] Transform currentWeaponTransform;

    [Header("데이터")]
    [SerializeField] WeaponData weaponData;

    public WeaponData WeaponData
    {
        get => weaponData;
        set
        {
            var weaponData = this.weaponData.ToReferenceNull();
            value = value.ToReferenceNull();
            if (weaponData?.ID != value?.ID)
            {
                weaponData = value;

                // 기존 무기 제거
                if (currentWeaponTransform != null)
                {
                    //currentWeaponTransform.gameObject.SetActive(false);
                    var currentWeaponObject = currentWeaponTransform.gameObject;

                    var damageTriggers = currentWeaponObject.GetComponents<DamageTrigger>();
                    foreach(var damageTrigger in damageTriggers)
                    {
                        damageTriggerManager.DamageTriggers.Remove(damageTrigger);
                    }
                    Destroy(currentWeaponObject);
                }

                // 무기 오브젝트 생성
                if (weaponData != null)
                {
                    var currentWeaponObject = Instantiate(weaponData.WeaponPrefab);

                    currentWeaponTransform = currentWeaponObject.transform;
                    currentWeaponTransform.parent = weaponJoint;

                    var damageTriggers = currentWeaponObject.GetComponents<DamageTrigger>();
                    damageTriggerManager.DamageTriggers.AddRange(damageTriggers);
                }
            }
        } // end of setter
    }
}
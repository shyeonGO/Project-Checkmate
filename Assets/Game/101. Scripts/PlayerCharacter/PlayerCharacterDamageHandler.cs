
using UnityEngine;

class PlayerCharacterDamageHandler : DamageHandler
{
    [SerializeField] PlayerCharacterBehaviour behaviour;

    private void Awake()
    {
        if (behaviour == null)
        {
            behaviour = GetComponent<PlayerCharacterBehaviour>();
        }
    }

    public override void DamageHandle(DamageData damageData)
    {
        Debug.Log($"플레이어 데미지 핸들: {damageData}");
        behaviour.Status.Hp -= damageData.Damage;
    }
}
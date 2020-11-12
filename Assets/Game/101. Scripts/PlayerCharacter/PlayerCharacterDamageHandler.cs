
using UnityEngine;

class PlayerCharacterDamageHandler : DamageHandler
{
    [SerializeField] PlayerCharacterBehaviour behaviour;

    private void Awake()
    {
        if (behaviour != null)
        {
            behaviour = GetComponent<PlayerCharacterBehaviour>();
        }
    }

    public override void DamageHandle(double damage)
    {
        Debug.Log($"플레이어 데미지 핸들: {damage}");
        behaviour.Status.Hp -= damage;
    }
}
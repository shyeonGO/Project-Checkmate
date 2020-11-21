// 풀링하는 구조를 생각해봐야겠음.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectStruct
{
    public ParticleSystem particleObject;
    [HideInInspector]
    public GameObject particleGameObject;
}

public class BossEffectManager : MonoBehaviour
{
    public List<EffectStruct> effects;
    public Transform effectPosition;
    public float effectDuration;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < effects.Count; i++)
        {
            effects[i].particleGameObject = effects[i].particleObject.gameObject;
            effects[i].particleGameObject.SetActive(false);
        }
    }

    public void Effect_Start(int index)
    {
        effects[index].particleGameObject.SetActive(true);
        effects[index].particleObject.Play();
        //effects[index].particleGameObject.transform.position = effectPosition.position;
        //effects[index].particleGameObject.transform.rotation = Quaternion.identity;
        StartCoroutine(DestroyParticle(effects[index].particleGameObject, effects[index].particleObject.duration));
        Debug.Log("Effect Index : " + index);
    }

    IEnumerator DestroyParticle(GameObject gb, float particleDuration)
    {
        yield return new WaitForSeconds(particleDuration);
        gb.SetActive(false);
    }
}

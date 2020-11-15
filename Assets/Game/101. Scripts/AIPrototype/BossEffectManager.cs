using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEffectManager : MonoBehaviour
{
    public GameObject effectPrefab;
    public Transform effectPosition;
    public float effectDuration;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Effect_Start()
    {
        GameObject effect = Instantiate(effectPrefab, effectPosition.position, Quaternion.identity);
        StartCoroutine(DestroyParticle(effect));
    }

    IEnumerator DestroyParticle(GameObject gb)
    {
        yield return new WaitForSeconds(effectDuration);
        Destroy(gb);
    }
}

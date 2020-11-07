using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyDamageTrigger : DamageTrigger
{
    [SerializeField] AudioSource dummyAudioSoucre;
    public override int GetDamage()
    {
        dummyAudioSoucre.Play();
        var random  = Random.Range(0, 10000);
        Debug.Log($"죽어라 얍! {random}");
        return random;
    }
}

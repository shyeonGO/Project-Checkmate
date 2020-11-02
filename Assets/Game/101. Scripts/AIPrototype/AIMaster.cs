using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMaster : MonoBehaviour
{
    [Header("Basic Setting")]
    public string bossName;
    public float healthPoint;

    [Header("Groggy")]
    public float groggy;
    private float saveGroggy;
    public float groggyContinuingTime;
    public float groggyRecoverySpeed;
    public float groggyRecoveryInterval;

    [Header("Infect")]
    public float infect;
    private float saveInfect;
    public float infectContinuingTime;
    public float infectRecoverySpeed;
    public float infectRecoveryInterval;

    private NavMeshAgent agent;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        // 추가됨
        saveGroggy = 0;
        saveInfect = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

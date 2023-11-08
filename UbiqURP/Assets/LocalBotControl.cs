using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LocalBotControl : MonoBehaviour
{
    public Transform testTarget;
    NavMeshAgent agent;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = testTarget.position;
        animator.SetFloat("AgentSpeed", agent.velocity.magnitude);
    }
}

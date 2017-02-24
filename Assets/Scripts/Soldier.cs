using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : MonoBehaviour {
    public Animator anim;
    public int goal;

    public List<Transform> waypoints;

    NavMeshAgent agent;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
    }


    void Update() {
    }

    public void Kill() {
        anim.SetBool("Dead", true);

        StartCoroutine("Ragdoll");
    }

    IEnumerator Ragdoll() {
        yield return new WaitForSeconds(2);

        anim.enabled = false;
        GetComponent<CharacterController>().enabled = false;
    }


    public void StartPatrol() {
        agent.SetDestination(waypoints[goal].position);
        agent.Resume();
    }

    public void UpdatePatrol() {
        Debug.Log(Vector3.Distance(agent.destination, transform.position) + " " + agent.stoppingDistance);
        if (Vector3.Distance(agent.destination, transform.position) <= agent.stoppingDistance) {
            goal++;

            if (goal >= waypoints.Count) {
                goal = 0;
            }
            agent.SetDestination(waypoints[goal].position);
        }
    }


    public void EndPatrol() {
        agent.Stop();
    }
}
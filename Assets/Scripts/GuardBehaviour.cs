using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(AICharacterControl))]
public class GuardBehaviour : MonoBehaviour
{
    public Transform waypoints;
    public Transform player;
    public float Walking_speed = 0.5f;
    public float Running_speed = 1;

    private bool player_spotted;
    private Transform new_destination;
    private Transform[] waypoints_list;
    private AICharacterControl aiControl;
    private NavMeshAgent navMesh;
    private RaycastHit hit;
 


    void Start()
    {
        aiControl = GetComponent<AICharacterControl>();
        navMesh = GetComponent<NavMeshAgent>();
        waypoints_list = waypoints.GetComponentsInChildren<Transform>();
        SelectDest(null);
        navMesh.speed = Walking_speed; // Delete when timer is implemented
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, player.position - transform.position, out hit))
        {
            if (hit.collider.CompareTag("Player") && (Vector3.Angle(hit.transform.position, transform.TransformDirection(Vector3.forward)) < 45))
            {
                Debug.DrawRay(transform.position, player.position - transform.position, Color.red);
                aiControl.target = player;
                navMesh.speed = Running_speed;
                player_spotted = true;
            }
            else
            {
                Debug.DrawRay(transform.position, player.position - transform.position, Color.gray);
                //player_spotted = false;
                // needs a timer
            }
        }
    }

    void SelectDest(Collider other)
    {
        do {
            new_destination = waypoints_list[Random.Range(1, waypoints_list.Length)];
        } while (new_destination.GetComponent<Collider>() == other);

        aiControl.target = new_destination;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Waypoint"))
        {
            SelectDest(other);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoosterBehaviour : MonoBehaviour {

    private NavMeshAgent agent;
    private Vector3 CurrentDestination;

    // Use this for initialization
    void Start () {

        agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update () {

        agent.SetDestination(target.transform.position);

    }

    public void Alarm(GameObject target)
    {
        CurrentDestination = target.transform.position;
    }

    private GameObject FindNearest(Collider[] Neighborgs)
    {
        float distance = 0f;
        float nearestDistance = float.MaxValue;
        GameObject NearestElement = null;

        foreach (Collider NearbyElement in Neighborgs)
        {
            if (NearbyElement != null)
            {
                distance = Vector3.Distance(NearbyElement.transform.position, transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    NearestElement = NearbyElement.gameObject;
                }
            }
        }
        return NearestElement;
    }
}

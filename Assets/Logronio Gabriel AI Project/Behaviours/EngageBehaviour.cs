using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngageBehaviour : GeneralBehaviour
{
    private float timer = 0f;
    private float AttackTime = 2f;
    public float PushForce = 1f;

    override public void ExecuteBehaviour(Collider[] Neighbors)
    {
        timer += Time.deltaTime;
        if(timer > AttackTime)
        {
            FindNearest(Neighbors).GetComponent<Rigidbody>().AddForce(transform.forward * PushForce, ForceMode.Impulse);
            timer = 0;
        }
    }

    private GameObject FindNearest(Collider[] Neighborgs)
    {
        float distance = 0f;
        float nearestDistance = float.MaxValue;
        GameObject NearestElement = null;

        foreach (Collider NearbyElement in Neighborgs)
        {
            distance = Vector3.Distance(NearbyElement.transform.position, transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                NearestElement = NearbyElement.gameObject;
            }
        }
        return NearestElement;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public Vector3 StartingPosition;
    public Vector3 ReachingPosition;
    public float Speed;
    private bool BackAndForth = true;

	// Use this for initialization
	void Start () {

        transform.position = StartingPosition;

	}
	
	// Update is called once per frame
	void Update () {

        if (BackAndForth)
        {
            if (transform.position != ReachingPosition)
                transform.position = Vector3.MoveTowards(transform.position, ReachingPosition, Speed * Time.deltaTime);
            else BackAndForth = false;
        }

        else
        {
            if (transform.position != StartingPosition)
                transform.position = Vector3.MoveTowards(transform.position, StartingPosition, Speed * Time.deltaTime);
            else BackAndForth = true;
        }

	}

    void OnCollisionEnter(Collision col)
    {
        col.gameObject.transform.parent = transform;
    }

    void OnCollisionExit(Collision col)
    {
        Debug.Log("EXIT");
        col.gameObject.transform.parent = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBehaviour : MonoBehaviour {

    public GameObject Door;

    private void OnCollisionEnter(Collision collision)
    {
        /*GameObject coll = collision.gameObject;

        if (coll.tag.Equals("Player"))
        {*/
        Destroy(Door);
        Destroy(gameObject);
        // }Door.SetActive(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlantBehaviour : NetworkBehaviour {

    public int speed = 5;

    // Update is called once per frame
    void Update () {
		if (transform.position.y < 0.0f)
        {
            transform.Translate(transform.up * speed * Time.deltaTime);
        }
	}
}

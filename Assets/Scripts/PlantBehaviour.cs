using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBehaviour : MonoBehaviour {

    public int speed = 5;

    // Update is called once per frame
    void Update () {
		if (transform.position.y < 0.0f)
        {
            transform.Translate(transform.up * speed * Time.deltaTime);
        }
	}
}

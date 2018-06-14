﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnBoss : NetworkBehaviour {

    public GameObject boss;

	// Use this for initialization
	void Start () {
        GameObject bossspawn = Instantiate(boss, new Vector3(0, 0, 0), Quaternion.identity);
        NetworkServer.Spawn(bossspawn);

	}
}

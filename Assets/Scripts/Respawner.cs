using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour {

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == 9)
        {
            BasicController playerController = col.gameObject.GetComponent<BasicController>();
            playerController.Respawn(MoleficentGameManager.instance.LastCheckPoint());

        }
        else
        {
            col.gameObject.SetActive(false);
        }
    }
}

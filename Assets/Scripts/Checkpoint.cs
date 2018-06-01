using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == 9)
        {
            MoleficentGameManager.instance.UpdateCheckpoint(transform.position);
            gameObject.SetActive(false);
        }
    }
}

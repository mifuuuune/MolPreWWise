using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RespawnWithDelay : NetworkBehaviour {

    public float Delay = 0f;

    private void OnDisable()
    {
        Invoke("CmdReEnable", Delay);
    }

    [Command]
    private void CmdReEnable()
    {
        gameObject.SetActive(true);
    }

}

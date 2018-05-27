using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnWithDelay : MonoBehaviour {

    public float Delay = 0f;

    private void OnDisable()
    {
        Invoke("ReEnable", Delay);
    }

    private void ReEnable()
    {
        gameObject.SetActive(true);

    }

}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCheck : MonoBehaviour {

    public GameObject TutorialBlock;
    private bool SirBeanArrived;
    private bool SirEalArrived;
    private bool SirLoinArrived;
    private bool SirSageArrived;

    void OnTriggerEnter(Collision col)
    {
        if (col.gameObject.layer == 9)
        {
            if (!SirBeanArrived && col.gameObject.tag == "Bean") SirBeanArrived = true;
            if (!SirEalArrived && col.gameObject.tag == "Eal") SirEalArrived = true;
            if (!SirLoinArrived && col.gameObject.tag == "Loin") SirLoinArrived = true;
            if (!SirSageArrived && col.gameObject.tag == "Sage") SirSageArrived = true;

            if (SirBeanArrived && SirEalArrived && SirLoinArrived && SirSageArrived) TutorialBlock.SetActive(false);
        }
    }
}

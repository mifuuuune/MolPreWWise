using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HensParametersManager : MonoBehaviour {

    //--------------------------------------------------------PARAMETERS FOR THE FLOCKING BEHAVIOUR--------------------------------------------

    public static float HenFOV = 1.5f;

    public static float HenAttackFOV = 0.75f;

    public static float HenAggressiveFOV = 2f;

    public static float HenProtectiveFOV = 1.75f; // == HenUnlikableFOV

    public static float HenCowardFOV = 2.5f;

    public static float HenSpeed = 1.25f;

    public static float HenAlignWeight = 1f;

    public static float HenCohesionWeight = 1f;

    public static float HenSeparationWeight = 1f;

}

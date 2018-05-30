using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChickDecisionTree : MonoBehaviour {

    public enum ChickPersonalities { COCKY, COWARD, CURIOUS, SISSY, SLY}

    public ChickPersonalities Personality;

    public GameObject Rooster;
    private GameObject NearestHen;
    private GameObject NearestPlayer;
    private NavMeshAgent agent;

    public LayerMask HensLayer;
    private Collider[] NearbyHens = new Collider[7];

    public LayerMask ChicksLayer;
    private Collider[] NearbyChicks = new Collider[7];

    public LayerMask PlayersLayer;
    private Collider[] NearbyPlayers = new Collider[4];

    // Use this for initialization
    void Start()
    {

        float RandomPersonality = Random.value;
        Debug.Log(gameObject.name + ": " + RandomPersonality);
        if (RandomPersonality < 0.1f) Personality = ChickPersonalities.COCKY;
        if (RandomPersonality >= 0.1f && RandomPersonality < 0.45f) Personality = ChickPersonalities.COWARD;
        if (RandomPersonality >= 0.45f && RandomPersonality < 0.6f) Personality = ChickPersonalities.CURIOUS;
        if (RandomPersonality >= 0.6f && RandomPersonality < 0.7f) Personality = ChickPersonalities.SISSY;
        if (RandomPersonality >= 0.7f) Personality = ChickPersonalities.SLY;

        agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update () {

        if (Vector3.Distance(Rooster.transform.position, transform.position) > 4f) CatchUp();
        else
        {
            if ((Personality == ChickPersonalities.COWARD && Physics.OverlapSphereNonAlloc(transform.position, ChicksParametersManager.ChickCowardFOV, NearbyPlayers, PlayersLayer) > 0)
                || Physics.OverlapSphereNonAlloc(transform.position, ChicksParametersManager.ChickFOV, NearbyPlayers, PlayersLayer) > 0)
            {
                float Ignore = Random.value;
                NearestPlayer = FindNearest(NearbyPlayers);

                if (Ignore >= 0.9f || (Ignore >= 0.75f && NearestPlayer.GetComponent<BasicController>().IsTheMole()))
                {

                }
            }
            else
            {
                if (Personality == ChickPersonalities.SISSY && Physics.OverlapSphereNonAlloc(transform.position, ChicksParametersManager.ChickFOV, NearbyHens, HensLayer) > 0)
                {
                    NearestHen = FindNearest(NearbyHens);
                    if (Vector3.Distance(NearestHen.transform.position, transform.position) > ChicksParameters.ChickDangerFOV) GetComponent<FleeBehaviour>().ExecuteBehaviour(NearbyHens);
                    else GetComponent<AlarmBehaviour>().ExecuteBehaviour(NearbyHens);

                }
                else
                {
                    Physics.OverlapSphereNonAlloc(transform.position, ChicksParametersManager.ChickFOV, NearbyChicks, ChicksLayer);
                    GetComponent<RoamingBehaviour>().ExecuteBehaviour(NearbyChicks);
                }
            }
        }
    }

    private void CatchUp()
    {
        agent.speed = 4f;
        agent.SetDestination(Rooster.transform.position);
        //transform.rotation = Quaternion.LookRotation(Rooster.transform.position - transform.position);
        //transform.position = Vector3.MoveTowards(transform.position, Rooster.transform.position, HensParametersManager.HenSpeed * Time.deltaTime);
    }

    private GameObject FindNearest(Collider[] Neighborgs)
    {
        float distance = 0f;
        float nearestDistance = float.MaxValue;
        GameObject NearestElement = null;

        foreach (Collider NearbyElement in Neighborgs)
        {
            distance = Vector3.Distance(NearbyElement.transform.position, transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                NearestElement = NearbyElement.gameObject;
            }
        }
        return NearestElement;
    }
}

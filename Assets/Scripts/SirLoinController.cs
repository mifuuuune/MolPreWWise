using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SirLoinController : BasicController {

    private float FallingSpeed = -0.3f;
    private bool CanGlide = false;
    private bool AlternateThrow = false;
    private float timer = 1.5f;
    private float AnimationStop = 0.75f;
    public GameObject FirstKnife;
    public GameObject SecondKnife;
    private int number_of_knifes=0;
    private GameObject knife1, knife2;

    protected override void SpecialJump()
    {
        base.SpecialJump();
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(GetDirection()), RotationSpeed);
        transform.position += (GetDirection() * MovementSpeed / JumpSlow * Time.deltaTime);
        rb.velocity = new Vector3(0, FallingSpeed, 0);
    }

    protected override void StatusUpdate(float CurrentInput)
    {

        if (Input.GetButtonUp("Jump")) CanGlide = true;
            

        if (!IsGrounded)
        {
            if (CanGlide && Input.GetButton("Jump"))
            {
                anim.SetBool("Gliding", true);
                SpecialJump();
            }
            else
            {
                anim.SetBool("Gliding", false);
                Jump();
            }

        }
        else
        {
            anim.SetBool("Gliding", false);

            if (timer <= AnimationStop)
            {
                timer += Time.deltaTime;
                anim.SetBool("Moving", false);
                return;
            }

            if (CurrentInput > 0) Run();
                else Idle();

            if (Input.GetMouseButtonDown(0))
            {
                CmdUseAbility();
                return;
            }

            if (Input.GetButtonDown("Jump"))
            {
                rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
                CanGlide = false;
                Jump();
                return;
            }
        }
    }

    
    protected override void CmdUseAbility()
    {
        base.CmdUseAbility();

        RaycastHit AbilityHit;
        Ray AbilityRay = new Ray(transform.position + new Vector3(0, 0.68f, 0), AimRayCast() - (transform.position + new Vector3(0, 0.68f, 0)));
        Debug.DrawRay(transform.position + new Vector3(0, 0.68f, 0), AimRayCast() - (transform.position + new Vector3(0, 0.68f, 0)), Color.green);
        if (Physics.Raycast(AbilityRay, out AbilityHit, AbilityRange))
        {
            if (AbilityHit.collider.gameObject.tag == "Wood")
            {
                timer = 0;
                if (AlternateThrow)
                {
                    if (number_of_knifes >= 2)
                    {
                        CmdUnSpawnKnife(1);
                        
                        Debug.Log("----->>>Knifes: " + number_of_knifes);
                    }                        
                    AlternateThrow = false;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(AimRayCast().x - transform.position.x, 0, AimRayCast().z - transform.position.z)), RotationSpeed * 10);
                    anim.SetTrigger("FirstThrow");
                    //StickKnife(FirstKnife, AbilityHit);
                    //FirstKnife.SetActive(true);
                    FirstKnife.transform.position = AimRayCast();
                    FirstKnife.transform.rotation = Quaternion.FromToRotation(Vector3.back, AbilityHit.normal);
                    CmdSpawnKnife(FirstKnife, 1);
                    number_of_knifes++;
                    Debug.Log("Knifes: " + number_of_knifes);
                }
                else
                {
                    if (number_of_knifes >= 2)
                    {
                        CmdUnSpawnKnife(2);
                        Debug.Log("----->>>Knifes: " + number_of_knifes);
                    }
                    AlternateThrow = true;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(AimRayCast().x - transform.position.x, 0, AimRayCast().z - transform.position.z)), RotationSpeed * 10);
                    anim.SetTrigger("SecondThrow");
                    //StickKnife(SecondKnife, AbilityHit);
                    //SecondKnife.SetActive(true);
                    SecondKnife.transform.position = AimRayCast();
                    SecondKnife.transform.rotation = Quaternion.FromToRotation(Vector3.back, AbilityHit.normal);
                    CmdSpawnKnife(SecondKnife, 2);
                    number_of_knifes++;
                    Debug.Log("Knifes: " + number_of_knifes);
                }
            }

        }
    }

    [Command]
    private void CmdSpawnKnife(GameObject knifepos, int i)
    {
        if (i == 1)
        {
            //Debug.Log("spawn1");
            knife1 = GameObject.Instantiate<GameObject>(this.FirstKnife, knifepos.transform.position, knifepos.transform.rotation);
            NetworkServer.Spawn(knife1);
        }
        else
        {
            //Debug.Log("spawn2");
            knife2 = GameObject.Instantiate<GameObject>(this.SecondKnife, knifepos.transform.position, knifepos.transform.rotation);
            NetworkServer.Spawn(knife2);
        }
    }

    [Command]
    private void CmdUnSpawnKnife(int i)
    {
        if (i == 1)
        {
            //Debug.Log("Unspawn1");
            number_of_knifes--;
            var knifesinscene = GameObject.FindGameObjectsWithTag("Knife");
            foreach(GameObject obj in knifesinscene)
            {
                
                if(obj.name== "SirLoin's FirstKnife.pref(Clone)")
                {
                    Debug.Log("sono nel for each 1 e ho trovaot first");
                    Destroy(obj);
                    NetworkServer.UnSpawn(obj);
                }
            }
           // GameObject obj = GameObject.Find("SirLoin's FirstKnife.pref(Clone)");
            
        }
        else
        {
            //Debug.Log("Unspawn2");
            number_of_knifes--;
            var knifesinscene = GameObject.FindGameObjectsWithTag("Knife");
            foreach (GameObject obj in knifesinscene)
            {
                if (obj.name == "SirLoin's SecondKnife.pref(Clone)")
                {
                    Debug.Log("sono nel for each 1 e ho trovaot first");
                    Destroy(obj);
                    NetworkServer.UnSpawn(obj);
                }
            }
            /* GameObject obj = GameObject.Find("SirLoin's SecondKnife.pref(Clone)");
             Destroy(obj);
             NetworkServer.UnSpawn(obj);*/
        }
    }

    //DECOMMENTARE PER IMPLEMENTARE IL DESPAWN DEI COLTELLI AL TOCCO, LASCIARE PER GIOCARE CON I COLTELLI
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Knife")
        {
            col.gameObject.SetActive(false);
            number_of_knifes--;
            Debug.Log("Knifes: " + number_of_knifes);
        }
    }
}

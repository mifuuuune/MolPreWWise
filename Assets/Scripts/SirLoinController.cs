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

    [Command]
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
                    AlternateThrow = false;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(AimRayCast().x - transform.position.x, 0, AimRayCast().z - transform.position.z)), RotationSpeed * 10);
                    anim.SetTrigger("FirstThrow");
                    StartCoroutine(StickKnife(FirstKnife, AbilityHit));
                }
                else
                {
                    AlternateThrow = true;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(AimRayCast().x - transform.position.x, 0, AimRayCast().z - transform.position.z)), RotationSpeed * 10);
                    anim.SetTrigger("SecondThrow");
                    StartCoroutine(StickKnife(SecondKnife, AbilityHit));
                }
            }

        }
    }

    IEnumerator StickKnife(GameObject Knife, RaycastHit AbilityHit)
    {
        yield return new WaitForSeconds(0.65f);
        Knife.SetActive(true);
        Knife.transform.position = AimRayCast();
        Knife.transform.rotation = Quaternion.FromToRotation(Vector3.back, AbilityHit.normal);
    }

    //DECOMMENTARE PER IMPLEMENTARE IL DESPAWN DEI COLTELLI AL TOCCO, LASCIARE PER GIOCARE CON I COLTELLI
    /*private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Knife")
        {
            col.gameObject.SetActive(false);
        }
    }*/
}

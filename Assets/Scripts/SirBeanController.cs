using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SirBeanController : BasicController
{
    public float Force = 100f;

    protected override void StatusUpdate(float CurrentInput)
    {
        if (!IsGrounded)
        {
            Jump();

            if ((!SpecialJumped) && (Input.GetButtonDown("Jump")))
            {
                SpecialJump();
            }
        }
        else
        {
            SpecialJumped = false;
            anim.SetBool("SpecialJumping", false);

            if (CurrentInput > 0)
            {
                MovementSpeed = 4.5f;
                Run();
            }
            else Idle();

            if (Input.GetMouseButton(0))
            {
                UseAbility();
                return;
            }

            if (Input.GetMouseButtonUp(0))
                anim.SetBool("UsingAbility", false);

            if (Input.GetButtonDown("Jump"))
            {
                rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
                Jump();
                return;
            }
        }
    }

    protected override void SpecialJump()
    {
        base.SpecialJump();
        rb.AddForce(Vector3.up * (JumpForce - (rb.velocity.y * rb.mass)), ForceMode.Impulse);
        anim.SetBool("SpecialJumping", true);
    }

    protected override void UseAbility()
    {
        base.UseAbility();
        //Debug.Log(AimRayCast().tag);

        anim.SetBool("UsingAbility", false);
        try
        {
            if (ProximityRayCast().tag == "Boulder" && ProximityRayCast() != null)
            {

                anim.SetBool("UsingAbility", true);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(ProximityRayCast().transform.position.x - transform.position.x, 0, ProximityRayCast().transform.position.z - transform.position.z)), RotationSpeed);
                //METTERE SIR BEAN AD UNA DATA DISTANZA DAL MASSO, QUINDI SE è TROPPO VICINO SI ALLONTANA, SE è TROPPO LONTANO SI AVVICINA
                MovementSpeed = 1f;
                ProximityRayCast().transform.Translate(transform.forward * Time.deltaTime);

            }
            else if (ProximityRayCast().tag == "Rock" && ProximityRayCast() != null)
            {

                anim.SetBool("UsingAbility", true);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(ProximityRayCast().transform.position.x - transform.position.x, 0, ProximityRayCast().transform.position.z - transform.position.z)), RotationSpeed);
                //METTERE SIR BEAN AD UNA DATA DISTANZA DAL MASSO, QUINDI SE è TROPPO VICINO SI ALLONTANA, SE è TROPPO LONTANO SI AVVICINA
                ProximityRayCast().GetComponent<Rigidbody>().AddForce(transform.forward * Force, ForceMode.Impulse);

            }
        }
        catch(NullReferenceException ex)
        {
            Debug.Log("No object in range");
        }
       
    }
}
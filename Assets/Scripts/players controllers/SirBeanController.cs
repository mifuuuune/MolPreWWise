using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class SirBeanController : BasicController
{
    public float Force = 100f;

    protected override void StatusUpdate(float CurrentInput)
    {
        base.StatusUpdate(CurrentInput);
        anim.SetBool("UsingAbility", false);

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
                CmdUseAbility();
                return;
            }

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

    
    protected override void CmdUseAbility()
    {
        base.CmdUseAbility();
        //Debug.Log(AimRayCast().tag);
        /*if (isClient)
            Force = 200;*/
       try
        {
            if (ProximityRayCast().tag == "Boulder" && ProximityRayCast() != null)
            {
                Rigidbody rb = ProximityRayCast().GetComponent<Rigidbody>();
                anim.SetBool("UsingAbility", true);
                Debug.Log("sono in boudler 1");
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(ProximityRayCast().transform.position.x - transform.position.x, 0, ProximityRayCast().transform.position.z - transform.position.z)), RotationSpeed);
                //rb.AddForce(transform.forward * Force, ForceMode.Impulse);
                CmdPush(transform.forward, Force);

            }
            else if (ProximityRayCast().tag == "Rock" && ProximityRayCast() != null)
            {

                anim.SetBool("UsingAbility", true);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(ProximityRayCast().transform.position.x - transform.position.x, 0, ProximityRayCast().transform.position.z - transform.position.z)), RotationSpeed);
                //rb.AddForce(transform.forward * Force, ForceMode.Impulse);
                

            }
        }
        catch(NullReferenceException ex)
        {
            Debug.Log("No object in range");
            //Debug.Log(ex);
        }
       
    }

    [Command]
    public void CmdPush(Vector3 direction, float force)
    {
        RpcPush(direction, force);
        //rb.AddForce(transform.forward * Force, ForceMode.Impulse);
        //ProximityRayCast().transform.Translate(transform.forward * 1.5f * Time.deltaTime);
    }

    [ClientRpc]
    public void RpcPush(Vector3 direction, float force)
    {
        rb.AddForce(direction*force, ForceMode.Impulse);
    }
}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SirSageController : BasicController {

    private bool inTrampolineState = false;
    private bool RedHooked = false;
    private bool GreenHooked = false;
    private Vector3 HookPosition;
    private float flyingSpeed = 7;
    private float StopAt = 0.5f;
    private float timer = 2f;
    private float AnimationStop = 1.3f;
    private BoxCollider BoxColl;
    private Rigidbody tramp;
    protected override void Start()
    {
        base.Start();
        BoxColl = GetComponent<BoxCollider>();
    }

    protected override void StatusUpdate(float CurrentInput)
    {
        base.StatusUpdate(CurrentInput);

        if ((Input.GetButtonUp("Jump")))
        {
            inTrampolineState = false;
            coll.enabled = true;
            BoxColl.enabled = false;
            anim.SetBool("Trampoline", false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            CmdUseAbility();
        }

        if (!inTrampolineState)
        {
            if (RedHooked)
            {
                if (Vector3.Distance(HookPosition, transform.position) > StopAt)
                {
                    transform.position = Vector3.MoveTowards(transform.position, HookPosition, flyingSpeed * Time.deltaTime);
                    rb.useGravity = false;
                }
                else
                {
                    RedHooked = false;
                    rb.useGravity = true;
                }
                return;
            }

            if (!IsGrounded)
            {
                Jump();

                if ((Input.GetButtonDown("Jump")))
                {
                    SpecialJump();
                }
            }
            else
            {

                if (timer <= AnimationStop)
                {
                    timer += Time.deltaTime;
                    anim.SetBool("Moving", false);
                    return;
                }

                if (CurrentInput > 0) Run();
                else Idle();

                if (Input.GetButtonDown("Jump"))
                {
                    rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
                    Jump();
                    return;
                }
            }
        }
    }

    protected override void SpecialJump()
    {
        base.SpecialJump();
        inTrampolineState = true;
        CmdSetTramp();
        anim.SetBool("Trampoline", true);

    }

    [Command]
    public void CmdSetTramp()
    {
        RpcSetTramp();
    }

    [ClientRpc]
    public void RpcSetTramp()
    {
        coll.enabled = false;
        BoxColl.enabled = true;
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

            if (AbilityHit.collider.gameObject.tag == "RedHook")
            {
                timer = 0;
                HookPosition = AbilityHit.collider.gameObject.transform.position + new Vector3(0, 1f, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(AimRayCast().x - transform.position.x, 0, AimRayCast().z - transform.position.z)), RotationSpeed * 10);
                anim.SetTrigger("Ability");
                if (IsGrounded)
                {
                    StartCoroutine(GoTo());

                }
                else RedHooked = true;

            }
            else
            if (AbilityHit.collider.gameObject.tag == "GreenHook")
            {
                timer = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(AimRayCast().x - transform.position.x, 0, AimRayCast().z - transform.position.z)), RotationSpeed * 10);
                anim.SetTrigger("Ability");
            }

        }
    }

    IEnumerator GoTo()
    {
        yield return new WaitForSeconds(AnimationStop);
        RedHooked = true;

    }

    void OnCollisionEnter(Collision col)
    {
        if(inTrampolineState && col.gameObject.layer == 9)
        {
            Debug.Log("sono nell'if del collision");
            GameObject obj = col.gameObject.GetComponent<Rigidbody>().gameObject;
            Vector3 EnteringForce = col.relativeVelocity * rb.mass;
            CmdTrampoline(EnteringForce, obj);
            
            //rb.AddForce(transform.up * BasicController.JumpForce, ForceMode.Impulse);
        }
    }

    [Command]
    public void CmdTrampoline(Vector3 Inforce, GameObject obj)
    {
        Debug.Log("server lo lancia sulla luna");
        RpcTrampoline(Inforce, obj);
        
    }

   [ClientRpc]
    public void RpcTrampoline(Vector3 Inforce, GameObject obj)
    {
        Debug.Log("lo lancio sulla luna client");
        Debug.Log(transform.up);
        Debug.Log(-transform.up*Inforce.y);
        //Debug.Log(tramp.gameObject.name);
        obj.GetComponent<Rigidbody>().AddForce(-transform.up * Inforce.y, ForceMode.Impulse);
    }
}

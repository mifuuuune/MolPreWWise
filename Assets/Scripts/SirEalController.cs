using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SirEalController : BasicController {

    public GameObject Plant;
    public GameObject Terrain;
    private float timer = 1.5f;
    private float AnimationStop = 1.1f;
    private bool attachedToWall = false;

    protected override void UseAbility()
    {
        base.UseAbility();


        try
        {
            if (ProximityRayCast().tag == "Terrain" && ProximityRayCast() != null)
            {
                timer = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(ProximityRayCast().transform.position.x - transform.position.x, 0, ProximityRayCast().transform.position.z - transform.position.z)), RotationSpeed * 10);
                anim.SetTrigger("Plant");
                GameObject newPlant = Instantiate<GameObject>(Plant, ProximityRayCast().transform.position - new Vector3(0, 3, 0), Quaternion.identity);
                Destroy(ProximityRayCast());
            }

            if (ProximityRayCast().tag == "Plant" && ProximityRayCast() != null)
            {
                timer = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(ProximityRayCast().transform.position.x - transform.position.x, 0, ProximityRayCast().transform.position.z - transform.position.z)), RotationSpeed * 10);
                anim.SetTrigger("Plant");
                GameObject newTerrain = Instantiate<GameObject>(Terrain, ProximityRayCast().transform.position - new Vector3(0, 1, 0), Quaternion.identity);
                Destroy(ProximityRayCast());
            }
        } catch (NullReferenceException ex)
        {
            Debug.Log("No object in range.");
        }

    }

    protected override void SpecialJump()
    {
        base.SpecialJump();
    }

    protected override void StatusUpdate(float CurrentInput)
    {
        if (!IsGrounded)
        {
            if (attachedToWall)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    anim.SetBool("IsStick", false);
                    rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
                    rb.useGravity = true;
                    attachedToWall = false;
                    Jump();
                }
            }
            else
            {
                Jump();
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

            if (Input.GetMouseButtonDown(0))
            {
                UseAbility();
                return;
            }

            if (Input.GetButtonDown("Jump"))
            {
                rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
                Jump();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject coll = collision.gameObject;

        if (coll.tag.Equals("Wall") && !IsGrounded)
        {
            anim.SetBool("IsStick", true);
            attachedToWall = true;
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-collision.contacts[0].normal), RotationSpeed * 10);
        }
    }
}

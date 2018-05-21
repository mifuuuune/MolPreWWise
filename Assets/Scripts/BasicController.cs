using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class BasicController : NetworkBehaviour
{

    public enum PlayerState { Idle, Ability, Run, Jump, SpecialJump, Fall };

    //--------------------------------------------------- COMPONENTS ---------------------------------------------------//
    protected Animator anim;
    protected Rigidbody rb;
    protected CapsuleCollider coll;
    private Vector3 dir;

    public Camera cam;
    public LayerMask GroundLayer;

    //--------------------------------------------------- CONTROL PARAMETERS ---------------------------------------------------//
    protected float MovementSpeed = 4.5f;
    protected float RotationSpeed = 0.15f;
    static public float JumpForce = 60f;
    protected float JumpSlow = 1.5f;
    protected float MaxDistance = 50f;
    public float AbilityRange = 0f;

    //--------------------------------------------------- INTERNAL PARAMETERS ---------------------------------------------------//
    public PlayerState State;
    protected bool IsGrounded;
    protected float InputX;
    protected float InputZ;
    protected bool SpecialJumped = false;

    //Initial setup, gets the components
    protected void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<CapsuleCollider>();

    }

    //Updates the internal parameters
    void Update()
    {
        if (this.isLocalPlayer)
        {
            InputX = Input.GetAxisRaw("Horizontal");
            InputZ = Input.GetAxisRaw("Vertical");
            float CurrentInput = Mathf.Sqrt(InputX * InputX + InputZ * InputZ);

            IsGrounded = GroundDetection();
            anim.SetBool("IsGrounded", IsGrounded);

            StatusUpdate(CurrentInput);
        }
        else
        {
            this.cam.enabled = false;
            this.cam.GetComponent<AudioListener>().enabled = false;
        }
    }

    //Decides the character's status
    protected virtual void StatusUpdate(float CurrentInput)
    {
        //Implemented in every character
    }

    //Detects whether the character is on the ground or not
    protected bool GroundDetection()
    {
        return Physics.CheckCapsule(coll.bounds.center, new Vector3(coll.bounds.center.x, coll.bounds.min.y, coll.bounds.center.z), coll.radius * 0.9f, GroundLayer);
    }

    //Shoots a raycast and returns the object hit id any
    protected Vector3 AimRayCast()
    {
        RaycastHit CameraHit;
        Ray CameraRay = new Ray(cam.transform.position, cam.transform.forward * MaxDistance);
        Debug.DrawRay(cam.transform.position, cam.transform.forward * MaxDistance, Color.red);
        if (Physics.Raycast(CameraRay, out CameraHit, MaxDistance))
        {
            return CameraHit.point;
        }
        else return transform.position; ;
    }

    protected GameObject ProximityRayCast()
    {
        RaycastHit AbilityHit;
        Ray AbilityRay = new Ray(transform.position + new Vector3(0, 0.1f, 0), new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z).normalized);
        if (Physics.Raycast(AbilityRay, out AbilityHit, AbilityRange))
        {
            Debug.DrawRay(transform.position + new Vector3(0, 0.1f, 0), new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z).normalized * AbilityRange, Color.green);
            return AbilityHit.collider.gameObject;
        }
        else {
            Debug.DrawRay(transform.position + new Vector3(0, 0.1f, 0), new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z).normalized * AbilityRange, Color.red);
            return null;
        }
    }

    //Calculates the movement direction given the input and the camera direction
    protected Vector3 GetDirection()
    {
        var CamForward = cam.transform.forward;
        var CamRight = cam.transform.right;

        CamForward.y = CamRight.y = 0;

        return (CamForward * InputZ + CamRight * InputX).normalized;
    }

    //When on the ground moves the character
    protected void Run()
    {
        State = PlayerState.Run;

        anim.SetBool("Moving", true);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(GetDirection()), RotationSpeed);
        transform.position += GetDirection() * MovementSpeed * Time.deltaTime;
    }

    //When on the ground stay still
    protected void Idle()
    {
        State = PlayerState.Idle;
        anim.SetBool("Moving", false);
    }

    //Whether in Idle or Run apply a vertical force to Jump
    protected void Jump()
    {
        State = PlayerState.Jump;
        if ((dir = GetDirection()) != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(GetDirection()), RotationSpeed);
            transform.position += GetDirection() * MovementSpeed / JumpSlow * Time.deltaTime;

        }
    }

    protected virtual void CmdUseAbility()
    {
        State = PlayerState.Ability;
        //AimRayCast();
        //AimDifference();
    }

    protected virtual void SpecialJump()
    {
        State = PlayerState.SpecialJump;
        SpecialJumped = true;
    }
}
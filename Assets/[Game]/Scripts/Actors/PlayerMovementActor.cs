using System.Collections;
using System.Collections.Generic;
using TriflesGames.ManagerFramework;
using UnityEngine;

public class PlayerMovementActor : Actor<PlayerManager>
{
    public Rigidbody rigidbody;
    public float speed;
    public float moveForce;
    public FloatingJoystick _joystick;
    public Animator _animator;

    public float animSpeed;
    private Vector3 oldPosition;
    private static readonly int Walk = Animator.StringToHash("Walk");


    protected override void MB_Update()
    {
        speed = rigidbody.velocity.magnitude;
    }

    protected override void MB_FixedUpdate()
    {
        rigidbody.velocity = new Vector3(_joystick.Horizontal * moveForce, rigidbody.velocity.y, _joystick.Vertical * moveForce);
    
        if (rigidbody.velocity.magnitude > 0.5f)
        {
            _animator.SetBool(Walk,true);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z)), Time.deltaTime * 75f);
        }
    
        if (rigidbody.velocity.magnitude < 0.5f)
        {
            _animator.SetBool(Walk,false);
        }
                
        animSpeed = Vector3.Distance(oldPosition, transform.position) * 100f;
        oldPosition = transform.position;
            
        _animator.SetFloat("speedMult", animSpeed / 20);
    }
}

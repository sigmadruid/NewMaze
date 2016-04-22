using UnityEngine;
using System.Collections;

namespace GameLogic
{
    [RequireComponent(typeof(CharacterController))]
    public class MoveScript : MonoBehaviour 
    {
    	public bool IsControllable = true;

    	public float MoveThreshold = 1f;

    	public float MoveSpeed = 2.0f;
    	
    	public float gravity = 5f;

    	public float angularSpeed = 10f;

    	public bool LerpRotation = true;

    	private Vector3 moveVector = Vector3.zero;
    	
    	private CollisionFlags collisionFlags; 
    	
    	public bool IsMoving { get; private set; }

    	private Transform trans;
    	private	CharacterController characterController;

    	void Awake ()
    	{
    		trans = transform;
    		characterController = GetComponent<CharacterController>();
    	}

    	void Update() 
    	{
    		if (!IsControllable)
    		{
    			return;
    		}
    		
    		if (IsMoving)
    		{
    			Vector3 movement = (moveVector + Vector3.down * gravity) * Time.deltaTime;
    			collisionFlags = characterController.Move(movement);

    			if (trans.forward != moveVector.normalized)
    			{
    				if (LerpRotation)
    				{
    					Vector3 direction = Vector3.Lerp(trans.forward, moveVector, Time.deltaTime * angularSpeed);
    					trans.localRotation = Quaternion.LookRotation(direction);
    				}
    				else
    				{
    					trans.localRotation = Quaternion.LookRotation(moveVector);
    				}
    			}
    		}
    	}
    	
    	public void Move(Vector3 moveDirection, float moveSpeed)
    	{
			moveVector = moveDirection.normalized * moveSpeed;
    		IsMoving = IsControllable && moveDirection.sqrMagnitude > MoveThreshold;
    	}

    	public void LookAt(Vector3 direction)
    	{
    		trans.localRotation = Quaternion.LookRotation(direction);
    	}

    	public bool IsGrounded () 
    	{
    		return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
    	}
    }
}
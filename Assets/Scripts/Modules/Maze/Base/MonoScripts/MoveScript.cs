using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class MoveScript : MonoBehaviour 
{
	public enum CharacterState {
		Idle = 0,
		Walking = 1,
		Trotting = 2,
		Running = 3,
		Jumping = 4,
	}
	
	private CharacterState _characterState;

	public bool IsControllable = true;

	public float MoveThreshold = 1f;

	public float MoveSpeed = 2.0f;
	
	public float gravity = 5f;

	public float angularSpeed = 10f;

	public bool LerpRotation = true;

	private Vector3 moveDirection = Vector3.zero;
	
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
			Vector3 movement = (moveDirection * MoveSpeed + Vector3.down * gravity) * Time.deltaTime;
			collisionFlags = characterController.Move(movement);

			if (trans.forward != moveDirection)
			{
				if (LerpRotation)
				{
					Vector3 direction = Vector3.Lerp(trans.forward, moveDirection, Time.deltaTime * angularSpeed);
					trans.localRotation = Quaternion.LookRotation(direction);
				}
				else
				{
					trans.localRotation = Quaternion.LookRotation(moveDirection);
				}
			}


		}

	}
	
	public void Move(Vector3 moveVector)
	{
		moveDirection = moveVector.normalized;
		IsMoving = IsControllable && moveVector.sqrMagnitude > MoveThreshold;
	}

	public void LookAt(Vector3 direction)
	{
		trans.localRotation = Quaternion.LookRotation(direction);
	}

	public bool IsGrounded () 
	{
		return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
	}
	public Vector3 GetDirection () 
	{
		return moveDirection;
	}
	
}

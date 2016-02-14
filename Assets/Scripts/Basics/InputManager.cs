using UnityEngine;
using System.Collections;

using Base;
using GameUI;
using GameLogic;
using StaticData;

public class InputManager 
{
    private const KeyCode KeyAttack = KeyCode.J;
    private const KeyCode KeyFunction = KeyCode.U;
    private const KeyCode KeyMazeMap= KeyCode.Tab;

	public Utils.CallbackVector3 CallbackUpdatePosition;
	public Utils.CallbackVoid CallbackFire;
    public Utils.CallbackVoid CallbackFunction;
	public Utils.CallbackVoid CallbackMazeMap;

	private static InputManager instance;
	public static InputManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new InputManager();
			}
			return instance;
		}
	}

	public void Init()
	{
		Enable = true;
	}

	public void Update () 
	{
		if (!Enable) return;

        if(!IsPause)
        {
            float xOffset = Input.GetAxisRaw("Horizontal");
            float zOffset = Input.GetAxisRaw("Vertical");
            DirectionVector = Quaternion.Euler(Vector3.up * (-45f)) * new Vector3(xOffset, 0, zOffset);
            DirectionVector.Normalize();
            if (CallbackUpdatePosition != null)
            {
                CallbackUpdatePosition(DirectionVector);
            }
            if(Input.GetKeyDown(KeyAttack))
            {
                if(CallbackFire != null)
                {
                    CallbackFire();
                }
            }

            if(Input.GetKeyDown(KeyFunction))
            {
                if(CallbackFunction != null)
                {
                    CallbackFunction();
                }
            }
        }

        if(Input.GetKeyDown(KeyMazeMap))
        {
            if(CallbackMazeMap != null)
            {
                CallbackMazeMap();
            }
        }
		Test();
	}

	public Vector3 DirectionVector { get; private set; }

	private bool enable = true;
	public bool Enable
	{
		get { return enable; }
		set
		{
			enable = value;
			if (!enable)
			{
				DirectionVector = Vector3.zero;
			}
		}
	}

    public bool IsPause { get; set; }

//	private bool isMoving;
//	private Vector3 directionVector;
//	private Vector3 originMousePos;
//	private Vector3 currentMousePos;
//	private float pressDelayCounter;
//	private void DragMoveAndAttack()
//	{
//		if (UICamera.hoveredObject != null)
//		{
//			return;
//		}
//		
//		bool mouseDown = Input.GetMouseButton(0);
//		Vector3 mousePosition = Input.mousePosition;
//		
//		if (Input.GetMouseButtonUp(0) && !isMoving)
//		{
//			if (CallbackUpdateFire != null)
//			{
//				CallbackUpdateFire();
//			}
//		}
//		
//		if (mouseDown)
//		{
//			if (originMousePos == Vector3.zero)
//			{
//				originMousePos = mousePosition;
//			}
//			directionVector = mousePosition - originMousePos;
//			directionVector = Vector3.Normalize(directionVector);
//			pressDelayCounter += Time.deltaTime;
//		}
//		else
//		{
//			originMousePos = Vector3.zero;
//			directionVector = Vector3.zero;
//			pressDelayCounter = 0f;
//			isMoving = false;
//			if (CallbackUpdatePosition != null)
//			{
//				CallbackUpdatePosition(directionVector);
//			}
//		}
//		
//		if (pressDelayCounter > 0.15f)
//		{
//			isMoving = true;
//			if (CallbackUpdatePosition != null)
//			{
//				CallbackUpdatePosition(directionVector);
//			}
//		}
//	}

	void Test()
	{
	}

}

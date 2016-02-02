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

	public Utils.CallbackVector3 CallbackUpdatePosition;
	public Utils.CallbackVoid CallbackFire;
	public Utils.CallbackVoid CallbackFunction;

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

		float xOffset = Input.GetAxisRaw("Horizontal");
		float zOffset = Input.GetAxisRaw("Vertical");
		DirectionVector = Quaternion.Euler(Vector3.up * (-45f)) * new Vector3(xOffset, 0, zOffset);
		DirectionVector.Normalize();

		if (CallbackUpdatePosition != null)
		{
			CallbackUpdatePosition(DirectionVector);
		}

		if (Input.GetKeyDown(KeyAttack))
		{
			if (CallbackFire != null)
			{
				CallbackFire();
			}
		}

		if (Input.GetKeyDown(KeyFunction))
		{
			if (CallbackFunction != null)
			{
				CallbackFunction();
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
		if (Input.GetKeyDown(KeyCode.F1))
		{
			BeforeTransport();
			TransitionPanel panel = PopupManager.Instance.CreateAndAddPopup<TransitionPanel>() ;
			panel.CallbackTransition = OnTransportIn;
			panel.StartTransition();
		}
		else if (Input.GetKeyDown(KeyCode.F2))
		{
			BeforeTransport();
			TransitionPanel panel = PopupManager.Instance.CreateAndAddPopup<TransitionPanel>();
			panel.CallbackTransition = OnTransportOut;
			panel.StartTransition();
		}

	}

	private void OnTransportIn()
	{
		Hero.Instance.IsSlowUpdating = false;
		ApplicationFacade.Instance.RetrieveProxy<HallProxy>().LeavePosition = Hero.Instance.WorldPosition;
		ApplicationFacade.Instance.DispatchNotification(NotificationEnum.HALL_INIT);
		ApplicationFacade.Instance.DispatchNotification(NotificationEnum.HERO_TRANSPORT, Hall.Instance.Script.EntryPos.position);
		AfterTransport();
	}
	private void OnTransportOut()
	{
		Hero.Instance.IsSlowUpdating = true;
		Vector3 leavePosition = ApplicationFacade.Instance.RetrieveProxy<HallProxy>().LeavePosition;
		ApplicationFacade.Instance.DispatchNotification(NotificationEnum.BLOCK_REFRESH, leavePosition);
		ApplicationFacade.Instance.DispatchNotification(NotificationEnum.HERO_TRANSPORT, leavePosition);
		AfterTransport();
	}

	private void BeforeTransport()
	{
		InputManager.Instance.Enable = false;
		ApplicationFacade.Instance.DispatchNotification(NotificationEnum.BATTLE_PAUSE, true);
	}
	private void AfterTransport()
	{
		InputManager.Instance.Enable = true;
		ApplicationFacade.Instance.DispatchNotification(NotificationEnum.BATTLE_PAUSE, false);
	}
}

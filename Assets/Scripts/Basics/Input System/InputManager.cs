using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using GameUI;
using GameLogic;
using StaticData;

namespace Base
{
	public class InputManager 
	{
        private Dictionary<int, KeyboardAction> keyboardActionDic = new Dictionary<int, KeyboardAction>();

        public bool IsPause { get; set; }

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

	    private const KeyCode KeyAttack = KeyCode.J;
	    private const KeyCode KeyFunction = KeyCode.U;
	    private const KeyCode KeyMazeMap = KeyCode.Tab;

        public Action CallbackAttack;
        public Action CallbackFunction;
        public Action CallbackMazeMap;

		private static InputManager instance;
		public static InputManager Instance
		{
			get
			{
				if (instance == null) instance = new InputManager();
				return instance;
			}
		}

		public void Init()
		{
            Array typeArray = Enum.GetValues(typeof(KeyboardActionType));
            foreach(var obj in typeArray)
            {
                KeyboardActionType type = (KeyboardActionType)obj;
                KeyboardAction action = new KeyboardAction();
                action.Data = KeyboardDataManager.Instance.GetData(type);
                keyboardActionDic.Add((int)type, action);
            }

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
            }

            Dictionary<int, KeyboardAction>.Enumerator enumerator = keyboardActionDic.GetEnumerator();
            while(enumerator.MoveNext())
            {
                KeyboardAction action = enumerator.Current.Value;
                if(Input.GetKeyDown(action.Data.Key))
                {
                    if(!IsPause || action.Data.ActiveWhenPause)
                    {
                        if(action.Callback != null)
                        {
                            action.Callback();
                        }
                    }
                }
            }
	           
			Test();
		}

        public void SetKeyboardAction(KeyboardActionType type, Action callback)
        {
            int typeID = (int)type;
            if(keyboardActionDic.ContainsKey(typeID))
            {
                KeyboardAction action = keyboardActionDic[typeID];
                action.Callback = callback;
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
            if(Input.GetKeyDown(KeyCode.F1))
            {
                Hero.Instance.Script.SetTransparent(true);
            }
            if(Input.GetKeyDown(KeyCode.F2))
            {
                Hero.Instance.Script.SetTransparent(false);
            }
		}

	}
}

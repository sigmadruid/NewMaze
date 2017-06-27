using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using GameUI;
using GameLogic;
using StaticData;

namespace Base
{
    public enum MouseHitType
    {
        None,
        Left,
        Right,
    }
	public class InputManager 
	{
        private GraphicRaycaster[] uiRaycasters;
        private Dictionary<int, KeyboardAction> keyboardActionDic = new Dictionary<int, KeyboardAction>();
        private Plane hitPlane = new Plane();

        private EventSystem eventSystem;
        public EventSystem InputSystem
        {
            get 
            { 
                if (eventSystem == null) 
                    eventSystem = GameObject.FindObjectOfType<EventSystem>();
                return eventSystem;
            }
        }

        #region Properties

        public bool IsPause { get; set; }

        public Vector3 DirectionVector { get; private set; }

        public MouseHitType HitType { get; private set; }

        public Vector3 MouseHitPosition { get; private set; }
        public GameObject MouseHitObject { get; private set; }

        public Vector3 MouseHoverPosition { get; private set; }
        public GameObject MouseHoverObject { get; private set; }

        public Vector3 PlaneHitPosition { get; private set; }


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
                    MouseHitPosition = Vector3.zero;
                    MouseHitObject = null;
                    MouseHoverPosition = Vector3.zero;
                    MouseHoverObject = null;
                }
            }
        }

        #endregion

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
            keyboardActionDic.Clear();
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

            MouseHitObject = null;
            PlaneHitPosition = Vector3.zero;
            MouseHitPosition = Vector3.zero;
            if(!IsPause)
            {
                float xOffset = Input.GetAxisRaw("Horizontal");
                float zOffset = Input.GetAxisRaw("Vertical");
                DirectionVector = Quaternion.Euler(Vector3.up * (-45f)) * new Vector3(xOffset, 0, zOffset);
                DirectionVector.Normalize();
//                if(DirectionVector != Vector3.zero)

                if(Input.GetMouseButtonDown(0))
                    HitType = MouseHitType.Left;
                else if(Input.GetMouseButtonDown(1))
                    HitType = MouseHitType.Right;
                else
                    HitType = MouseHitType.None;

                if(HitType != MouseHitType.None)
                {
                    PointerEventData eventData = new PointerEventData(InputSystem);
                    eventData.pressPosition = Input.mousePosition;
                    eventData.position = Input.mousePosition;

                    List<RaycastResult> raycastList = new List<RaycastResult>();
                    for(int i = 0; i < uiRaycasters.Length; ++i)
                    {
                        GraphicRaycaster raycaster = uiRaycasters[i];
                        raycaster.Raycast(eventData, raycastList);
                    }

                    if(raycastList.Count == 0)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                        Adam adam = Adam.Instance;
                        hitPlane.SetNormalAndPosition(Vector3.up, adam.WorldPosition + adam.Script.EmitPosition);
                        float distance = 0;
                        if(hitPlane.Raycast(ray, out distance))
                        {
                            PlaneHitPosition = ray.GetPoint(distance);
                        }

                        RaycastHit hitinfo;
                        if(Physics.Raycast(ray, out hitinfo, 9999f, GlobalConfig.InputConfig.MouseHitMask))
                        {
                            MouseHitPosition = hitinfo.point;
                            MouseHitObject = hitinfo.collider.gameObject;
                            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.MOUSE_HIT_OBJECT);
                        }
                    }
                }
                else
                {
                    MouseHoverPosition = Vector3.zero;
                    MouseHoverObject = null;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hitinfo;
                    if(Physics.Raycast(ray, out hitinfo, 9999f, GlobalConfig.InputConfig.MouseHoverMask))
                    {
                        MouseHoverPosition = hitinfo.point;
                        MouseHoverObject = hitinfo.collider.gameObject;
                    }
                }

            }

            if(Input.anyKeyDown)
            {
                Dictionary<int, KeyboardAction>.Enumerator enumerator = keyboardActionDic.GetEnumerator();
                while(enumerator.MoveNext())
                {
                    KeyboardAction action = enumerator.Current.Value;
                    if(Input.GetKeyDown(action.Data.Key))
                    {
                        ApplicationFacade.Instance.DispatchNotification(NotificationEnum.KEY_DOWN, action.Data.Type);
                    }
                }
            }
	           
			Test();
		}

        public void PreventMouseAction()
        {
            MouseHitObject = null;
            MouseHitPosition = Vector3.zero;
        }

        public void UpdateRaycasters()
        {
            uiRaycasters = GameObject.FindObjectsOfType<GraphicRaycaster>();
        }

        public void EnableKeyboardAction(KeyboardActionType type, bool state)
        {
            int typeID = (int)type;
            if(keyboardActionDic.ContainsKey(typeID))
            {
                KeyboardAction action = keyboardActionDic[typeID];
                action.IsEnabled = state;
            }
        }

        public bool CheckMouseHitLayer(int layer)
        {
            return MouseHitObject != null && 1 << MouseHitObject.layer == layer;
        }

		void Test()
		{
            if(Input.GetKeyDown(KeyCode.F1))
            {
                Adam.Instance.IsVisible = !Adam.Instance.IsVisible;
//                Camera3DScript.Instance.Vibrate();
            }
		}

	}
}

﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Base
{
	public abstract class BasePopupView : MonoBehaviour
	{
		#region Properties

//		public int id;


		/// <summary>
		/// 面板Prefab路径
		/// </summary>
		/// <value>The prefab path.</value>
        [HideInInspector]
		public string PrefabPath { get; set; } //TODO: Considering different panels instantiated by same prefab. Then they share the same prefab path.

		/// <summary>
		/// 弹窗的模式标识。各种面板特殊行为的开关。
		/// </summary>
        [HideInInspector]
		public uint popupMode;

		/// <summary>
		/// 参数字典。不同的模式标识对应不同的参数。
		/// </summary>
		public Dictionary<uint, object> paramDic;

        private CanvasGroup group;
        public CanvasGroup Group
		{
			get
			{
				if (group == null)
                    group = GetComponent<CanvasGroup>();
				return group;
			}
		}

        private RectTransform rectTransform;
        public RectTransform RectTransform
        {
            get
            {
                if(rectTransform == null)
                    rectTransform = GetComponent<RectTransform>();
                return rectTransform;
            }
        }

		#endregion

		#region Life Time Functions

		public delegate void UpdateDelegate();

		public UpdateDelegate UpdateShowDelegate;
		public UpdateDelegate UpdateHideDelegate;

		public virtual void onInitialize()
		{
		}

		public virtual void onDispose()
		{
		}

		public virtual void beforeEnter ()
		{
		}

		public virtual void onEnter()
		{
		}

		public virtual void onExit()
		{
		}

		public virtual void afterExit ()
		{
		}

		#endregion

		#region Popup Animation

		public BasePopupAnimation popupAnimation;
		
		public PopupAnimationType AnimationType
		{
			get
			{
				PopupAnimationDTO dto = getAnimationDTO();
				return dto.animationType;
			}
		}

		public void StartAnimation(bool reverse, BasePopupAnimation.AnimationDelegate endDelegate, object delegateParam = null)
		{
			PopupAnimationDTO dto = getAnimationDTO();
			
			popupAnimation.OnAnimationEnds = endDelegate;
			popupAnimation.endParam = delegateParam;
			popupAnimation.StartAnimation(this, dto.duration > 0 ? dto.duration : popupAnimation.DefaultDuration, reverse);
		}

		private PopupAnimationDTO getAnimationDTO()
		{
			PopupAnimationDTO dto;
			
			if (paramDic == null || !paramDic.ContainsKey(PopupMode.ANIMATED))
				dto = PopupAnimationDTO.defaultDTO;
			else
				dto = (PopupAnimationDTO)paramDic[PopupMode.ANIMATED];
			
			return dto;
		}

		#endregion
	}
}

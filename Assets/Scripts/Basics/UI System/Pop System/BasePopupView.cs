using UnityEngine;

using System;
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

        public RectTransform RectTransform
        {
            get
            {
                return transform as RectTransform;
            }
        }

		#endregion

		#region Life Time Functions

		public delegate void UpdateDelegate();

		public UpdateDelegate UpdateShowDelegate;
		public UpdateDelegate UpdateHideDelegate;

		public virtual void OnInitialize()
		{
		}

		public virtual void OnDispose()
		{
		}

		public virtual void BeforeEnter ()
		{
		}

		public virtual void OnEnter()
		{
		}

		public virtual void BeforeExit()
		{
		}

		public virtual void OnExit ()
		{
		}

		#endregion

		#region Popup Animation

        public bool IsAnimationPlaying;

        public void StartAnimation(Action<UIAnimationParam> callbackAnimEnds, UIAnimationParam param)
		{
            IsAnimationPlaying = true;
            Framework.Instance.TaskManager.AddTask(TaskEnum.UI_ANIMATION, 1f, 1, () => 
                {
                    IsAnimationPlaying = false;
                    callbackAnimEnds(param);
                });
		}



		#endregion
	}
}

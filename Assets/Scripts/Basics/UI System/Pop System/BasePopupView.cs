using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace Base
{
	public abstract class BasePopupView : MonoBehaviour
	{
		#region Properties

        [HideInInspector]
		public string PrefabPath { get; set; }

        public RectTransform RectTransform
        {
            get
            {
                return transform as RectTransform;
            }
        }

		#endregion

		#region Life Time Functions

		public virtual void OnInitialize()
		{
		}

		public virtual void OnDispose()
		{
		}

		public virtual void OnEnter()
		{
		}

		public virtual void OnExit ()
		{
		}

		#endregion

		#region Popup Animation

        public bool IsAnimationPlaying;

		#endregion
	}
}

using UnityEngine;

using System;

using Base;

using HighlightingSystem;

namespace GameLogic
{
    public class ExplorationScript : EntityScript
    {
        public Action CallbackClick;
        public Action CallbackEnter;
        public Action CallbackExit;

        public Transform IconPos;

        [HideInInspector]
        public HUDIcon Icon;

		private Highlighter highlighter;

		void Awake()
		{
			highlighter = GetComponent<Highlighter>();

            if(IconPos != null)
            {
                Icon = HUDIcon.Create(HUDIconType.Exploration);
                Icon.CallbackClick = OnIconClick;
                Icon.gameObject.SetActive(false);
            }
		}

        void OnDestroy()
        {
            if(Icon != null)
            {
                Icon.CallbackClick = null;
                HUDIcon.Recycle(Icon);
            }
        }

        void Update()
        {
            if (Icon != null && Icon.gameObject.activeSelf)
            {
                Icon.UpdatePosition(IconPos.position);
            }
        }

		void OnTriggerEnter(Collider other)
		{
            if (other.CompareTag(Tags.Hero))
			{
                if (Icon != null)
                    Icon.gameObject.SetActive(true);
                if (CallbackEnter != null) 
                    CallbackEnter();
			}
		}
		void OnTriggerExit(Collider other)
		{
            if (other.CompareTag(Tags.Hero))
			{
                if (Icon != null)
                    Icon.gameObject.SetActive(false);
                if (CallbackExit != null) 
                    CallbackExit();
			}
		}
        private void OnIconClick()
        {
            if (CallbackClick != null)
            {
                CallbackClick();
            }
        }

        public void HighlightOn(Color color)
        {
            highlighter.ConstantOn(color);
        }
        public void HighlightOff()
        {
            highlighter.ConstantOff();
        }
    }
}


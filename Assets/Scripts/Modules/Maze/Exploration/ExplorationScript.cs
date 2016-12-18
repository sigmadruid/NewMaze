using UnityEngine;

using System;

using Base;

using HighlightingSystem;

namespace GameLogic
{
    public class ExplorationScript : EntityScript
    {
        public Transform IconPos;

        private Action callbackEnter;
        private Action callbackExit;

        private HUDIcon hud;
		private Highlighter highlighter;

		void Awake()
		{
			highlighter = GetComponent<Highlighter>();
		}
        void Update()
        {
            if (hud != null && hud.gameObject.activeSelf)
            {
                hud.UpdatePosition(IconPos.position);
            }
        }
		void OnTriggerEnter(Collider other)
		{
            if (other.CompareTag(Tags.Hero))
			{
                if (hud != null)
                    hud.gameObject.SetActive(true);
                if (callbackEnter != null) 
                    callbackEnter();
			}
		}
		void OnTriggerExit(Collider other)
		{
            if (other.CompareTag(Tags.Hero))
			{
                if (hud != null)
                    hud.gameObject.SetActive(false);
                if (callbackExit != null) 
                    callbackExit();
			}
		}

        public void Init(string uid, Action click, Action enter, Action exit)
        {
            Uid = uid;
            callbackEnter = enter;
            callbackExit = exit;
            if(IconPos != null)
            {
                hud = HUDIcon.Create(HUDIconType.Exploration, click);
                hud.gameObject.SetActive(false);
            }
            transform.parent = RootTransform.Instance.ExplorationRoot;
        }
        public void Dispose()
        {
            HUDIcon.Recycle(hud);
            hud = null;
            callbackEnter = null;
            callbackExit = null;
            Uid = null;
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


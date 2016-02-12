using UnityEngine;

using System;

using Base;

using HighlightingSystem;

namespace GameLogic
{
    public class ExplorationScript : EntityScript
    {
        public Action CallbackEnter;
        public Action CallbackExit;

		private Highlighter highlighter;

		void Awake()
		{
			highlighter = GetComponent<Highlighter>();
		}

		void OnTriggerEnter(Collider other)
		{
            if (other.CompareTag(Tags.Hero))
			{
                if (CallbackEnter != null) CallbackEnter();
			}
		}
		void OnTriggerExit(Collider other)
		{
            if (other.CompareTag(Tags.Hero))
			{
                if (CallbackExit != null) CallbackExit();
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


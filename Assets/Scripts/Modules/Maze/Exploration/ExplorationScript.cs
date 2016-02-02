using UnityEngine;

using System;

using Base;

using HighlightingSystem;

namespace GameLogic
{
    public class ExplorationScript : EntityScript
    {
		private Highlighter highlighter;

		void Awake()
		{
			highlighter = GetComponent<Highlighter>();
		}

		void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag(Tags.Hero) && !Hero.Instance.InBattle)
			{
				highlighter.ConstantOn(Color.red);
			}
		}
		void OnTriggerExit(Collider other)
		{
			if (other.CompareTag(Tags.Hero) && !Hero.Instance.InBattle)
			{
				highlighter.ConstantOff();
			}
		}

    }
}


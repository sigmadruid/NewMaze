using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using StaticData;
using Base;

namespace GameLogic
{
    public class Exploration : Entity
    {
        protected ExplorationProxy proxy;

		public new ExplorationData Data
		{
			get { return data as ExplorationData; }
			set { data = value; }
		}
		
		public new ExplorationScript Script
		{
			get { return script as ExplorationScript; }
			protected set { script = value; }
		}

		public override void SetRotation(float angle)
		{
			Script.transform.localEulerAngles += Vector3.up * angle;
		}

		public virtual bool CheckAppearCondition()
		{
			return false;
		}

        public virtual void OnFunction() {}

        protected virtual void OnEnter()
        {
            Script.HighlightOn(Color.white);
            proxy.AddEnteredExploration(this);
        }

        protected virtual void OnExit()
        {
            Script.HighlightOff();
            proxy.RemoveEnteredExploration(this);
        }

        public static void Init(Exploration exploration, ExplorationData data)
        {
            exploration.Uid = Guid.NewGuid().ToString();
            exploration.Data = data;
            exploration.Script = ResourceManager.Instance.LoadAsset<ExplorationScript>(ObjectType.GameObject, exploration.Data.GetResPath());
            exploration.Script.Init(exploration.Uid, exploration.OnFunction, exploration.OnEnter, exploration.OnExit);
            exploration.proxy = ApplicationFacade.Instance.RetrieveProxy<ExplorationProxy>();
        }

		public static void Recycle(Exploration exploration)
		{
			if (exploration != null)
			{
                exploration.Script.Dispose();
                ResourceManager.Instance.RecycleAsset(exploration.Script.gameObject);
				exploration.Script = null;
                exploration.proxy = null;
                exploration.Data = null;
                exploration.Uid = null;
			}
			else
			{
				BaseLogger.Log("Recyle a null exploration!");
			}
		}
    }
}


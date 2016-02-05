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
			protected set { data = value; }
		}
		
		public new ExplorationScript Script
		{
			get { return script as ExplorationScript; }
			protected set { script = value; }
		}

		public override void SetRotation(float angle)
		{
			Script.CachedTransform.localEulerAngles += Vector3.up * angle;
		}

		public virtual bool CheckAppearCondition()
		{
			return false;
		}

        public virtual void OnFunction() {}

        protected virtual void OnEnter()
        {
            Script.HighlightOn(Color.red);
            proxy.AddEnteredExploration(this);
        }

        protected virtual void OnExit()
        {
            Script.HighlightOff();
            proxy.RemoveEnteredExploration(this);
        }

        public static void Init(Exploration exploration, ExplorationType type)
		{
			exploration.Uid = Guid.NewGuid().ToString();
            exploration.Data = ExplorationDataManager.Instance.GetRandomData(type); 
			exploration.Script = ResourceManager.Instance.LoadAsset<ExplorationScript>(ObjectType.GameObject, exploration.Data.GetResPath());
			exploration.Script.CachedTransform.parent = RootTransform.Instance.ExplorationRoot;
            exploration.Script.CallbackEnter = exploration.OnEnter;
            exploration.Script.CallbackExit = exploration.OnExit;
            exploration.proxy = ApplicationFacade.Instance.RetrieveProxy<ExplorationProxy>();
		}

		public static void Recycle(Exploration exploration)
		{
			if (exploration != null)
			{
				exploration.Data = null;
				ResourceManager.Instance.RecycleAsset(exploration.Script.gameObject);
				exploration.Script = null;
			}
			else
			{
				BaseLogger.Log("Recyle a null exploration!");
			}
		}
    }
}


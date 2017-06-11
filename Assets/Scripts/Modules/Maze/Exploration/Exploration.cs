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

        public virtual void OnFunction() 
        {
        }

        protected virtual void OnEnter()
        {
            Script.HighlightOn(Color.white);
        }

        protected virtual void OnExit()
        {
            Script.HighlightOff();
        }

        public ExplorationRecord ToRecord()
        {
            ExplorationRecord record = new ExplorationRecord();
            record.Uid = Uid;
            record.Kid = Data.Kid;
            record.WorldPosition = new Vector3Record(WorldPosition);
            record.WorldAngle = WorldAngle;
            return record;
        }

        public static void Init(Exploration exploration, ExplorationData data, string uid)
        {
            exploration.Uid = string.IsNullOrEmpty(uid) ? Guid.NewGuid().ToString() : uid;
            exploration.Data = data;
            exploration.Script = ResourceManager.Instance.LoadAsset<ExplorationScript>(ObjectType.GameObject, exploration.Data.GetResPath());
            exploration.Script.Init(exploration.Uid, exploration.OnEnter, exploration.OnExit);
        }

		public static void Recycle(Exploration exploration)
		{
			if (exploration != null)
			{
                exploration.Script.Dispose();
                ResourceManager.Instance.RecycleAsset(exploration.Script.gameObject);
				exploration.Script = null;
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


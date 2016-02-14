using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using StaticData;
using Base;

namespace GameLogic
{
    public class Entity
    {
		public string Uid { get; protected set; }

		protected EntityScript script;
		public EntityScript Script
		{
			get { return script; }
			protected set { script = value; }

		}

		protected EntityData data;
		public EntityData Data
		{
			get { return data; }
			protected set { data = value; }
		}
		
		public Vector3 WorldPosition 
		{ 
            get { return Script.transform.position; }
		}
        public float WorldAngle
        {
            get { return Script.transform.localEulerAngles.y; }
        }

		public virtual void SetPosition(Vector3 position)
		{
			Script.transform.position = position;
		}
		
		public virtual void SetRotation(float angle)
		{
			Script.transform.localRotation = Quaternion.Euler(Vector3.up * angle);
		}

		public virtual void Pause(bool isPause)
		{
            Script.Pause(isPause);
		}

		protected virtual void Update()
		{
		}

		protected virtual void SlowUpdate()
		{
		}

		public virtual EntityRecord ToRecord()
		{
			return null;
		}

    }
}


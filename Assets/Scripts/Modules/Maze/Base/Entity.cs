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

        protected EntityInfo info;
        public EntityInfo Info
        {
            get { return info; }
            protected set { info = value; }
        }
		
		public Vector3 WorldPosition 
		{ 
            get { return Script.transform.position; }
		}
        public float WorldAngle
        {
            get { return Script.transform.localEulerAngles.y; }
        }

        public Vector2 MazePosition
        {
            get
            {
                float blockSize = MazeDataManager.Instance.CurrentMazeData.BlockSize;
                int col, row;
                MazeUtil.GetMazePosition(WorldPosition, blockSize, out col, out row);
                return new Vector2(col, row);
            }
        }

		public virtual void SetPosition(Vector3 position)
		{
			Script.transform.position = position;
		}
		
		public virtual void SetRotation(float angle)
		{
			Script.transform.localRotation = Quaternion.Euler(Vector3.up * angle);
		}
        public virtual void SetRotation(Vector3 direction)
        {
            direction = new Vector3(direction.x, 0, direction.z);
            Script.transform.localRotation = Quaternion.LookRotation(direction);
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


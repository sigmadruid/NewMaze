using UnityEngine;

using System;
using System.Collections.Generic;

using Base;
using GameUI;
using StaticData;

namespace GameLogic
{
    public class Drop : Entity
    {
		public new DropData Data
		{
			get { return data as DropData; }
			protected set { data = value; }
		}
		
		public new DropScript Script
		{
			get { return script as DropScript; }
			protected set { script = value; }
		}

		public int Count;

		public void PickedUp()
		{
			TopAlertPanel.AddAlert(string.Format("Pick up coins: {0}", Count));
		}

		public void StartFlying(Vector3 position)
		{
			Script.Fly(position);
		}

		public override EntityRecord ToRecord()
        {
			DropRecord record = new DropRecord();
			record.Kid = Data.Kid;
			record.WorldPosition = WorldPosition;
			record.GoodsKid = Data.Kid;
			record.Count = Count;
			return record;
		}
            
    	public static Drop Create(DropRecord record)
		{
			Drop drop = new Drop();
			drop.Uid = record.Uid;
			drop.Data = DropDataManager.Instance.GetData(record.Kid);
			drop.Count = RandomUtils.Range(drop.Data.MinCount, drop.Data.MaxCount);
			//TODO: Switch Types
			drop.Script = ResourceManager.Instance.LoadAsset<DropScript>(ObjectType.GameObject, drop.Data.GetResPath());
			drop.Script.transform.parent = RootTransform.Instance.DropRoot;	
			
			drop.SetPosition(record.WorldPosition);
			
			return drop;
		}
		public static Drop Create(int kid, Vector3 position)
		{
			Drop drop = new Drop();
			drop.Uid = Guid.NewGuid().ToString();
			drop.Data = DropDataManager.Instance.GetData(kid);
			drop.Count = RandomUtils.Range(drop.Data.MinCount, drop.Data.MaxCount);
			//TODO: Switch Types
			drop.Script = ResourceManager.Instance.LoadAsset<DropScript>(ObjectType.GameObject, drop.Data.GetResPath());
			drop.Script.transform.parent = RootTransform.Instance.DropRoot;	
			
			drop.SetPosition(position);
			
			return drop;
		}

		public static void Recycle(Drop drop)
		{
			if (drop != null)
			{
				ResourceManager.Instance.RecycleAsset(drop.Script.gameObject);
				drop.Script = null;
				drop.Data = null;
			}
			else
			{
				BaseLogger.Log("Recyle a null drop!");
			}
		}
    }
}


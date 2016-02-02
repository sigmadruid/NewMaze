using UnityEngine;

using System;
using System.Collections.Generic;
using System.Linq;

using Base;
using GameLogic;
using StaticData;


namespace AI
{
	public class AICore
	{
		public const float AI_UPDATE_INTERVAL = 0.2f;

		private Dictionary<string, AIBase> aiDic;

		public AICore ()
		{
			aiDic = new Dictionary<string, AIBase>();
		}

		public void Update()
		{
			Dictionary<string, AIBase>.Enumerator enumerator = aiDic.GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.Value.Update();
			}
		}
		public void SlowUpdate()
		{
			Dictionary<string, AIBase>.Enumerator enumerator = aiDic.GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.Value.SlowUpdate();
			}
		}

		public void AddAI(Monster monster)
		{
			if (aiDic.ContainsKey(monster.Uid))
			{
				BaseLogger.LogFormat("Ai already exists: {0}", monster.Uid);
			}

			AIBase ai = null;
			if (monster.Data.AttackType == AttackType.Melee)
			{
				ai = new MeleeAI(monster);
			}
			else
			{
				ai = new ShooterAI(monster);
			}

			aiDic.Add(ai.Uid, ai);

			ai.Start();
		}

		public void RemoveAI(string uid)
		{
			if (!aiDic.ContainsKey(uid))
			{
				BaseLogger.LogFormat("Ai doesn't exists: {0}", uid);
			}

			AIBase ai = aiDic[uid];
			ai.End();

			aiDic.Remove(uid);
		}

	}
}


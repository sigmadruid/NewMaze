using UnityEngine;

using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
	public class AnimatorDataManager
	{
		public int IdleHash;
		public int RunHash;
		public int Attack1Hash;
		public int Attack2Hash;
		public int HitHash;
		public int DieHash;

		public int ParamIsMoving;
		public int ParamDoAttack;
		public int ParamAttackRandomValue;
		public int ParamIsHit;
		public int ParamDoDie;

		private Dictionary<int, Dictionary<int, AnimatorData>> kvDic;

		private static AnimatorDataManager instance;
		public static AnimatorDataManager Instance
		{
			get
			{
				if (instance == null) instance = new AnimatorDataManager();
				return instance;
			}
		}

		public AnimatorDataManager ()
		{
			IdleHash = Animator.StringToHash("Base.idle");
			RunHash = Animator.StringToHash("Base.run");
			Attack1Hash = Animator.StringToHash("Base.attack01");
			Attack2Hash = Animator.StringToHash("Base.attack02");
			HitHash = Animator.StringToHash("Base.hit");
			DieHash = Animator.StringToHash("Base.die");

			ParamIsMoving = Animator.StringToHash("isMoving");
			ParamDoAttack = Animator.StringToHash("doAttack");
			ParamAttackRandomValue = Animator.StringToHash("attackRandomValue");
			ParamIsHit = Animator.StringToHash("isHit");
			ParamDoDie = Animator.StringToHash("doDie");
		}

		public void Init()
		{
			AnimatorDataParser parser = new AnimatorDataParser();
			parser.Parse("AnimatorDataConfig.csv", out kvDic);
		}

		public Dictionary<int, AnimatorData> GetDataDic(int kid)
		{
			if (kvDic.ContainsKey(kid))
			{
				return kvDic[kid];
			}
			else
			{
				BaseLogger.LogFormat("No such animator dictionary: {0}", kid);
				return null;
			}
		}

		public AnimatorData GetAnimatorData(int kid, int nameHash)
		{
			if (kvDic.ContainsKey(kid))
			{
				Dictionary<int, AnimatorData> dataDic = kvDic[kid];
				if (dataDic.ContainsKey(nameHash))
				{
					AnimatorData data = dataDic[nameHash];
					return data;
				}
				else
				{
					BaseLogger.LogFormat("No such animator data: {0}", nameHash);
				}
			}
			else
			{
				BaseLogger.LogFormat("No such animator data: {0}", kid);
			}
			return null;
		}
	}
}


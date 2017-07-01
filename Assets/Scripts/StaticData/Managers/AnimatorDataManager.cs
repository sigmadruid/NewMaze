using UnityEngine;

using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
	public class AnimatorDataManager
	{
        public int NullHash;
		public int IdleHash;
		public int MoveHash;
        public int SkillHash_1;
        public int SkillHash_2;
		public int Attack1Hash;
		public int Attack2Hash;
		public int HitHash;
        public int DieHash;

		public int ParamIsMoving;
        public int ParamDoSkill_1;
        public int ParamDoSkill_2;
		public int ParamDoAttack;
		public int ParamAttackRandomValue;
		public int ParamDoHit;
        public int ParamDoDie;
        public int ParamDoExit;
        public int ParamDoUnarmed;
        public int ParamDoSword;
        public int ParamDoAxe;
        public int ParamDoBow;
        public int ParamRoll;

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
            NullHash = Animator.StringToHash("Base.null");
			IdleHash = Animator.StringToHash("Base.idle");
			MoveHash = Animator.StringToHash("Base.move");
            SkillHash_1 = Animator.StringToHash("Base.skill_1");
            SkillHash_2 = Animator.StringToHash("Base.skill_2");
			Attack1Hash = Animator.StringToHash("Base.attack01");
			Attack2Hash = Animator.StringToHash("Base.attack02");
			HitHash = Animator.StringToHash("Base.hit");
			DieHash = Animator.StringToHash("Base.die");

			ParamIsMoving = Animator.StringToHash("IsMoving");
            ParamDoSkill_1 = Animator.StringToHash("DoSkill_1");
            ParamDoSkill_2 = Animator.StringToHash("DoSkill_2");
			ParamDoAttack = Animator.StringToHash("DoAttack");
			ParamAttackRandomValue = Animator.StringToHash("AttackRandomValue");
			ParamDoHit = Animator.StringToHash("DoHit");
            ParamDoDie = Animator.StringToHash("DoDie");
            ParamDoExit = Animator.StringToHash("DoExit");
            ParamDoUnarmed = Animator.StringToHash("DoUnarmed");
            ParamDoSword = Animator.StringToHash("DoSword");
            ParamDoAxe = Animator.StringToHash("DoAxe");
            ParamDoBow = Animator.StringToHash("DoBow");
            ParamRoll = Animator.StringToHash("Roll");
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
        public AnimatorData GetAdamAnimatorData(int nameHash)
        {
            Dictionary<int, AnimatorData> dataDic = kvDic[30001];
            if (dataDic.ContainsKey(nameHash))
            {
                AnimatorData data = dataDic[nameHash];
                return data;
            }
            return null;
        }
	}
}


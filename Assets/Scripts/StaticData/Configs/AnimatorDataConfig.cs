using UnityEngine;

using System;
using System.Collections.Generic;

using Base;

namespace StaticData
{
	public class AnimatorDataConfig
	{
		public Dictionary<int, Dictionary<int, AnimatorData>> animatorDataDic; 

		public AnimatorDataConfig ()
		{
			IDManager idManager = IDManager.Instance;
			animatorDataDic = new Dictionary<int, Dictionary<int, AnimatorData>>();
			Dictionary<int, AnimatorData> dataDic;
			AnimatorData data;

			#region Heroes
			//-------------Hero_1----------------
			dataDic = new Dictionary<int, AnimatorData>();

			data = new AnimatorData();
			data.Kid = idManager.GetID(IDType.Hero, 1);
			data.Name = "Base.idle";
			data.NameHash = Animator.StringToHash(data.Name);
			data.IsLoop = true;
			data.Priority = AnimatorPriorityEnum.Idle;
			data.NormalTime = 0;
			dataDic.Add(data.NameHash, data);

			data = new AnimatorData();
			data.Kid = idManager.GetID(IDType.Hero, 1);
			data.Name = "Base.run";
			data.NameHash = Animator.StringToHash(data.Name);
			data.IsLoop = true;
			data.Priority = AnimatorPriorityEnum.Run;
			data.NormalTime = 0;
			dataDic.Add(data.NameHash, data);

			data = new AnimatorData();
			data.Kid = idManager.GetID(IDType.Hero, 1);
			data.Name = "Base.attack01";
			data.NameHash = Animator.StringToHash(data.Name);
			data.IsLoop = false;
			data.Priority = AnimatorPriorityEnum.Attack;
			data.NormalTime = 0.4f;
			data.ParamDic[AnimatorParamKey.AreaType] = (int)AreaType.Rectangle;
			data.ParamDic[AnimatorParamKey.Range] = 3;
			data.ParamDic[AnimatorParamKey.Width] = 1;
			dataDic.Add(data.NameHash, data);

			data = new AnimatorData();
			data.Kid = idManager.GetID(IDType.Hero, 1);
			data.Name = "Base.attack02";
			data.NameHash = Animator.StringToHash(data.Name);
			data.IsLoop = false;
			data.Priority = AnimatorPriorityEnum.Attack;
			data.NormalTime = 0.4f;
			data.ParamDic[AnimatorParamKey.AreaType] = (int)AreaType.Fan;
			data.ParamDic[AnimatorParamKey.Range] = 2;
			data.ParamDic[AnimatorParamKey.Angle] = 60;
			dataDic.Add(data.NameHash, data);

			data = new AnimatorData();
			data.Kid = idManager.GetID(IDType.Hero, 1);
			data.Name = "Base.hit";
			data.NameHash = Animator.StringToHash(data.Name);
			data.IsLoop = false;
			data.Priority = AnimatorPriorityEnum.Hit;
			data.NormalTime = 0;
			dataDic.Add(data.NameHash, data);

			data = new AnimatorData();
			data.Kid = idManager.GetID(IDType.Hero, 1);
			data.Name = "Base.die";
			data.NameHash = Animator.StringToHash(data.Name);
			data.IsLoop = false;
			data.Priority = AnimatorPriorityEnum.Die;
			data.NormalTime = 0;
			dataDic.Add(data.NameHash, data);

			animatorDataDic.Add(data.Kid, dataDic);

			//-------------Hero_2----------------
			dataDic = new Dictionary<int, AnimatorData>();
			
			data = new AnimatorData();
			data.Kid = idManager.GetID(IDType.Hero, 2);
			data.Name = "Base.idle";
			data.NameHash = Animator.StringToHash(data.Name);
			data.IsLoop = true;
			data.Priority = AnimatorPriorityEnum.Idle;
			data.NormalTime = 0;
			dataDic.Add(data.NameHash, data);
			
			data = new AnimatorData();
			data.Kid = idManager.GetID(IDType.Hero, 2);
			data.Name = "Base.run";
			data.NameHash = Animator.StringToHash(data.Name);
			data.IsLoop = true;
			data.Priority = AnimatorPriorityEnum.Run;
			data.NormalTime = 0;
			dataDic.Add(data.NameHash, data);
			
			data = new AnimatorData();
			data.Kid = idManager.GetID(IDType.Hero, 2);
			data.Name = "Base.attack01";
			data.NameHash = Animator.StringToHash(data.Name);
			data.IsLoop = false;
			data.Priority = AnimatorPriorityEnum.Attack;
			data.NormalTime = 0.4f;
			data.ParamDic[AnimatorParamKey.AreaType] = (int)AreaType.Fan;
			data.ParamDic[AnimatorParamKey.Range] = 3;
			data.ParamDic[AnimatorParamKey.Angle] = 120;
			dataDic.Add(data.NameHash, data);
			
			data = new AnimatorData();
			data.Kid = idManager.GetID(IDType.Hero, 2);
			data.Name = "Base.attack02";
			data.NameHash = Animator.StringToHash(data.Name);
			data.IsLoop = false;
			data.Priority = AnimatorPriorityEnum.Attack;
			data.NormalTime = 0.4f;
			data.ParamDic[AnimatorParamKey.AreaType] = (int)AreaType.Fan;
			data.ParamDic[AnimatorParamKey.Range] = 3;
			data.ParamDic[AnimatorParamKey.Angle] = 360;
			dataDic.Add(data.NameHash, data);
			
			data = new AnimatorData();
			data.Kid = idManager.GetID(IDType.Hero, 2);
			data.Name = "Base.hit";
			data.NameHash = Animator.StringToHash(data.Name);
			data.IsLoop = false;
			data.Priority = AnimatorPriorityEnum.Hit;
			data.NormalTime = 0;
			dataDic.Add(data.NameHash, data);
			
			data = new AnimatorData();
			data.Kid = idManager.GetID(IDType.Hero, 2);
			data.Name = "Base.die";
			data.NameHash = Animator.StringToHash(data.Name);
			data.IsLoop = false;
			data.Priority = AnimatorPriorityEnum.Die;
			data.NormalTime = 0;
			dataDic.Add(data.NameHash, data);
			
			animatorDataDic.Add(data.Kid, dataDic);

			#endregion

			#region Monsters

			//-------------Monster_1----------------
			dataDic = new Dictionary<int, AnimatorData>();
			
			data = new AnimatorData();
			data.Kid = idManager.GetID(IDType.Monster, 1);
			data.Name = "Base.idle";
			data.NameHash = Animator.StringToHash(data.Name);
			data.IsLoop = true;
			data.Priority = AnimatorPriorityEnum.Idle;
			data.NormalTime = 0;
			dataDic.Add(data.NameHash, data);
			
			data = new AnimatorData();
			data.Kid = idManager.GetID(IDType.Monster, 1);
			data.Name = "Base.run";
			data.NameHash = Animator.StringToHash(data.Name);
			data.IsLoop = true;
			data.Priority = AnimatorPriorityEnum.Run;
			data.NormalTime = 0;
			dataDic.Add(data.NameHash, data);
			
			data = new AnimatorData();
			data.Kid = idManager.GetID(IDType.Monster, 1);
			data.Name = "Base.attack01";
			data.NameHash = Animator.StringToHash(data.Name);
			data.IsLoop = false;
			data.Priority = AnimatorPriorityEnum.Attack;
			data.NormalTime = 0;
			data.ParamDic[AnimatorParamKey.AreaType] = (int)AreaType.Fan;
			data.ParamDic[AnimatorParamKey.Range] = 2;
			data.ParamDic[AnimatorParamKey.Angle] = 60;
			dataDic.Add(data.NameHash, data);
			
			data = new AnimatorData();
			data.Kid = idManager.GetID(IDType.Monster, 1);
			data.Name = "Base.attack02";
			data.NameHash = Animator.StringToHash(data.Name);
			data.IsLoop = false;
			data.Priority = AnimatorPriorityEnum.Attack;
			data.NormalTime = 0;
			data.ParamDic[AnimatorParamKey.AreaType] = (int)AreaType.Fan;
			data.ParamDic[AnimatorParamKey.Range] = 2;
			data.ParamDic[AnimatorParamKey.Angle] = 60;
			dataDic.Add(data.NameHash, data);
			
			data = new AnimatorData();
			data.Kid = idManager.GetID(IDType.Monster, 1);
			data.Name = "Base.die";
			data.NameHash = Animator.StringToHash(data.Name);
			data.IsLoop = false;
			data.Priority = AnimatorPriorityEnum.Die;
			data.NormalTime = 0;
			dataDic.Add(data.NameHash, data);
			
			animatorDataDic.Add(data.Kid, dataDic);

			//-------------Monster_2----------------
			dataDic = new Dictionary<int, AnimatorData>();
			
			data = new AnimatorData();
			data.Kid = idManager.GetID(IDType.Monster, 2);
			data.Name = "Base.idle";
			data.NameHash = Animator.StringToHash(data.Name);
			data.IsLoop = true;
			data.Priority = AnimatorPriorityEnum.Idle;
			data.NormalTime = 0;
			dataDic.Add(data.NameHash, data);
			
			data = new AnimatorData();
			data.Kid = idManager.GetID(IDType.Monster, 2);
			data.Name = "Base.run";
			data.NameHash = Animator.StringToHash(data.Name);
			data.IsLoop = true;
			data.Priority = AnimatorPriorityEnum.Run;
			data.NormalTime = 0;
			dataDic.Add(data.NameHash, data);
			
			data = new AnimatorData();
			data.Kid = idManager.GetID(IDType.Monster, 2);
			data.Name = "Base.attack01";
			data.NameHash = Animator.StringToHash(data.Name);
			data.IsLoop = false;
			data.Priority = AnimatorPriorityEnum.Attack;
			data.NormalTime = 0;
			data.ParamDic[AnimatorParamKey.AreaType] = (int)AreaType.Fan;
			data.ParamDic[AnimatorParamKey.Range] = 2;
			data.ParamDic[AnimatorParamKey.Angle] = 60;
			dataDic.Add(data.NameHash, data);
			
			data = new AnimatorData();
			data.Kid = idManager.GetID(IDType.Monster, 2);
			data.Name = "Base.attack02";
			data.NameHash = Animator.StringToHash(data.Name);
			data.IsLoop = false;
			data.Priority = AnimatorPriorityEnum.Attack;
			data.NormalTime = 0;
			data.ParamDic[AnimatorParamKey.AreaType] = (int)AreaType.Fan;
			data.ParamDic[AnimatorParamKey.Range] = 2;
			data.ParamDic[AnimatorParamKey.Angle] = 60;
			dataDic.Add(data.NameHash, data);
			
			data = new AnimatorData();
			data.Kid = idManager.GetID(IDType.Monster, 2);
			data.Name = "Base.die";
			data.NameHash = Animator.StringToHash(data.Name);
			data.IsLoop = false;
			data.Priority = AnimatorPriorityEnum.Die;
			data.NormalTime = 0;
			dataDic.Add(data.NameHash, data);
			
			animatorDataDic.Add(data.Kid, dataDic);

			//-------------Monster_3----------------
			dataDic = new Dictionary<int, AnimatorData>();
			
			data = new AnimatorData();
			data.Kid = idManager.GetID(IDType.Monster, 3);
			data.Name = "Base.idle";
			data.NameHash = Animator.StringToHash(data.Name);
			data.IsLoop = true;
			data.Priority = AnimatorPriorityEnum.Idle;
			data.NormalTime = 0;
			dataDic.Add(data.NameHash, data);
			
			data = new AnimatorData();
			data.Kid = idManager.GetID(IDType.Monster, 3);
			data.Name = "Base.run";
			data.NameHash = Animator.StringToHash(data.Name);
			data.IsLoop = true;
			data.Priority = AnimatorPriorityEnum.Run;
			data.NormalTime = 0;
			dataDic.Add(data.NameHash, data);
			
			data = new AnimatorData();
			data.Kid = idManager.GetID(IDType.Monster, 3);
			data.Name = "Base.attack01";
			data.NameHash = Animator.StringToHash(data.Name);
			data.IsLoop = false;
			data.Priority = AnimatorPriorityEnum.Attack;
			data.NormalTime = 0.5f;
			data.ParamDic[AnimatorParamKey.AreaType] = (int)AreaType.Rectangle;
			data.ParamDic[AnimatorParamKey.Range] = 2;
			data.ParamDic[AnimatorParamKey.Width] = 1;
			dataDic.Add(data.NameHash, data);
			
			data = new AnimatorData();
			data.Kid = idManager.GetID(IDType.Monster, 3);
			data.Name = "Base.attack02";
			data.NameHash = Animator.StringToHash(data.Name);
			data.IsLoop = false;
			data.Priority = AnimatorPriorityEnum.Attack;
			data.NormalTime = 0.5f;
			data.ParamDic[AnimatorParamKey.AreaType] = (int)AreaType.Rectangle;
			data.ParamDic[AnimatorParamKey.Range] = 2;
			data.ParamDic[AnimatorParamKey.Width] = 1;
			dataDic.Add(data.NameHash, data);
			
			data = new AnimatorData();
			data.Kid = idManager.GetID(IDType.Monster, 3);
			data.Name = "Base.die";
			data.NameHash = Animator.StringToHash(data.Name);
			data.IsLoop = false;
			data.Priority = AnimatorPriorityEnum.Die;
			data.NormalTime = 0;
			dataDic.Add(data.NameHash, data);
			
			animatorDataDic.Add(data.Kid, dataDic);

			#endregion
		}
	}
}


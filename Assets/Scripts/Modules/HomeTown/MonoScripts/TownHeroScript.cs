using UnityEngine;
using System.Collections;

using Base;
using GameLogic;
using StaticData;

public class TownHeroScript : CharacterScript 
{
	public MeleeWeaponTrail MeleeTrail;
	
	protected override void Start ()
	{
		AnimatorDataDic = AnimatorDataManager.Instance.GetDataDic(HeroDataManager.Instance.CurrentHeroData.Kid);

		MeleeTrail.Emit = false;

		base.Start ();
		
	}
}

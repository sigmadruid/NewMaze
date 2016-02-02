using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Base;
using GameLogic;
using StaticData;

public class BattleUIPanel : BasePopupView 
{
	public Utils.CallbackVoid CallbackUpdate;
	public Utils.CallbackVoid CallbackItemClick;

	public UIButton ButtonPause;
	public UIButton ButtonShowMap;
	public UIGrid GridHero;
	public UISlider HPBar;
	public UISlider MPBar;
	public NumberItem HPNumber;
	public NumberItem MPNumber;

	[HideInInspector]
	public HeroItem CurrentItem;

	private List<HeroItem> heroItemList;

	void Awake () 
	{
		heroItemList = new List<HeroItem>();

		ResourceManager.Instance.PreloadAsset(ObjectType.GameObject, "UI/Items/HeroItem", -1, 3);
	}

	void Update()
	{
		CallbackUpdate();
	}

	public void Init(List<HeroData> dataList)
	{
		PanelUtils.ClearChildren(GridHero.transform);

		for (int i = 0; i < dataList.Count; ++i)
		{
			HeroItem item = ResourceManager.Instance.LoadAsset<HeroItem>(ObjectType.GameObject, "UI/Items/HeroItem");
			item.transform.parent = GridHero.transform;
			item.transform.localScale = Vector3.one;

			HeroData data = dataList[i];
			item.Data = data;
			item.Label.text = TextDataManager.Instance.GetData(data.Name);
			UIEventListener.Get(item.gameObject).onClick = OnItemClick;

			heroItemList.Add(item);
		}
	}

	public void ShowHeroItems(bool isShow)
	{
		for (int i = 0; i < heroItemList.Count; ++i)
		{
			HeroItem item = heroItemList[i];
			item.gameObject.SetActive(isShow);
		}
	}
	public void UpdateSliderBar(float hpVal, float mpVal)
	{
		HPBar.value = hpVal;
		MPBar.value = mpVal;
	}
	public void UpdateHPNumber(AttackResult ar)
	{
		HPNumber.Show(ar);
	}
	public void UpdateMPNumber(int value)
	{
		MPNumber.Show(value.ToString());
    }

	private void OnItemClick(GameObject go)
	{
		CurrentItem = go.GetComponent<HeroItem>();
		CallbackItemClick();
	}



}

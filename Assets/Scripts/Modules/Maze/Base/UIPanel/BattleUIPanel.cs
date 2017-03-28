using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using Battle;
using GameLogic;
using StaticData;

namespace GameUI
{
    public class BattleUIPanel : BasePopupView 
    {
    	public Action CallbackUpdate;
    	public Action CallbackItemClick;

        public Image ImageHero;
        public Button ButtonPause;
        public Button ButtonPack;
        public VerticalLayoutGroup LayoutHeroes;
        public Slider HPBar;
        public Slider MPBar;
    	public NumberItem HPNumber;
    	public NumberItem MPNumber;

    	[HideInInspector]
    	public HeroItem CurrentItem;

    	private List<HeroItem> heroItemList;

    	void Awake () 
    	{
    		heroItemList = new List<HeroItem>();
    	}

    	void Update()
    	{
    		CallbackUpdate();
    	}

    	public void Init(List<HeroData> dataList)
    	{
    		PanelUtils.ClearChildren(LayoutHeroes.transform);

    		for (int i = 0; i < dataList.Count; ++i)
    		{
                HeroItem item = PopupManager.Instance.CreateItem<HeroItem>(LayoutHeroes.transform);
    			HeroData data = dataList[i];
                int id = IDManager.Instance.GetID(data.Kid);
                if(id == 0)
                    continue;
    			item.Data = data;
                item.ImageHero.sprite = PanelUtils.CreateSprite(PanelUtils.ATLAS_PORTRAIT, data.Res2D);
                item.TextName.text = TextDataManager.Instance.GetData(data.Name);
                EventTriggerListener.Get(item.gameObject).onClick = OnItemClick;

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
            ImageHero.sprite = PanelUtils.CreateSprite(PanelUtils.ATLAS_PORTRAIT, CurrentItem.Data.Res2D);

            CallbackItemClick();
    	}

    }
}

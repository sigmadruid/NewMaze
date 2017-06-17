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
        public Action CallbackHeroItemClick;
    	public Action CallbackProfileClick;

        public Image ImageHero;
        public Button ButtonPause;
        public Button ButtonPack;
        public VerticalLayoutGroup LayoutHeroes;
        public TransitionProgressBar HPBar;
        public Slider MPBar;
    	public NumberItem HPNumber;
    	public NumberItem MPNumber;

    	[HideInInspector]
    	public HeroItem CurrentItem;

        private List<HeroItem> heroItemList = new List<HeroItem>();

        public override void OnInitialize()
        {
            base.OnInitialize();

            ClickEventTrigger.Get(ImageHero.gameObject).onClick = OnProfileClicked;
    	}
        public override void OnDispose()
        {
            base.OnDispose();
        }

        public void SetData(List<int> kid)
    	{
    		PanelUtils.ClearChildren(LayoutHeroes.transform);

    		for (int i = 0; i < kid.Count; ++i)
    		{
                HeroData data = HeroDataManager.Instance.GetData(kid[i]) as HeroData;
                int id = IDManager.Instance.GetID(data.Kid);
                if(id == 0)
                    continue;
                HeroItem item = PopupManager.Instance.CreateItem<HeroItem>(LayoutHeroes.transform);
    			item.Data = data;
                item.ImageHero.sprite = PanelUtils.CreateSprite(PanelUtils.ATLAS_PORTRAIT, data.Res2D);
                item.TextName.text = TextDataManager.Instance.GetData(data.Name);
                ClickEventTrigger.Get(item.gameObject).onClick = OnHeroItemClick;

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
        public void UpdateLifeBar(float hpVal, bool isAnim)
    	{
            HPBar.SetValue(hpVal, isAnim);
    	}
        public void UpdateMagicBar(float mpVal)
        {
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

    	private void OnHeroItemClick(GameObject go)
    	{
    		CurrentItem = go.GetComponent<HeroItem>();
            ImageHero.sprite = PanelUtils.CreateSprite(PanelUtils.ATLAS_PORTRAIT, CurrentItem.Data.Res2D);

            CallbackHeroItemClick();
    	}
        private void OnProfileClicked(GameObject go)
        {
            CallbackProfileClick();
        }
    }
}

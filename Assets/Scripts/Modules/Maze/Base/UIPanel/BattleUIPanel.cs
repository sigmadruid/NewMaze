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

        public SkillItem[] SkillItemList;

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

        void Update()
        {
            HeroInfo info = Adam.Instance.Info;
            int currentSP = info.SP;
            float maxSP = info.GetBaseAttribute(BattleAttribute.SP);
            UpdateSPBar(currentSP / maxSP);
        }

        public void SetHeroListData(List<int> kidList)
    	{
    		PanelUtils.ClearChildren(LayoutHeroes.transform);

    		for (int i = 0; i < kidList.Count; ++i)
    		{
                HeroData data = HeroDataManager.Instance.GetData(kidList[i]) as HeroData;
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

            SetHeroData();
    	}
        public void SetHeroData()
        {
            ImageHero.sprite = PanelUtils.CreateSprite(PanelUtils.ATLAS_PORTRAIT, Adam.Instance.Data.Res2D);
            for(int i = 0; i < SkillItemList.Length; ++i)
            {
                SkillItem item = SkillItemList[i];
                Skill skill = Adam.Instance.Info.GetSkill(i + 1);
                item.SetData(skill);
            }
        }

        public void UpdateHPBar(float hpRatio, bool isAnim)
    	{
            HPBar.SetValue(hpRatio, isAnim);
    	}
        public void UpdateSPBar(float spRatio)
        {
            MPBar.value = spRatio;
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

            CallbackHeroItemClick();
    	}
        private void OnProfileClicked(GameObject go)
        {
            CallbackProfileClick();
        }
    }
}

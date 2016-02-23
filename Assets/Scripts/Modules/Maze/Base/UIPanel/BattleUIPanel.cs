using UnityEngine;
using UnityEngine.UI;

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
    	public Utils.CallbackVoid CallbackUpdate;
    	public Utils.CallbackVoid CallbackItemClick;

        public Button ButtonPause;
        public GridLayoutGroup GridHero;
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
    		PanelUtils.ClearChildren(GridHero.transform);

    		for (int i = 0; i < dataList.Count; ++i)
    		{
    			HeroItem item = ResourceManager.Instance.LoadAsset<HeroItem>(ObjectType.GameObject, "NewUI/Items/HeroItem");
                item.transform.SetParent(GridHero.transform);

    			HeroData data = dataList[i];
    			item.Data = data;
    			item.Label.text = TextDataManager.Instance.GetData(data.Name);
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
    		CallbackItemClick();
    	}



    }
}

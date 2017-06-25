using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections.Generic;

using Base;
using Battle;
using StaticData;

namespace GameUI
{
    public class ProfilePanel : BasePopupView
    {
        public Button buttonClose;
        public Button buttonLeft;
        public Button buttonRight;
        public UITargetTexture targetTexture;

        public Text textHeroName;
        public Text textHeroLevel;

        public AttributeItem AttributeItemPrefab;
        public GridLayoutGroup gridAttributes;
        private UIItemPool<AttributeItem> itemPool = new UIItemPool<AttributeItem>();

        private int index;
        private List<int> kidList;
        private HeroInfo heroInfo;

        public override void OnInitialize()
        {
            base.OnInitialize();

            itemPool.Init(AttributeItemPrefab.gameObject, gridAttributes.transform);

            ClickEventTrigger.Get(buttonClose.gameObject).onClick = OnClose;
            ClickEventTrigger.Get(buttonLeft.gameObject).onClick = OnLeft;
            ClickEventTrigger.Get(buttonRight.gameObject).onClick = OnRight;
        }

        public override void OnDispose()
        {
            base.OnDispose();
        }

        public void SetData(List<int> kidList)
        {
            this.kidList = kidList;
            index = 0;

            int kid = kidList[0];
            HeroData data = HeroDataManager.Instance.GetData(kid) as HeroData;
            heroInfo = new HeroInfo(data);
            SetSingleData(kid);
        }

        private void SetSingleData(int kid)
        {
            HeroData data = HeroDataManager.Instance.GetData(kid) as HeroData;
            heroInfo.Convert(data);

            GameObject hero = ResourceManager.Instance.CreateGameObject("Heroes/ProfileHero");
            targetTexture.Set(hero);

            textHeroName.text = TextDataManager.Instance.GetData(data.Name);
            textHeroLevel.text = "Lv." + heroInfo.Level.ToString();

            itemPool.RemoveAll();
            Array attrs = Enum.GetValues(typeof(BattleAttribute));
            foreach(var obj in attrs)
            {
                BattleAttribute attr = (BattleAttribute)obj;
                itemPool.AddItem().SetData(attr, heroInfo.GetBaseAttribute(attr));
            }
        }

        private void OnClose(GameObject go)
        {
            PopupManager.Instance.RemovePopup(this);
        }
        private void OnLeft(GameObject go)
        {
            index = index > 0 ? index - 1 : kidList.Count - 1;
            SetSingleData(kidList[index]);
        }
        private void OnRight(GameObject go)
        {
            index = index < kidList.Count - 1 ? index + 1 : 0;
            SetSingleData(kidList[index]);
        }
    }
}


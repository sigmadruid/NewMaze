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
        private List<HeroInfo> infoList;

        public override void OnInitialize()
        {
            base.OnInitialize();

            itemPool.Init(AttributeItemPrefab.gameObject, gridAttributes.transform);

            EventTriggerListener.Get(buttonClose.gameObject).onClick = OnClose;
            EventTriggerListener.Get(buttonLeft.gameObject).onClick = OnLeft;
            EventTriggerListener.Get(buttonRight.gameObject).onClick = OnRight;
        }

        public override void OnDispose()
        {
            base.OnDispose();
        }

        public void SetData(List<HeroInfo> infoList)
        {
            this.infoList = infoList;
            index = 0;
            SetSingleData(infoList[0]);
        }

        private void SetSingleData(HeroInfo info)
        {
            GameObject hero = ResourceManager.Instance.CreateGameObject("Heroes/MazeMapHero");
            targetTexture.Set(hero);

            textHeroName.text = TextDataManager.Instance.GetData(info.Data.Name);
            textHeroLevel.text = "Lv." + info.Level.ToString();

            itemPool.RemoveAll();
            Array attrs = Enum.GetValues(typeof(BattleAttribute));
            foreach(var obj in attrs)
            {
                BattleAttribute attr = (BattleAttribute)obj;
                itemPool.AddItem().SetData(attr, info.GetBaseAttribute(attr));
            }
        }

        private void OnClose(GameObject go)
        {
            PopupManager.Instance.RemovePopup(this);
        }
        private void OnLeft(GameObject go)
        {
            index = index > 0 ? index - 1 : infoList.Count - 1;
            SetSingleData(infoList[index]);
        }
        private void OnRight(GameObject go)
        {
            index = index < infoList.Count - 1 ? index + 1 : 0;
            SetSingleData(infoList[index]);
        }
    }
}


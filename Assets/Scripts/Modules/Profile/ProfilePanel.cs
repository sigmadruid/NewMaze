using UnityEngine;
using UnityEngine.UI;

using System;

using Base;

namespace GameUI
{
    public class ProfilePanel : BasePopupView
    {
        public RectTransform AttributeItemPrefab;
        public GridLayoutGroup gridAttributes;

        void Awake()
        {
            for(int i = 0; i < 3; ++i)
            {
                GameObject attributeItem = Instantiate(AttributeItemPrefab.gameObject);
                attributeItem.transform.parent = gridAttributes.transform;
            }
        }

//        public override void OnInitialize()
//        {
//            base.OnInitialize();
//        }
//
//        public override void OnDispose()
//        {
//            base.OnDispose();
//        }
    }
}


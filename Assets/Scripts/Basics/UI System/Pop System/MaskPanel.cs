using UnityEngine;
using UnityEngine.UI;

using System;

namespace Base
{
    public class MaskPanel : BasePopupView
    {
        public Image imageBackground;

        void Awake()
        {
//            EventTriggerListener.Get(imageBackground.gameObject).onClick = OnBackgroundClick;
        }

        private void OnBackgroundClick(GameObject go)
        {
//            PopupManager.Instance.RemovePopup(this);
        }
    }
}


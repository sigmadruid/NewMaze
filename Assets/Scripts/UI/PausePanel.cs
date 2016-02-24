using UnityEngine;
using UnityEngine.UI;

using System;

using Base;
using GameLogic;

namespace GameUI
{
    public class PausePanel : BasePopupView
    {
        public Image imageBackground;

        void Awake()
        {
            EventTriggerListener.Get(imageBackground.gameObject).onClick = OnBackgroundClick;
        }

        private void OnBackgroundClick(GameObject go)
        {
            PopupManager.Instance.RemovePopup(this);
            Game.Instance.SetPause(false);
        }
    }
}


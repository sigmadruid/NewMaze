using UnityEngine;

using System;

using Base;
using GameLogic;

namespace GameUI
{
    public class PausePanel : BasePopupView
    {
        public UISprite spriteBackground;

        void Awake()
        {
            UIEventListener.Get(spriteBackground.gameObject).onClick = OnBackgroundClick;
        }

        private void OnBackgroundClick(GameObject go)
        {
            PopupManager.Instance.RemovePopup(this);
            Game.Instance.SetPause(false);
        }
    }
}


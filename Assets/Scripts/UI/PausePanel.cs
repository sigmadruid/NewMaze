using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections.Generic;

using Base;
using GameLogic;
using DG.Tweening;

namespace GameUI
{
    public class PausePanel : BasePopupView
    {
        public float TweenRadius = 100f;
        public float TweenDuration = 0.2f;

        public Action CallbackPackClick;
        public Action CallbackSoulAttachClick;
        public Action CallbackSettingsClick;

        public Button buttonPack;
        public Button buttonSoulAttach;
        public Button buttonSettings;
        public CanvasGroup groupButtonRoot;

        private List<Button> buttonList = new List<Button>();

        public override void OnInitialize()
        {
            base.OnInitialize();

            UILocalizer.LocalizeByName(transform);

            buttonList.Add(buttonPack);
            buttonList.Add(buttonSoulAttach);
            buttonList.Add(buttonSettings);

            ClickEventTrigger.Get(buttonPack.gameObject).onClick = OnPackClick;
            ClickEventTrigger.Get(buttonSoulAttach.gameObject).onClick = OnSoulAttachClick;
            ClickEventTrigger.Get(buttonSettings.gameObject).onClick = OnSettingsClick;
        }

        public override void OnDispose()
        {
            base.OnDispose();

            ClickEventTrigger.Get(buttonPack.gameObject).onClick = null;
            ClickEventTrigger.Get(buttonSoulAttach.gameObject).onClick = null;
            ClickEventTrigger.Get(buttonSettings.gameObject).onClick = null;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            for(int i = 0; i < buttonList.Count; ++i)
            {
                Button button = buttonList[i];
                button.transform.localPosition = Vector3.zero;
            }
            for(int i = 0; i < buttonList.Count; ++i)
            {
                float arc = 2 * i * Mathf.PI / buttonList.Count + Mathf.PI / 2;
                float x = TweenRadius * Mathf.Cos(arc);
                float y = TweenRadius * Mathf.Sin(arc);
                Button button = buttonList[i];
                button.transform.DOLocalMove(new Vector3(x, y, 0), TweenDuration);
            }
            groupButtonRoot.DOFade(1, TweenDuration);
        }

        private void OnPackClick(GameObject go)
        {
            CallbackPackClick();
        }
        private void OnSoulAttachClick(GameObject go)
        {
            CallbackSoulAttachClick();
        }
        private void OnSettingsClick(GameObject go)
        {
            CallbackSettingsClick();
        }
    }
}


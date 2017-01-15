using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;

namespace GameLogic
{
    public class EnvironmentMediator : Mediator
    {
		private float timer;
		private float duration;

		private EnvironmentScript script;
		private EnvironmentProxy proxy;

		public override void OnRegister ()
		{
			base.OnRegister ();
			proxy = ApplicationFacade.Instance.RetrieveProxy<EnvironmentProxy>();
		}

		public override IList<Enum> ListNotificationInterests ()
		{
			return new Enum[]
			{
                NotificationEnum.ENVIRONMENT_INIT,
                NotificationEnum.ENVIRONMENT_SHOW_MAZE_MAP,
			};
		}
		
		public override void HandleNotification (INotification notification)
		{
			switch((NotificationEnum)notification.NotifyEnum)
			{
				case NotificationEnum.ENVIRONMENT_INIT:
					HandleEnvironmentInit();
					break;
                case NotificationEnum.ENVIRONMENT_SHOW_MAZE_MAP:
                    bool show = (bool)notification.Body;
                    HandleEnvironmentShowMazeMap(show);
                    break;
			}
		}

		private void HandleEnvironmentInit()
		{
			script = GameObject.Find("Environment").GetComponent<EnvironmentScript>();
			script.CallbackUpdate = OnUpdate;
			script.MainLightScript.CallbackConvertingEnds = OnConvertingEnds;

			proxy.IsNight = false;
			timer = 0;
			duration = GlobalConfig.EnvironmentConfig.DayDuration;
		}
        private void HandleEnvironmentShowMazeMap(bool show)
        {
            script.MazeMapLight.enabled = show;
        }

		private void OnUpdate()
		{
			timer += Time.deltaTime;
			if (timer >= duration)
			{
				timer = 0;
				duration = proxy.IsNight ? GlobalConfig.EnvironmentConfig.DayDuration : GlobalConfig.EnvironmentConfig.NightDuration;
				script.MainLightScript.DayNightConvert(proxy.IsNight);
                string title = null;
                if(proxy.IsNight)
                    title = TextDataManager.Instance.GetData("common.daytitle");
                else
                    title = TextDataManager.Instance.GetData("common.nighttitle");
				TitlePanel.Show(title);
			}
		}

		private void OnConvertingEnds()
		{
			proxy.IsNight = !proxy.IsNight;
            DispatchNotification(NotificationEnum.ENVIRONMENT_DAYNIGHT_CHANGE, proxy.IsNight);
		}
    }
}


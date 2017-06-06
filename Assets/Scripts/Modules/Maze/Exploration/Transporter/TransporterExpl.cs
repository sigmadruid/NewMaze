using UnityEngine;

using System;

using Base;
using GameUI;
using StaticData;

namespace GameLogic
{
    public enum TransporterDirectionType
    {
        Forward,
        Back,
    }
    public class TransporterExpl : Exploration
    {
        public override void OnFunction()
        {
            base.OnFunction();

            string title = TextDataManager.Instance.GetData("common.transporter");
            string content = null;
            if (Hall.Instance == null)
            {
                content = TextDataManager.Instance.GetData("common.transporter.to");
                ConfirmPanel.Show(title, content, DoTransportForward, OnCancel);
            }
            else
            {
                content = TextDataManager.Instance.GetData("common.transporter.from");
                ConfirmPanel.Show(title, content, DoTransportBack, OnCancel);
            }
            Game.Instance.SetPause(true);
        }

        private void DoTransportForward()
        {
            BeforeTransport();
            TransitionPanel panel = PopupManager.Instance.CreateAndAddPopup<TransitionPanel>();
            panel.CallbackTransition = OnTransportForward;
            panel.Play();
        }
        private void DoTransportBack()
        {
            BeforeTransport();
            TransitionPanel panel = PopupManager.Instance.CreateAndAddPopup<TransitionPanel>();
            panel.CallbackTransition = OnTransportBack;
            panel.Play();
        }
        private void OnCancel()
        {
            Game.Instance.SetPause(false);
        }

        private void OnTransportForward()
        {
            ApplicationFacade.Instance.RetrieveProxy<AdamProxy>().IsInHall = true;
            int hallKid = int.Parse(Data.Param1);
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.BLOCK_DESPAWN);
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.HALL_INIT, hallKid);
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.HALL_SPAWN);
            Hall.Instance.LeavePosition = Adam.Instance.WorldPosition;
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.HERO_TRANSPORT, Hall.Instance.Script.EntryPos.position);
            AfterTransport();
        }
        private void OnTransportBack()
        {
            Vector3 leavePosition = Hall.Instance.LeavePosition;
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.HALL_DESPAWN);
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.HALL_DISPOSE);
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.BLOCK_SPAWN);
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.HERO_TRANSPORT, leavePosition);
            ApplicationFacade.Instance.RetrieveProxy<AdamProxy>().IsInHall = false;
            AfterTransport();
        }

        private void BeforeTransport()
        {
            InputManager.Instance.PreventMouseAction();
        }
        private void AfterTransport()
        {
            Game.Instance.SetPause(false);
        }

    }
}


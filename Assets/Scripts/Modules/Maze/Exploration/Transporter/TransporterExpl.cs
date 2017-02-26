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

            if (Hall.Instance == null)
            {
                ConfirmPanel.Show("Transporter", "Do you want to transport to somwhere unknown?", DoTransportForward, OnCancel);
            }
            else
            {
                ConfirmPanel.Show("Transporter", "Do you want to transport back to maze?", DoTransportBack, OnCancel);
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
            Adam.Instance.Info.IsInHall = true;
            int hallKid = int.Parse(Data.Param1);
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.BLOCK_HIDE_ALL);
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.HALL_INIT, hallKid);
            Hall.Instance.LeavePosition = Adam.Instance.WorldPosition;
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.HERO_TRANSPORT, Hall.Instance.Script.EntryPos.position);
            AfterTransport();
        }
        private void OnTransportBack()
        {
            Vector3 leavePosition = Hall.Instance.LeavePosition;
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.HALL_DISPOSE);
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.BLOCK_INIT);
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.BLOCK_REFRESH, leavePosition);
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.HERO_TRANSPORT, leavePosition);
            Adam.Instance.Info.IsInHall = false;
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


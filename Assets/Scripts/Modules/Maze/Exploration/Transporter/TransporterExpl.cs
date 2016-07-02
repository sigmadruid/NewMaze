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
        public TransporterDirectionType DirectionState;

        public override void OnFunction()
        {
            base.OnFunction();

            if(DirectionState == TransporterDirectionType.Forward)
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
            panel.StartTransition();
        }
        private void DoTransportBack()
        {
            BeforeTransport();
            TransitionPanel panel = PopupManager.Instance.CreateAndAddPopup<TransitionPanel>();
            panel.CallbackTransition = OnTransportBack;
            panel.StartTransition();
        }
        private void OnCancel()
        {
            Game.Instance.SetPause(false);
        }

        private void OnTransportForward()
        {
            Hero.Instance.IsSlowUpdating = false;
            Hero.Instance.IsInHall = true;
            HallProxy hallProxy = ApplicationFacade.Instance.RetrieveProxy<HallProxy>();
            hallProxy.LeavePosition = Hero.Instance.WorldPosition;
            int hallKid = int.Parse(Data.Param1);
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.HALL_INIT, hallKid);
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.HERO_TRANSPORT, hallProxy.CurrentHall.Script.EntryPos.position);
//            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.BLOCK_DISPOSE);
            AfterTransport();
        }
        private void OnTransportBack()
        {
            Hero.Instance.IsSlowUpdating = true;
            Hero.Instance.IsInHall = false;
            Vector3 leavePosition = ApplicationFacade.Instance.RetrieveProxy<HallProxy>().LeavePosition;
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.HALL_DISPOSE);
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.BLOCK_REFRESH, leavePosition);
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.HERO_TRANSPORT, leavePosition);
            AfterTransport();
        }

        private void BeforeTransport()
        {
        }
        private void AfterTransport()
        {
            Game.Instance.SetPause(false);
        }

        public static void Init(TransporterExpl expl, TransporterDirectionType directionType)
        {
            Exploration.Init(expl);
            expl.DirectionState = directionType;
        }
    }
}


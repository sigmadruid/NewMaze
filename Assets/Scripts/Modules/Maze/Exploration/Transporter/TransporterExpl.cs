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
                ConfirmPanel.Show("Transporter", "Do you want to transport to somwhere unknown?", DoTransportForward);
            }
            else
            {
                ConfirmPanel.Show("Transporter", "Do you want to transport back to maze?", DoTransportBack);

            }
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

        private void OnTransportForward()
        {
            Hero.Instance.IsSlowUpdating = false;
            ApplicationFacade.Instance.RetrieveProxy<HallProxy>().LeavePosition = Hero.Instance.WorldPosition;
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.HALL_INIT);
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.HERO_TRANSPORT, Hall.Instance.Script.EntryPos.position);
            AfterTransport();
        }
        private void OnTransportBack()
        {
            Hero.Instance.IsSlowUpdating = true;
            Vector3 leavePosition = ApplicationFacade.Instance.RetrieveProxy<HallProxy>().LeavePosition;
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.BLOCK_REFRESH, leavePosition);
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.HERO_TRANSPORT, leavePosition);
            AfterTransport();
        }

        private void BeforeTransport()
        {
            InputManager.Instance.Enable = false;
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.BATTLE_PAUSE, true);
        }
        private void AfterTransport()
        {
            InputManager.Instance.Enable = true;
            ApplicationFacade.Instance.DispatchNotification(NotificationEnum.BATTLE_PAUSE, false);
        }

        public static void Init(TransporterExpl expl, ExplorationType type, TransporterDirectionType directionType)
        {
            Exploration.Init(expl, type);
            expl.DirectionState = directionType;
        }
    }
}


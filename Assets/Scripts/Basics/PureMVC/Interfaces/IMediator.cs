using System;
using System.Collections.Generic;

namespace Base
{
    public interface IMediator
	{
        //property
		string MediatorName { get; }

        IList<Enum> ListNotificationInterests();
		void HandleNotification(INotification notification);

		void OnRegister();
        void RegisterInterestsUI();
        void RegisterInterestsMsg();
		void OnRemove();
	}
}

using System;

namespace Base
{
    public interface IView
	{
        //  Observer
        void RegisterObserver(Enum notificationName, IObserver observer);
        void RemoveObserver(Enum notificationName, object notifyContext);
		void NotifyObservers(INotification note);

		//  Mediator
		void RegisterMediator(IMediator mediator);
		IMediator RetrieveMediator(string mediatorName);
        IMediator RemoveMediator(string mediatorName);
		bool HasMediator(string mediatorName);
	}
}

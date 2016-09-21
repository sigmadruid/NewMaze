using System;

namespace Base
{
    public interface IFacade : INotifier
	{
        //  Proxy
		void RegisterProxy(IProxy proxy);
        T RetrieveProxy<T>() where T: IProxy;
        //IProxy RetrieveProxy(string proxyName);
        IProxy RemoveProxy(string proxyName);
		bool HasProxy(string proxyName);
        //  Mediator
		void RegisterMediator(IMediator mediator);
        T RetrieveMediator<T>() where T: IMediator;
        //IMediator RetrieveMediator(string mediatorName);
        IMediator RemoveMediator(string mediatorName);
		bool HasMediator(string mediatorName);
        //  Observer
		void NotifyObservers(INotification note);
	}
}

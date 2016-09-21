using System;
using System.Collections.Generic;

namespace Base
{
    public class Facade: IFacade
    {
        //  Variables
        protected IModel model = Model.Instance;
        protected IView view = View.Instance;
        protected static IFacade m_instance;
        //  property
        public View View { get { return view as View; } }
        public Model Model { get { return model as Model; } }

        //  Constructor
        protected Facade() { }
        static Facade() { }
        //  Instance
        public static IFacade Instance
        {
            get
            {
                if(m_instance == null)
                {
                    m_instance = new Facade();
                }
                return m_instance;
            }
        }
        public void Start()
        {
            InitializeModel();
            InitializeView();
        }
        protected virtual void InitializeModel() { }
        protected virtual void InitializeView() { }
        //  Facade Proxy Methods
        public virtual void RegisterProxy(IProxy proxy)
        {
            model.RegisterProxy(proxy);
        }
        public virtual T RetrieveProxy<T>() where T:IProxy
        {
            return (T)model.RetrieveProxy(typeof(T).FullName);
        }
        public virtual IProxy RetrieveProxy(Type type)
        {
            return model.RetrieveProxy(type.FullName);
        }
        public virtual IProxy RemoveProxy(string proxyName)
        {
            return model.RemoveProxy(proxyName);
        }
        public virtual bool HasProxy(string proxyName)
        {
            return model.HasProxy(proxyName);
        }
        //  Facade Mediator Methods
        public virtual void RegisterMediator(IMediator mediator)
        {
            view.RegisterMediator(mediator);
        }
        private IMediator RetrieveMediator(string mediatorName)
        {
            return view.RetrieveMediator(mediatorName);
        }
        public virtual T RetrieveMediator<T>() where T: IMediator
        {
            return (T)RetrieveMediator(typeof(T).FullName);
        }
        public virtual IMediator RetrieveMediator(Type type)
        {
            return view.RetrieveMediator(type.FullName);
        }
        public virtual IMediator RemoveMediator(string mediatorName)
        {
            return view.RemoveMediator(mediatorName);
        }
        public virtual bool HasMediator(string mediatorName)
        {
            return view.HasMediator(mediatorName);
        }
        //  NotifyObservers
        public virtual void NotifyObservers(INotification notification)
        {
            view.NotifyObservers(notification);
        }
        //  SendNotification
        public virtual void DispatchNotification(Enum notificationName)
        {
            NotifyObservers(new Notification(notificationName));
        }
        public virtual void DispatchNotification(Enum notificationName, object body)
        {
            NotifyObservers(new Notification(notificationName, body));
        }
        public virtual void DispatchNotification(Enum notificationName, object body, Enum enumValue)
        {
            NotifyObservers(new Notification(notificationName, body, enumValue));
        }

    }
}

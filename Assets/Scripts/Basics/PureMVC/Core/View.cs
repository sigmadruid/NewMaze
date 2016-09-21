using System;
using System.Linq;
using System.Collections.Generic;

namespace Base
{
    public class View: IView
    {
        //  Variables
        protected IDictionary<string, IMediator> m_mediatorMap = new Dictionary<string, IMediator>();
        protected IDictionary<Enum, IList<IObserver>> m_observerMap = new Dictionary<Enum, IList<IObserver>>();
        public static IView Instance { get; set; }
        //  Private constructor and static constructor
        private View() { }
        static View() { Instance = new View(); }
        //  Implements method
        public virtual void RegisterObserver(Enum notificationName, IObserver observer)
        {
            if(!m_observerMap.ContainsKey(notificationName))
            {
                m_observerMap[notificationName] = new List<IObserver>();
            }

            m_observerMap[notificationName].Add(observer);
        }
        public virtual void NotifyObservers(INotification notification)
        {
            IList<IObserver> observers = null;

            if(m_observerMap.ContainsKey(notification.NotifyEnum))
            {
                IList<IObserver> observers_ref = m_observerMap[notification.NotifyEnum];
                observers = new List<IObserver>(observers_ref);
            }
            if(observers != null)
            {
                for(int i = 0; i < observers.Count; i++)
                {
                    IObserver observer = observers[i];
                    observer.NotifyObserver(notification);
                }
            }
        }
        public virtual void RemoveObserver(Enum notificationName, object notifyContext)
        {
            if(m_observerMap.ContainsKey(notificationName))
            {
                IList<IObserver> observers = m_observerMap[notificationName];
                for(int i = 0; i < observers.Count; i++)
                {
                    if(observers[i].CompareNotifyContext(notifyContext))
                    {
                        observers.RemoveAt(i);
                        break;
                    }
                }
                if(observers.Count == 0)
                {
                    m_observerMap.Remove(notificationName);
                }
            }
        }
        //  Facade
        public virtual void RegisterMediator(IMediator mediator)
        {
            if(m_mediatorMap.ContainsKey(mediator.MediatorName))
            {
                return;
            }
            m_mediatorMap[mediator.MediatorName] = mediator;
            IList<Enum> interests = mediator.ListNotificationInterests();
            if(interests.Count > 0)
            {
                IObserver observer = new Observer("handleNotification", mediator);
                for(int i = 0; i < interests.Count; i++)
                {
                    RegisterObserver(interests[i], observer);
                }
            }
            mediator.OnRegister();
            mediator.RegisterInterestsUI();
            mediator.RegisterInterestsMsg();
        }
        public virtual IMediator RetrieveMediator(string mediatorName)
        {
            if(!m_mediatorMap.ContainsKey(mediatorName))
                return null;
            return m_mediatorMap[mediatorName];
        }
        public virtual IMediator RemoveMediator(string mediatorName)
        {
            IMediator mediator = null;

            // Retrieve the named mediator
            if(!m_mediatorMap.ContainsKey(mediatorName))
                return null;
            mediator = (IMediator)m_mediatorMap[mediatorName];

            // for every notification this mediator is interested in...
            IList<Enum> interests = mediator.ListNotificationInterests();

            for(int i = 0; i < interests.Count; i++)
            {
                // remove the observer linking the mediator 
                // to the notification interest
                RemoveObserver(interests[i], mediator);
            }

            // remove the mediator from the map		
            m_mediatorMap.Remove(mediatorName);

            // alert the mediator that it has been removed
            if(mediator != null)
                mediator.OnRemove();
            return mediator;
        }
        public virtual bool HasMediator(string mediatorName)
        {
            return m_mediatorMap.ContainsKey(mediatorName);
        }
        
        //  Log Or Check
        public bool IsRegisterMediator(Type type)
        {
            return m_mediatorMap.ContainsKey(type.FullName);
        }
    }
}

using System;
using System.Reflection;

namespace Base
{
    public class Observer: IObserver
    {
        public Observer(string notifyMethod, object notifyContext)
        {
            m_notifyMethod = notifyMethod;
            m_notifyContext = notifyContext;
        }

        public virtual void NotifyObserver(INotification notification)
        {
            object context = NotifyContext;
            string method = NotifyMethod;

            Type t = context.GetType();
            BindingFlags f = BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase;
            MethodInfo mi = t.GetMethod(method, f);
            mi.Invoke(context, new object[] { notification });
        }

        public virtual bool CompareNotifyContext(object obj)
        {
            return NotifyContext.Equals(obj);
        }

        public virtual string NotifyMethod
        {
            get
            {
                // Setting and getting of reference types is atomic, no need to lock here
                return m_notifyMethod;
            }
            set
            {
                // Setting and getting of reference types is atomic, no need to lock here
                m_notifyMethod = value;
            }
        }

        public virtual object NotifyContext
        {
            get
            {
                return m_notifyContext;
            }
            set
            {
                m_notifyContext = value;
            }
        }
        private string m_notifyMethod;
        private object m_notifyContext;
    }
}

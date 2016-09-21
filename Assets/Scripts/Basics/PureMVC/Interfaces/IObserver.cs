using System;
using System.Reflection;

namespace Base
{
    public interface IObserver
    {
        string NotifyMethod { set; get; }
        object NotifyContext { set; get; }
        void NotifyObserver(INotification notification);
		bool CompareNotifyContext(object obj);
    }
}

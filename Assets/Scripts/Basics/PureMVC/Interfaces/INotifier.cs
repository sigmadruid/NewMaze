using System;

namespace Base
{
    public interface INotifier
    {
		void DispatchNotification(Enum notificationName);
        void DispatchNotification(Enum notificationName, object body);
        void DispatchNotification(Enum notificationName, object body, Enum enumValue);
    }
}

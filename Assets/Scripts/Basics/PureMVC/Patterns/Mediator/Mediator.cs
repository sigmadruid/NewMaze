using System;
using System.Collections.Generic;

namespace Base
{
    public class Mediator: Notifier, IMediator, INotifier
    {
        public string MediatorName { get; set; }
        public Mediator()
        {
            MediatorName = GetType().FullName;
        }

        public virtual IList<Enum> ListNotificationInterests() { return new Enum[] { }; }
        public virtual void HandleNotification(INotification notification) { }
        public virtual void OnRegister() { }
        public virtual void RegisterInterestsUI() { }
        public virtual void RegisterInterestsMsg() { }
        public virtual void OnRemove() { }
    }
}

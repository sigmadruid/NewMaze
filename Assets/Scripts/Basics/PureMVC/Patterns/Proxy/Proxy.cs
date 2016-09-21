using System;

namespace Base
{
    public class Proxy: Notifier, IProxy, INotifier
    {
        public string ProxyName { get; set; }

        public Proxy()
        {
            ProxyName = GetType().FullName;
        }

        public virtual void OnRegister() { }
        public virtual void OnRemove() { }
    }
}
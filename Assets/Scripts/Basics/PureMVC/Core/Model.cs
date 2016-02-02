using System;
using System.Linq;
using System.Collections.Generic;

namespace Base
{
    public class Model: IModel
    {
        //  Variables
        protected IDictionary<string, IProxy> m_proxyMap = new Dictionary<string, IProxy>();
        public static IModel Instance { get; set; }
        //  Private constructor and static constructor
        private Model() { }
        static Model() { Instance = new Model(); }
        //  Used for Facade
        public virtual void RegisterProxy(IProxy proxy)
        {
            m_proxyMap[proxy.ProxyName] = proxy;
            proxy.OnRegister();
        }
        public virtual IProxy RetrieveProxy(string proxyName)
        {
            if(!m_proxyMap.ContainsKey(proxyName))
                return null;
            return m_proxyMap[proxyName];
        }
        public virtual bool HasProxy(string proxyName)
        {
            return m_proxyMap.ContainsKey(proxyName);
        }
        public virtual IProxy RemoveProxy(string proxyName)
        {
            IProxy proxy = null;

            if(m_proxyMap.ContainsKey(proxyName))
            {
                proxy = RetrieveProxy(proxyName);
                m_proxyMap.Remove(proxyName);
            }

            if(proxy != null)
                proxy.OnRemove();
            return proxy;
        }
        //  Check or Log
        public bool IsRegisterProxy(Type type)
        {
            return m_proxyMap.ContainsKey(type.FullName);
        }
    }
}

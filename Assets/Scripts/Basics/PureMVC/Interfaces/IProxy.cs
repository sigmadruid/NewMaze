using System;

namespace Base
{
    public interface IProxy
    {
		string ProxyName { get; }

		void OnRegister();
		void OnRemove();
    }
}

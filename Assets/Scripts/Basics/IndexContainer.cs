using System;
using System.Collections.Generic;

namespace Base
{
    public class IndexContainer<T>
    {
		private Dictionary<object, IndexContainer<T>> containerDic = new Dictionary<object, IndexContainer<T>>();
		private List<T> elementList = new List<T>();

        public IndexContainer()
        {
        }

		public void AddElement(T element, params object[] attrList)
		{

		}

		public T RetrieveElement(params object[] attrList)
		{
			return default(T);
		}

		public void RemoveElement(params object[] attrList)
		{
		}
    }
}


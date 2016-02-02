using System;
namespace Base
{
	public class LinkList
	{
		private LinkNode head = null;

		public void Add(object obj)
		{
			if (head == null)
			{
				head = new LinkNode();
				head.data = obj;
				head.next = null;
			}
			else
			{
				LinkNode node = new LinkNode();
				node.data = obj;
				node.next = head;
				head = node;
			}
		}

		public object Remove()
		{
			if (head == null)
			{
				return null;
			}
			else
			{
				LinkNode node = head;
				head = node.next;
				return node.data;
			}
		}

		public void PrintAll()
		{
			LinkNode node = head;
			while (node != null)
			{
				UnityEngine.Debug.Log(node.data);
				node = node.next;
			}
		}

		class LinkNode
		{
			public object data;
			public LinkNode next;
		}
	}
}


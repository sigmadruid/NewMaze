using System;

namespace Base
{
    public class Notification : INotification
	{
        public Notification(Enum name)         
            : this(name, null){ }
        public Notification(Enum name, object body)
            : this(name, body, null){ }
        public Notification(Enum name, object body, Enum enumValue)
		{
            NotifyEnum = name;
            Body = body;
            EnumValue = enumValue;
		}


        public virtual Enum NotifyEnum { get; set; }
        public virtual object Body { get; set; }
        public virtual Enum EnumValue { get; set; }
	}
}

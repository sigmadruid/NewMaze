using System;

namespace Base
{
    public interface INotification
    {
		Enum NotifyEnum { get; }
		object Body { get; set; }
		Enum EnumValue { get; set; }
        string ToString();
    }
}

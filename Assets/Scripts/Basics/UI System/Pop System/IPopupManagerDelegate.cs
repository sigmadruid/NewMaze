using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Base
{
	public interface IPopupManagerDelegate
	{
		T CreateAndAddPopup<T>()  where T : BasePopupView;

		void RemovePopup(BasePopupView view, bool cleanup = false);

	}
}


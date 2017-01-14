using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Base
{
	public struct AddMaskDTO
	{
		public int depth;
		
		public bool clickHide;

		public Transform popupRoot;
	}

	/// <summary>
	/// 添加遮罩行为.
	/// 参数：AddMaskDTO
	/// </summary>
	public class AddMaskPopupAction : BasePopupAction
	{
		private const int Z_MASK_INCREMENT = 5;

		private List<Transform> blackMaskStack;

		private BasePopupView currentPopup;

		public AddMaskPopupAction (IPopupManagerDelegate popupManagerDelegate) : base(PopupMode.ADD_MASK, popupManagerDelegate)
		{
			blackMaskStack = new List<Transform>();

			DefaultParam = false;
		}

		public override void ExecuteShow (BasePopupView view, object param = null)
		{
			if (param != null)
			{
				AddMaskDTO dto = (AddMaskDTO)param;
				
				currentPopup = view;
				
				Transform popParent = dto.popupRoot;
				
				Transform blackMaskTransform = ResourceManager.Instance.LoadGameObject(ObjectType.GameObject, PopupConst.MASK_PATH).transform;
				blackMaskTransform.parent = popParent;
				blackMaskTransform.localScale = Vector3.one;
				blackMaskTransform.localPosition = Vector3.zero;
				blackMaskStack.Add(blackMaskTransform);
//				blackMaskTransform.GetComponent<UIPanel>().depth = dto.depth - Z_MASK_INCREMENT;
				
				if (dto.clickHide)
                    EventTriggerListener.Get(blackMaskTransform.gameObject).onClick = onBlackMaskClick;
			}
		}

		public override void ExecuteHide (BasePopupView view, object param = null)
		{
			if (blackMaskStack.Count > 0)
			{
				int lastIndex = blackMaskStack.Count - 1;
				Transform blackMaskTransform = blackMaskStack[lastIndex];
                EventTriggerListener.Get(blackMaskTransform.gameObject).onClick = null;
				blackMaskStack.RemoveAt(lastIndex);
				ResourceManager.Instance.RecycleAsset(blackMaskTransform.gameObject);
			}
		}

		private void onBlackMaskClick(GameObject go)
		{
            EventTriggerListener.Get(go).onClick = null;
			popupManagerDelegate.RemovePopup(currentPopup);
		}
	}
}


using System;
using System.Collections;
using System.Collections.Generic;

namespace Base
{
	public struct PopupAnimationDTO
	{
		public static PopupAnimationDTO defaultDTO = new PopupAnimationDTO(PopupAnimationType.Floating, -1f);
		
		public PopupAnimationDTO(PopupAnimationType animationType, float duration)
		{
			this.animationType = animationType;
			this.duration = duration;
		}
		
		public PopupAnimationType animationType;
		
		public float duration;
	}

	public class PopupAnimationManager
	{
		private Dictionary<PopupAnimationType, Type> animationDic;

		private Dictionary<PopupAnimationType, PopupAnimationDTO> defaultDTODic;

		public PopupAnimationManager ()
		{
			animationDic = new Dictionary<PopupAnimationType, Type>()
			{
				{PopupAnimationType.Expand,  typeof(ExpandPopupAnimation)},
				{PopupAnimationType.Floating,  typeof(FloatingPopupAnimation)}
			};

			defaultDTODic = new Dictionary<PopupAnimationType, PopupAnimationDTO>()
			{
				{PopupAnimationType.Expand, new PopupAnimationDTO(PopupAnimationType.Expand, -1f)},
				{PopupAnimationType.Floating, new PopupAnimationDTO(PopupAnimationType.Floating, -1f)}
			};
		}

		public BasePopupAnimation CreateAnimation(PopupAnimationType animationType)
		{
			Type type = animationDic[animationType];
			return Activator.CreateInstance(type) as BasePopupAnimation;
		}

		public PopupAnimationDTO GetDefaultDTO(PopupAnimationType animationType)
		{
			return defaultDTODic.ContainsKey(animationType) ? defaultDTODic[animationType] : PopupAnimationDTO.defaultDTO;
		}

	}
}


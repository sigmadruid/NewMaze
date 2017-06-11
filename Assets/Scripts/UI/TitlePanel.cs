using UnityEngine;
using UnityEngine.UI;

using System;

using Base;
using DG.Tweening;

public class TitlePanel : BasePopupView
{
    public Text LabelTitle;
    public Image ImageDeco;

    private CanvasGroup group;

    void Awake()
    {
        group = GetComponent<CanvasGroup>();
    }

	public void Init(string title, float duration)
	{
        group.alpha = 0f;
		LabelTitle.text = title;

        Tweener tweener =  group.DOFade(1f, duration).SetLoops(2, LoopType.Yoyo);
        tweener.OnComplete(OnFinished);
	}

	private void OnFinished()
	{
        PopupManager.Instance.RemovePopup(this);
	}

	public static void Show(string title, float duration = 3f)
	{
        TitlePanel panel = PopupManager.Instance.CreateAndAddPopup<TitlePanel>(PopupMode.SHOW, PopupQueueMode.NoQueue);
		panel.Init(title, duration);
	}
}


using UnityEngine;
using UnityEngine.UI;

using System;

using Base;

public class TitlePanel : BasePopupView
{
    public Text LabelTitle;

    public Image ImageDeco;

	private float duration;
	private TweenAlpha tween;

	void Awake()
	{
		tween = GetComponent<TweenAlpha>();
		tween.AddOnFinished(OnFinished);
	}

	public void Init(string title, float duration)
	{
		this.duration = duration;
		LabelTitle.text = title;
        ImageDeco.SetWidth(LabelTitle.GetWidth() + 140f);
		tween.delay = 0;
		tween.PlayForward();
	}

	private void OnFinished()
	{
		tween.delay = duration;
		tween.PlayReverse();
	}

	public static void Show(string title, float duration = 3f)
	{
		TitlePanel panel = PopupManager.Instance.CreateAndAddPopup<TitlePanel>(PopupMode.SHOW, PopupQueueMode.NoQueue);
		panel.Init(title, duration);
	}
}


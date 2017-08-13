using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections.Generic;

using Base;
using DG.Tweening;

public class ItemToasterPanel : BasePopupView
{
    public class Info
    {
        public string title;
    }

    public static Queue<Info> infoQueue = new Queue<Info>();

    public float Duration = 3f;
    public Text LabelTitle;
    public Image ImageDeco;

    private bool isTweening;

    private CanvasGroup group;
    private Tweener tweener;

    private static ItemToasterPanel instance;

    void Awake()
    {
        instance = this;
        group = GetComponent<CanvasGroup>();
    }

    void OnDestroy()
    {
        instance = null;
    }

    public void Play()
    {
        if(!isTweening)
        {
            isTweening = true;

            Info info = infoQueue.Dequeue();
            LabelTitle.text = info.title;
            
            group.alpha = 0f;
            tweener =  group.DOFade(1f, Duration).SetLoops(2, LoopType.Yoyo);
            tweener.OnComplete(OnFinished);
        }
    }

	private void OnFinished()
	{
        isTweening = false;
        if(infoQueue.Count > 0)
        {
            Play();
        }
	}

	public static void Show(string title)
	{
        Info info = new Info();
        info.title = title;
        infoQueue.Enqueue(info);

        if (instance == null)
            instance = PopupManager.Instance.CreateAndAddPopup<ItemToasterPanel>();
        instance.Play();
	}
}


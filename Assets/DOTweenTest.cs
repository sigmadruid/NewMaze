using UnityEngine;
using System.Collections;

using DG.Tweening;

public class DOTweenTest : MonoBehaviour
{
    public Transform CubeTrans;

	void Start () 
    {
	}
	
	void Update () 
    {
        if(Input.GetMouseButtonDown(0))
        {
            Test();
        }
	}

    public void Test()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(CubeTrans.DOLocalMoveY(1, 1));
        sequence.Append(CubeTrans.DORotate(Vector3.right * 180, 0.5f));
        sequence.Append(CubeTrans.DOScale(Vector3.one * 2, 1f));

    }
}

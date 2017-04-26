using UnityEngine;
using UnityEngine.UI;

using System;

public class TransitionProgressBar : MonoBehaviour
{
    public Slider ForeBar;
    public Slider TransitionBar;
    public float TransitionDuration;

    private float timer;
    private float prevValue;

    void Update()
    {
        if(timer < TransitionDuration)
        {
            TransitionBar.value = ForeBar.value + (prevValue - ForeBar.value) * (1 - timer / TransitionDuration);
            timer += Time.deltaTime;
        }
    }

    public void SetValue(float targetValue, bool isAnim)
    {
        Debug.LogError(targetValue);
        prevValue = ForeBar.value;
        ForeBar.value = targetValue;
        if(isAnim)
        {
            timer = 0;
        }
        else
        {
            prevValue = targetValue;
            TransitionBar.value = targetValue;
        }
    }
}


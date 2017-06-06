using UnityEngine;
using UnityEngine.UI;

using System;

public class TransitionProgressBar : MonoBehaviour
{
    public Slider ForeBar;
    public Slider BackBar;
    public float TransitionDuration;

    private Slider instantBar;
    private Slider transitionBar;

    private float timer;
    private float prevValue;

    void Update()
    {
        if(timer < TransitionDuration)
        {
            transitionBar.value = instantBar.value + (prevValue - instantBar.value) * (1 - timer / TransitionDuration);
            timer += Time.deltaTime;
        }
    }

    public void SetValue(float targetValue, bool isAnim)
    {
        prevValue = ForeBar.value;
        instantBar = targetValue > prevValue ? BackBar : ForeBar;
        transitionBar = targetValue > prevValue ? ForeBar : BackBar;

        instantBar.value = targetValue;
        if(isAnim)
        {
            timer = 0;
        }
        else
        {
            prevValue = targetValue;
            transitionBar.value = targetValue;
        }
    }
}


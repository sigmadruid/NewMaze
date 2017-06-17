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
    private float nextValue;
    private float currentValue;
    private float prevValue;

    void Awake()
    {
        currentValue = 1f;
    }

    void Update()
    {
        if(timer > 0)
        {
            currentValue = prevValue + (nextValue - prevValue) * (1 - timer / TransitionDuration);
            transitionBar.value = currentValue;
            timer -= Time.deltaTime;
        }
    }

    public void SetValue(float targetValue, bool isAnim)
    {
        instantBar = targetValue > prevValue ? BackBar : ForeBar;
        transitionBar = targetValue > prevValue ? ForeBar : BackBar;

        if(isAnim)
        {
            timer = TransitionDuration;
            prevValue = currentValue;
            nextValue = targetValue;
            instantBar.value = targetValue;
        }
        else
        {
            timer = 0f;
            currentValue = targetValue;
            nextValue = targetValue;
            prevValue = targetValue;
            instantBar.value = targetValue;
            transitionBar.value = targetValue;
        }
    }
}


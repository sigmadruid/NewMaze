using UnityEngine;

using System;

using GameLogic;

public class PlotTrigger : MonoBehaviour
{
    public string PlotName;

    void OnTriggerEnter(Collider other)
    {
        Game.Instance.PlotRunner.Prepare(PlotName);
        Game.Instance.PlotRunner.Play(PlotName);
    }
}


﻿using UnityEngine;

using System;

using Base;
using GameLogic;

public class ItemScript : EntityScript
{
    private const float DURATION = 1f;

    private bool isFlying;

    private float t;
    private Vector3 startPostion;
    private Vector3 destPosition;
    private Vector3 midPosition;

    private Collider trigger;

    void Awake()
    {
        trigger = GetComponent<Collider>();
    }

    void Update()
    {
        if (isFlying)
        {
            t += Time.deltaTime;
            transform.position = BezierUtil.LerpPosition(startPostion, midPosition, destPosition, t / DURATION);
            if (Vector3.SqrMagnitude(transform.position - destPosition) < 0.01f)
            {
                isFlying = false;
                trigger.enabled = true;
            }
        }
    }

    public void Fly(Vector3 position)
    {
        Vector2 circle = UnityEngine.Random.insideUnitCircle;
        startPostion = position;
        destPosition = position + new Vector3(circle.x, 0, circle.y) * 1f + Vector3.up * 0.2f;
        midPosition = BezierUtil.GetMiddlePosition(position, destPosition, 8f);

        t = 0f;
        isFlying = true;
        trigger.enabled = false;
    }
}


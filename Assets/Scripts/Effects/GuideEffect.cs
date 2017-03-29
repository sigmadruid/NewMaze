using UnityEngine;

using System;

public class GuideEffect : MonoBehaviour
{
    public float hideDelay = 1f;

    private ParticleSystem[] particles;
    private float timer;

    void Awake()
    {
        particles = GetComponentsInChildren<ParticleSystem>();
    }

    public void StartEffect(Vector3 position)
    {
        gameObject.SetActive(true);
        transform.position = position;
        timer = 0;
        foreach(ParticleSystem particle in particles)
        {
            particle.time = 0;
        }
    }

    void Update()
    {
        if(timer > hideDelay)
        {
            gameObject.SetActive(false);
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
}


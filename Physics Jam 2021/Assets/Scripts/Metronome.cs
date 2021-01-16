using System;
using UnityEngine;

public class Metronome : MonoBehaviour
{
    public static Metronome Current { get; private set; }

    private const float bpm = 82;

    private const float tick = (bpm / 60) / 4;

    public event Action OnTick;

    private void Awake()
    {
        Current = this;

        InvokeRepeating(nameof(Tick), 0f, tick);
    }

    private void Tick()
    {
        OnTick?.Invoke();
    }
}
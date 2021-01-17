using System;
using UnityEngine;

public class Metronome : MonoBehaviour
{
    public static Metronome Current { get; private set; }

    public const float BPM = 82;

    private const float TickDelay = (BPM / 60) / 4f;

    public event Action OnTick;

    private void Awake()
    {
        Current = this;

        InvokeRepeating(nameof(Tick), 0f, TickDelay);
    }

    private void Tick()
    {
        OnTick?.Invoke();
    }
}
using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float MaxTime = 60f;
    public float TimeLeft { get; private set; }

    public bool IsRunning { get; private set; }

    public event Action OnTimeUp;

    void Start()
    {
        StartTimer();
    }

    public void StartTimer()
    {
        TimeLeft = MaxTime;
        IsRunning = true;
    }

    public void StopTimer()
    {
        IsRunning = false;
    }

    void Update()
    {
        if (!IsRunning) return;

        TimeLeft -= Time.deltaTime;

        if (TimeLeft <= 0f)
        {
            TimeLeft = 0f;
            IsRunning = false;
            OnTimeUp?.Invoke();
        }
    }

    public void AddTime(float value)
    {
        TimeLeft += value;
    }

    public void SubtractTime(float value)
    {
        TimeLeft -= value;
    }
}

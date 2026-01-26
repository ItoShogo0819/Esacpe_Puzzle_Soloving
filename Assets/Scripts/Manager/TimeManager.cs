using System;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    [Header("Time")]
    [SerializeField] private float _maxTime = 60f;
    public float MaxTime => _maxTime;

    public float TimeLeft { get; private set; }

    public bool IsRunning { get; private set; }

    [Header("UI")]
    [SerializeField] private TMP_Text _timeText;

    public event Action OnTimeUp;

    void Start()
    {
        ResetTimer();
    }

    void Update()
    {
        if (!IsRunning) return;

        TimeLeft -= Time.deltaTime;

        if (TimeLeft <= 0f)
        {
            TimeLeft = 0f;
            StopTimer();
            OnTimeUp?.Invoke();
        }

        UpdateText();
    }

    public void StartTimer()
    {
        IsRunning = true;
    }

    public void StopTimer()
    {
        IsRunning = false;
    }

    public void ResetTimer()
    {
        TimeLeft = _maxTime;
        IsRunning = false;
        UpdateText();
    }

    public void AddTime(float value)
    {
        TimeLeft = Mathf.Min(TimeLeft + value, _maxTime);
        UpdateText();
    }

    public void SubtractTime(float value)
    {
        TimeLeft = Mathf.Max(TimeLeft - value, 0f);
        UpdateText();

        if(TimeLeft <= 0f && IsRunning)
        {
            StopTimer();
            OnTimeUp?.Invoke();
        }
    }

    private void UpdateText()
    {
        if (_timeText == null) return;
        float displayTime = Mathf.Max(TimeLeft, 0f);
        _timeText.text = displayTime.ToString("F2");
    }
}

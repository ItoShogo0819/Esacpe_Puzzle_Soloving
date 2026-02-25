using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{
    private int _successCount = 0;
    private int _missCount = 0;

    [SerializeField] private TimeManager _timeManager;

    // スコアが変更されたことを通知するイベント
    public event Action<int, int> OnScoreChanged;

    // ミスの回数が限界を超えたことを通知するイベント
    public event Action OnMissLimitExeeded;

    public void ResetScore()
    {
        _successCount = 0;
        _missCount = 0;
        OnScoreChanged?.Invoke(_successCount, _missCount);
    }

    public void AddResult(bool isSuccess, Difficulty difficulty)
    {
        if (isSuccess)
        {
            _successCount++;
        }
        else
        {
            _missCount++;
            HandMiss(difficulty);
        }
        OnScoreChanged?.Invoke(_successCount, _missCount);
    }

    private void HandMiss(Difficulty difficulty)
    {
        if(difficulty == Difficulty.EX)
        {
            // EX難易度の場合、ミスしたときに時間を5秒減らす
            _timeManager.AddTime(-5f); 

            if(_missCount >= 3)
            {
                // ミスが3回以上になったら、ミス限界超過イベントを発火
                OnMissLimitExeeded?.Invoke();
            }
        }
    }
}

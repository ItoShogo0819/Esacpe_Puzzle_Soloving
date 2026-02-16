using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// ゲームの状態
/// 細部な挙動はFlagGameControllerで管理
/// </summary>
public class FlagGameController : MonoBehaviour
{
    public GameState State { get; private set; } = GameState.Start;

    [Header("腕データ")]
    public ArmData LeftArm;
    public ArmData RightArm;

    [Header("指示")]
    public ArmOrder InstructionLeft;
    public ArmOrder InstructionRight;

    [Header("タイミング")]
    public float InstructionInterval = 1.5f; // 指示更新間隔
    public float JudgeDelay = 0.5f;          // 猶予時間

    private int _successCount = 0;
    private int _missCount = 0;

    [Header("ゲーム管理")]
    public TimeManager TimeManager;

    [SerializeField] private ArmData _playerArms;

    public Difficulty CurrentDifficulty = Difficulty.Easy;
    public Difficult Easy;
    public Difficult Normal;
    public Difficult Hard;
    public Difficult EX;

    private Difficult _current;
    private float _timer;
    private float _elapsedTime;

    public event Action OnGameStart;
    public event Action OnGameOver;
    public event Action OnResult;
    public event Action OnRest;
    public event Action<int, int> OnScoreChanged;
    public event Action<ArmOrder, ArmOrder> OnInstructionGenerated;
    public event Action<bool, bool> OnJudgeResult; // 成功, 失敗

    void Start()
    {
        if (TimeManager == null)
        {
            Debug.LogError("TimeManagerが設定されていません。");
            enabled = false;
            return;
        }

        TimeManager.OnTimeUp += GameOver;
    }

    public void StartGame()
    {
        _successCount = 0;
        _missCount = 0;
        _timer = 0f;
        _elapsedTime = 0f;

        ApplyDifficulty();
        TimeManager.StartTimer();

        State = GameState.Playing;
        OnGameStart?.Invoke();

        Debug.Log("ゲームスタート！");
    }

    void Update()
    {
        if (State != GameState.Playing || _current == null) return;

        _elapsedTime += Time.deltaTime;
        _timer += Time.deltaTime;

        if (_timer >= InstructionInterval)
        {
            GenerateInstruction();
            _timer = 0f;
        }
    }

    public void SetDifficulty(Difficulty difficulty)
    {
        CurrentDifficulty = difficulty;
        ApplyDifficulty();
    }

    private void ApplyDifficulty()
    {
        _current = CurrentDifficulty switch
        {
            Difficulty.Easy => Easy,
            Difficulty.Normal => Normal,
            Difficulty.Hard => Hard,
            Difficulty.EX => EX,
            _ => Easy,
        };

        if (_current == null)
        {
            Debug.LogError("難易度設定が見つかりません。");
            enabled = false;
            return;
        }

        InstructionInterval = _current.InstructionInterval;
        JudgeDelay = _current.JudgeDelay;
    }

    private void GenerateInstruction()
    {
        InstructionLeft = RandomOrder();
        InstructionRight = RandomOrder();

        if (InstructionLeft == ArmOrder.None && InstructionRight == ArmOrder.None)
        {
            InstructionLeft = (ArmOrder)UnityEngine.Random.Range(1, 3);
            if (CurrentDifficulty == Difficulty.EX)
                InstructionRight = (ArmOrder)UnityEngine.Random.Range(1, 3);
        }

        OnInstructionGenerated?.Invoke(InstructionLeft, InstructionRight);

        StartCoroutine(JudgeAfterDelay());
    }

    private IEnumerator JudgeAfterDelay()
    {
        yield return new WaitForSeconds(JudgeDelay);
        JudgeOnce();
    }

    private bool CheckArm(ArmData armData, ArmOrder order, bool isLeft)
    {
        if (armData == null || armData.Chest == null) return false;

        Transform armTransform = isLeft ? armData.LeftArm : armData.RightArm;
        if (armTransform == null) return false;

        Vector3 dir = (armTransform.position - armData.Chest.position).normalized;

        bool isUp = dir.y > 0.5f;
        bool isDown = dir.y < -0.5f;

        return order switch
        {
            ArmOrder.None => !armData.HasMoved,
            ArmOrder.Up => isUp,
            ArmOrder.Down => isDown,
            _ => false,
        };
    }

    private void JudgeOnce()
    {
        bool leftCorrect = CheckArm(_playerArms, InstructionLeft, true);
        bool rightCorrect = CheckArm(_playerArms, InstructionRight, false);

        bool isSuccess = leftCorrect && rightCorrect;

        if (isSuccess)
        {
            _successCount++;
            Debug.Log("成功！");
        }
        else
        {
            _missCount++;
            Debug.Log("失敗！");
            if (CurrentDifficulty == Difficulty.EX)
            {
                TimeManager.SubtractTime(5f);
                if (_missCount >= 3) GameOver();
            }
        }

        OnJudgeResult?.Invoke(leftCorrect, rightCorrect);
        OnScoreChanged?.Invoke(_successCount, _missCount);

        // 判定後リセット
        LeftArm.HasMoved = false;
        RightArm.HasMoved = false;
    }

    private ArmOrder RandomOrder()
    {
        if (_elapsedTime < _current.WarmupTime)
            return ArmOrder.None;

        if (_current.AllowNoneAfterStart && UnityEngine.Random.value < _current.FeintRate)
            return ArmOrder.None;

        return (ArmOrder)UnityEngine.Random.Range(1, 3);
    }

    public void ResetGame()
    {
        TimeManager.StopTimer();
        TimeManager.ResetTimer();

        State = GameState.Start;

        _successCount = 0;
        _missCount = 0;
        OnScoreChanged?.Invoke(_successCount, _missCount);

        InstructionLeft = ArmOrder.None;
        InstructionRight = ArmOrder.None;

        _timer = 0f;
        _elapsedTime = 0f;

        OnRest?.Invoke();
        Debug.Log("ゲームリセット");
    }

    void GameOver()
    {
        if (State == GameState.GameOver || State == GameState.Result) return;

        TimeManager.StopTimer();

        InstructionLeft = ArmOrder.None;
        InstructionRight = ArmOrder.None;

        if (CurrentDifficulty == Difficulty.EX)
        {
            State = GameState.GameOver;
            OnGameOver?.Invoke();
            Debug.Log("ゲームオーバー！");
        }
        else
        {
            State = GameState.Result;
            OnResult?.Invoke();
            Debug.Log("リザルトへ！");
        }
    }

    private void OnDestroy()
    {
        if (TimeManager != null)
            TimeManager.OnTimeUp -= GameOver;
    }

    public float InstructionRemainTime => State == GameState.Playing && _current != null ? Mathf.Max(InstructionInterval - _timer, 0f) : 0f;
    public float InstructionRemain01 => State == GameState.Playing && _current != null ? Mathf.Clamp01((InstructionInterval - _timer) / InstructionInterval) : 0f;
}

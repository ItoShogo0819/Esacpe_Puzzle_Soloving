using System;
using System.Collections;
using UnityEngine;

public class FlagGameController : MonoBehaviour
{
    public GameState State { get; private set; } = GameState.Start;

    public ArmData LeftArm;
    public ArmData RightArm;

    public ArmOrder InstructionLeft;
    public ArmOrder InstructionRight;

    public float InstructionInterval = 1.5f; // 指示更新間隔
    public float JudgeDelay = 0.5f; // 猶予時間

    private int _successCount = 0;
    private int _missCount = 0;

    public TimeManager TimeManager;

    public Difficulty CurrentDifficulty = Difficulty.Easy;

    public Difficult Easy;
    public Difficult Normal;
    public Difficult Hard;
    public Difficult EX;

    private Difficult _current;
    //public float EasyWarmupTime = 2.0f; // イージーモードウォームアップ時間

    private float _timer;
    private float _elapsedTime;

    public event Action OnGameStart;
    public event Action OnGameOver;

    void Start()
    {
        if(TimeManager == null)
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
        if(State != GameState.Playing) return;

        _elapsedTime += Time.deltaTime;
        _timer += Time.deltaTime;

        if (_timer >= InstructionInterval)
        {
            GenerateInstruction();
            _timer = 0f;
        }
    }

    void GameOver()
    {
        if (State == GameState.GameOver) return;

        State = GameState.GameOver;

        Debug.Log("ゲームオーバー！");

        TimeManager.StopTimer();

        InstructionLeft = ArmOrder.None;
        InstructionRight = ArmOrder.None;

        OnGameOver?.Invoke();
        Debug.Log("ゲームオーバー");
    }

    public void SetDifficulty(Difficulty difficulty)
    {
        CurrentDifficulty = difficulty;
        //ApplyDifficulty();
    }

    void ApplyDifficulty()
    {
        _current = CurrentDifficulty switch
        {
            Difficulty.Easy => Easy,
            Difficulty.Normal => Normal,
            Difficulty.Hard => Hard,
            Difficulty.EX => EX,
            _ => Easy,
        };

        if(_current == null)
        {
            Debug.LogError("難易度設定が見つかりません。");
            enabled = false;
            return;
        }

        InstructionInterval = _current.InstructionInterval;
        JudgeDelay = _current.JudgeDelay;
    }

    // ランダム指示生成
    void GenerateInstruction()
    {
        InstructionLeft = RandomOrder();
        InstructionRight = RandomOrder();

        if(InstructionLeft == ArmOrder.None && InstructionRight == ArmOrder.None)
        {
            InstructionLeft = (ArmOrder)UnityEngine.Random.Range(1, 3);

            if(CurrentDifficulty == Difficulty.EX)
            {
                InstructionRight = (ArmOrder)UnityEngine.Random.Range(1, 3);
            }
        }

        Debug.Log($"新指示 → 左: {InstructionLeft}, 右: {InstructionRight}");

        StartCoroutine(JudgeAfterDelay());
    }
    
    IEnumerator JudgeAfterDelay()
    {
        yield return new WaitForSeconds(JudgeDelay);
        JudgeOnce();
    }

    void JudgeOnce()
    {
        bool leftCorrect = CheckArm(LeftArm, InstructionLeft);
        bool rightCorrect = CheckArm(RightArm, InstructionRight);

        if(leftCorrect && rightCorrect)
        {
            _successCount++;
            Debug.Log($"成功！ 合計成功回数: {_successCount}");
        }
        else
        {
            _missCount++;
            Debug.Log($"失敗！ 合計失敗回数: {_missCount}");

            if(CurrentDifficulty == Difficulty.EX)
            {
                TimeManager.SubtractTime(5f);
                Debug.Log($"ペナルティ！ 残り時間: {TimeManager.TimeLeft}秒");

                if(_missCount >= 3)
                {
                    GameOver();
                }
            }
        }

        LeftArm.HasMoved = false;
        RightArm.HasMoved = false;
    }

    ArmOrder RandomOrder()
    {
        if (_elapsedTime < _current.WarmupTime)
        {
            return ArmOrder.None; // ウォームアップ中は指示なし
        }

        if(_current.AllowNoneAfterStart && UnityEngine.Random.value < _current.FeintRate)
        {
            return ArmOrder.None;
        }

        return (ArmOrder)UnityEngine.Random.Range(1, 3);
    }

    // 判定
    bool CheckArm(ArmData arm, ArmOrder order)
    {
        bool isUp = Vector3.Dot(arm.Arm.up, Vector3.up) > 0.5f;

        return order switch
        {
            ArmOrder.None => !arm.HasMoved,
            ArmOrder.Up => isUp,
            ArmOrder.Down => !isUp,
            _ => false
        };
    }

    void OnDestroy()
    {
        if(TimeManager != null)
        {
            TimeManager.OnTimeUp -= GameOver;
        }
    }
}

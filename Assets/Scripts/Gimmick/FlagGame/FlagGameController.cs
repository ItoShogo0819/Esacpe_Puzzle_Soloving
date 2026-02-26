using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// ゲーム内の司令塔的な役割を担うクラス。
/// ゲームの状態管理、指示の生成、判定の実行、スコア管理などを統括する。
/// </summary>
public class FlagGameController : MonoBehaviour
{
    public GameState State { get; private set; } = GameState.Start;

    [Header("腕データ")]
    public ArmData LeftArm;
    public ArmData RightArm;

    [Header("指示")]
    public ArmOrder InstructionLeft => _currentInstruction.Left;
    public ArmOrder InstructionRight => _currentInstruction.Right;

    [Header("ゲーム管理")]
    public TimeManager TimeManager;
    [SerializeField] private ArmData _playerArms;
    [SerializeField] private FlagJudge _judge;
    [SerializeField] private FlagGenerator _generator;
    [SerializeField] private ScoreManager _scoreManager;

    private InstructionSet _currentInstruction;
    private float _timer;
    private float _elapsedTime;

    // 難易度設定
    public Difficulty CurrentDifficulty = Difficulty.Easy;
    [SerializeField] private DifficultyData _easyData;
    [SerializeField] private DifficultyData _normalData;
    [SerializeField] private DifficultyData _hardData;
    [SerializeField] private DifficultyData _exData;
    [SerializeField] private DifficultyData _currentSettings;

    // イベント
    public event Action OnGameStart;
    public event Action OnGameOver;
    public event Action OnResult;
    public event Action OnRest;
    public event Action<ArmOrder, ArmOrder> OnInstructionGenerated;
    public event Action<bool, bool> OnJudgeResult; // 成功, 失敗

    void Start()
    {
        if (TimeManager == null) return;
        TimeManager.OnTimeUp += GameOver;

        _scoreManager.OnMissLimitExeeded += GameOver;
    }

    public void StartGame()
    {
        _timer = 0f;
        _elapsedTime = 0f;
        _scoreManager.ResetScore();

        ApplyDifficulty();
        TimeManager.StartTimer();

        State = GameState.Playing;
        OnGameStart?.Invoke();

        Debug.Log("ゲームスタート！");
    }

    void Update()
    {
        if (State != GameState.Playing || _currentSettings == null) return;

        _elapsedTime += Time.deltaTime;
        _timer += Time.deltaTime;

        if (_timer >= _currentSettings.InstructionInterval)
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
        // 難易度に応じた設定を適用
        _currentSettings = CurrentDifficulty switch
        {
            Difficulty.Easy => _easyData,
            Difficulty.Normal => _normalData,
            Difficulty.Hard => _hardData,
            Difficulty.EX => _exData,
            _ => _easyData
        };

        // 設定が正しく適用されているか確認
        if (_currentSettings == null)
        {
            Debug.LogError($"{CurrentDifficulty} のデータが設定されていません。");
            return;
        }
    }

    private void GenerateInstruction()
    {
        // 現在の難易度と経過時間に基づき、新たな指示セットを生成
        _currentInstruction = _generator.Generate(_currentSettings, CurrentDifficulty, _elapsedTime);
        
        // 現在の指示セットを更新
        OnInstructionGenerated?.Invoke(_currentInstruction.Left, _currentInstruction.Right);

        StartCoroutine(JudgeAfterDelay(_currentSettings.JudgeDelay));
    }

    private IEnumerator JudgeAfterDelay(float delay)
    {
        Debug.Log($"判定待ち開始: {delay}秒後に判定");
        yield return new WaitForSeconds(delay);
        JudgeOnce();
    }

    private void JudgeOnce()
    {
        //　プレイヤーの腕の状態と現在の指示を比較して成功か失敗かを判断
        bool isSuccess = _judge.JudgeAll(_playerArms, _currentInstruction);

        // 結果をスコアマネージャーに通知してスコアを更新(ペナルティ判断もManagerで管理)
        _scoreManager.AddResult(isSuccess, CurrentDifficulty);

        // 成功と失敗の両方の状態を通知
        OnJudgeResult?.Invoke(isSuccess, !isSuccess);

        // 腕の状態をリセット
        LeftArm.HasMoved = false;   
        RightArm.HasMoved = false;
    }

    public void ResetGame()
    {
        TimeManager.StopTimer();
        TimeManager.ResetTimer();

        State = GameState.Start;

        _scoreManager.ResetScore();

        _currentInstruction = default;

        _timer = 0f;
        _elapsedTime = 0f;

        OnRest?.Invoke();
        Debug.Log("ゲームリセット");
    }

    void GameOver()
    {
        if (State == GameState.GameOver || State == GameState.Result) return;

        TimeManager.StopTimer();

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
    
    public float GetInstructionProgress()
    {
        // 計算できない場合は0を返す
        if (State != GameState.Playing || _currentSettings == null) return 0f;
        if (_currentSettings.InstructionInterval <= 0f) return 0f;

        // 残り時間を計算し割合を出す
        float remainingTime = _currentSettings.InstructionInterval - _timer;
        float progress = remainingTime / _currentSettings.InstructionInterval;

        return Mathf.Clamp01(progress);
    }
}

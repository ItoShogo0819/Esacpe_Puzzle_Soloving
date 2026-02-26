using UnityEngine;

[CreateAssetMenu(fileName = "NewDifficulty", menuName = "FlagGame/DifficultyData")]
public class DifficultyData : ScriptableObject      
{
    [Header("基本設定")]
    public float InstructionInterval = 1.5f; // 指示更新間隔
    public float JudgeDelay = 0.5f;          // 猶予時間
    public float WarmupTime = 3f;            // ウォームアップ時間

    [Header("特殊ルール")]
    public bool AllowNoneAfterStart = true; // ゲーム開始後にNoneを許可するか
    [Range(0, 1)] public float FeintRate = 0.2f; // フェイントの発生率（0-1）
}

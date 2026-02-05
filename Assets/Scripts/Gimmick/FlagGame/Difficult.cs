public enum Difficulty
{
    Easy,
    Normal,
    Hard,
    EX
}

[System.Serializable]
public class Difficult
{
    public float InstructionInterval;
    public float JudgeDelay;
    public float WarmupTime;
    public float FeintRate;
    public bool AllowNoneAfterStart;
}
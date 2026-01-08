using System.Collections;
using UnityEngine;

public class FlagGameController : MonoBehaviour
{
    public ArmData LeftArm;
    public ArmData RightArm;

    public ArmOrder InstructionLeft;
    public ArmOrder InstructionRight;

    public float InstructionInterval = 1.5f; // 指示更新間隔
    public float JudgeDelay = 0.5f; // 猶予時間

    public Difficulty CurrentDifficulty = Difficulty.Easy;

    public Difficult Easy;
    public Difficult Normal;
    public Difficult Hard;
    public Difficult EX;

    private Difficult _current;
    //public float EasyWarmupTime = 2.0f; // イージーモードウォームアップ時間

    private float _timer;
    private float _elapsedTime;

    //public static Difficult CurrentSetting;

    void Start()
    {
        ApplyDifficulty();
    }

    void Update()
    {
        _elapsedTime += Time.deltaTime;
        _timer += Time.deltaTime;

        if (_timer >= InstructionInterval)
        {
            GenerateInstruction();
            _timer = 0f;
        }
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

        _timer = 0f;
        _elapsedTime = 0f;
    }

    // ランダム指示生成
    void GenerateInstruction()
    {
        InstructionLeft = RandomOrder();
        InstructionRight = RandomOrder();

        if(InstructionLeft == ArmOrder.None && InstructionRight == ArmOrder.None)
        {
            InstructionLeft = (ArmOrder)Random.Range(1, 3);
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
        CheckArm(LeftArm, InstructionLeft, "左腕");
        CheckArm(RightArm, InstructionRight, "右腕");
    }

    ArmOrder RandomOrder()
    {
        if (_elapsedTime < _current.WarmupTime)
        {
            return ArmOrder.None; // ウォームアップ中は指示なし
        }

        if(_current.AllowNoneAfterStart && Random.value < _current.FeintRate)
        {
            return ArmOrder.None;
        }

        return (ArmOrder)Random.Range(1, 3);
    }

    // 判定
    void CheckArm(ArmData arm, ArmOrder order, string name)
    {
        bool isUp = IsArmUp(arm.Arm);

        switch (order)
        {
            case ArmOrder.None:
                // 指示なし → 常に正解扱い
                break;

            case ArmOrder.Up:
                if (isUp)
                    Debug.Log($"{name}：上げ 正解！");
                else
                    Debug.Log($"{name}：上げ ミス！");
                break;

            case ArmOrder.Down:
                if (!isUp)
                    Debug.Log($"{name}：下げ 正解！");
                else
                    Debug.Log($"{name}：下げ ミス！");
                break;
        }
    }

    bool IsArmUp(Transform arm)
    {
        // 腕の角度判定（必要なら調整）
        return Vector3.Dot(arm.up, Vector3.up) > 0.5f;
    }
}

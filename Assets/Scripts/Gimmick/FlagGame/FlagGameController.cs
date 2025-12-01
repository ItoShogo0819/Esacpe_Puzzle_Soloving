using UnityEngine;

public class FlagGameController : MonoBehaviour
{
    public ArmData LeftArm,RightArm;

    public ArmOrder InstructionLeft;
    public ArmOrder InstructionRight;

    public float instructionInterval = 2f; // 指示更新間隔
    private float _timer;

    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= instructionInterval)
        {
            GenerateInstruction();
            _timer = 0f;
        }

        CheckArm(LeftArm, InstructionLeft, "左腕");
        CheckArm(RightArm, InstructionRight, "右腕");
    }

    // ランダム指示生成
    void GenerateInstruction()
    {
        InstructionLeft = RandomOrder();
        InstructionRight = RandomOrder();

        Debug.Log($"新指示 → 左: {InstructionLeft}, 右: {InstructionRight}");
    }

    ArmOrder RandomOrder()
    {
        int r = Random.Range(0, 3); // 0:None 1:Up 2:Down
        return (ArmOrder)r;
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
        return arm.localEulerAngles.x < 300f && arm.localEulerAngles.x > 180f;
    }
}

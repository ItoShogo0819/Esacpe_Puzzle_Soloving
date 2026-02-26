using UnityEngine;

/// <summary>
/// 判定クラス。プレイヤーの腕の状態をチェックして、指定されたArmOrderに従って正しいかどうかを判断する。
/// </summary>
public class FlagJudge : MonoBehaviour
{
    public bool JudgeAll(ArmData playerArms, InstructionSet instruction)
    {
        bool leftCorrect = CheckArm(playerArms, instruction.Left, true);                       // 左腕の状態をチェック
        bool rightCorrect = CheckArm(playerArms, instruction.Right, false);                    // 右腕の状態をチェック
        return leftCorrect && rightCorrect;                                             // 左腕と右腕の両方が正しい状態であればtrueを返す
    }

    public bool CheckArm(ArmData armData, ArmOrder order, bool isLeft)
    {
        if (armData == null || armData.Chest == null) return false;                     // もしArmDataやChestがnullならfalseを返す

        Transform armTransform = isLeft ? armData.LeftArm : armData.RightArm;           // 左腕か右腕のTransformを取得
        if (armTransform == null) return false;                                         // もし腕のTransformがnullならfalseを返す

        Vector3 dir = (armTransform.position - armData.Chest.position).normalized;      // 腕の位置と胸の位置から腕の方向を計算して正規化

        bool isUp = dir.y > 0.5f;                                                       // 腕の方向のy成分が0.5より大きければ上を向いていると判断
        bool isDown = dir.y < -0.5f;                                                    // 腕の方向のy成分が-0.5より小さければ下を向いていると判断

        return order switch                                                             // ArmOrderに応じて正しい腕の状態を判断
        {
            ArmOrder.None => !armData.HasMoved,                                         // ArmOrder.Noneの場合は腕が動いていないことを確認
            ArmOrder.Up => isUp,                                                        // ArmOrder.Upの場合は腕が上を向いていることを確認
            ArmOrder.Down => isDown,                                                    // ArmOrder.Downの場合は腕が下を向いていることを確認
            _ => false,                                                                 // その他の値の場合はfalseを返す
        };
    }

    public bool JudgeOnce(ArmData playerArms, ArmOrder leftOrder, ArmOrder rightOrder)  // プレイヤーの腕の状態と左腕・右腕の期待される状態を引数に取る
    {
        bool leftCorrect = CheckArm(playerArms, leftOrder, true);                       // 左腕の状態をチェック
        bool rightCorrect = CheckArm(playerArms, rightOrder, false);                    // 右腕の状態をチェック
        return leftCorrect && rightCorrect;                                             // 左腕と右腕の両方が正しい状態であればtrueを返す
    }
}

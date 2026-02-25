using UnityEngine;

public class FlagGenerator : MonoBehaviour
{
    /// <summary>
    /// 現在の難易度と経過時間に基づき、新たな指示セットを生成する。
    /// </summary>
    public InstructionSet Generate(DifficultyData settings, Difficulty difficulty, float elapsedTime)
    {
        InstructionSet result;

        // 左右の指示をランダムに生成
        result.Left = RandomOrder(settings, elapsedTime);
        result.Right = RandomOrder(settings, elapsedTime);

        //　両方が"None"の場合、ゲームが止まるのを防ぐため、片方をランダムに"Up"か"Down"にする
        if (result.Left == ArmOrder.None && result.Right == ArmOrder.None)
        {
            result.Left = (ArmOrder)Random.Range(1, 3);

            // EX難易度のみ、両方同時の指示を許可する
            if (difficulty == Difficulty.EX)
            {
                result.Right = (ArmOrder)Random.Range(1, 3);
            }
        }
        return result;
    }

    private ArmOrder RandomOrder(DifficultyData settings, float elapsedTime)
    {
        // ゲーム開始から一定時間は"None"のみを生成する(指示を出さない)
        if (elapsedTime < settings.WarmupTime)
        {
            return ArmOrder.None;
        }
        // ゲーム開始後は、設定に応じて一定確率で"None"を生成する(フェイント)
        if (settings.AllowNoneAfterStart && Random.value < settings.FeintRate)
        {
            return ArmOrder.None;
        }

        // "Up"か"Down"をランダムに生成する
        return (ArmOrder)Random.Range(1, 3);
    }
}

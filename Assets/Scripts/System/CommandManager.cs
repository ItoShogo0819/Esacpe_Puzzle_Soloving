using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public List<Command> Commands;

    void Update()
    {
        foreach (var cmd in Commands)
        {
            if (CheckCommand(cmd))
            {
                Debug.Log($"コマンド成功：{cmd.name}");
            }
        }
    }

    bool CheckCommand(Command cmd)
    {
        var buffer = InputBuffer.Instance.buffer;
        int seqIndex = cmd.Sequence.Length - 1;

        // 新しい記録から走査
        for (int i = buffer.Count - 1; i >= 0; i--)
        {
            Vector2 input = buffer[i].Dir;

            // 方向が近いなら一致として扱う
            if (Vector2.Distance(input.normalized, cmd.Sequence[seqIndex]) < cmd.Lenience)
            {
                seqIndex--;

                if (seqIndex < 0)
                {
                    return true; // 全てマッチ
                }
            }
        }
        return false;
    }
}

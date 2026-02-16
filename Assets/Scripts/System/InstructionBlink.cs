using UnityEngine;
using UnityEngine.UI;

public class InstructionBlink : MonoBehaviour
{
    [Header("UI Image 配列")]
    [SerializeField] private Image[] _leftImages;  // 0:None, 1:Up, 2:Down
    [SerializeField] private Image[] _rightImages; // 0:None, 1:Up, 2:Down

    [Header("予告開始割合")]
    [SerializeField] private float _preStart01 = 0.25f; // 残り25%で開始

    [Header("脈打ち設定")]
    [SerializeField] private float _pulseSpeed = 10f;
    [SerializeField] private float _pulseAmount = 0.08f;

    [Header("ゲームコントローラー")]
    [SerializeField] private FlagGameController _cont;

    void Update()
    {
        if (_cont == null) return;
        if (_cont.State != GameState.Playing) return;

        float remain01 = _cont.InstructionRemain01;

        UpdateSide(_leftImages, _cont.InstructionLeft, remain01);
        UpdateSide(_rightImages, _cont.InstructionRight, remain01);
    }

    private void UpdateSide(Image[] images, ArmOrder current, float remain01)
    {
        // 予告進行度（0 → 1）
        float t = 0f;

        if (remain01 <= _preStart01)
        {
            t = Mathf.InverseLerp(_preStart01, 0f, remain01);
        }

        for (int i = 0; i < images.Length; i++)
        {
            bool active = (i == (int)current);
            images[i].enabled = active;

            if (!active) continue;

            // ===== 白化 =====
            images[i].color = Color.Lerp(Color.white, Color.white, 1f); // 念のため初期化
            images[i].color = Color.Lerp(Color.white * 0.7f, Color.white, t);

            // ===== 脈打ち =====
            float pulse = 1f + Mathf.Sin(Time.time * _pulseSpeed) * _pulseAmount * t;
            images[i].transform.localScale = Vector3.one * pulse;
        }
    }
}
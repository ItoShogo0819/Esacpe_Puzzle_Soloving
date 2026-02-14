using UnityEngine;
using UnityEngine.UI;

public class InstructionBlink : MonoBehaviour
{
    [Header("UI Image 配列")]
    [SerializeField] private Image[] _leftImages;  // 0:None, 1:Up, 2:Down
    [SerializeField] private Image[] _rightImages; // 0:None, 1:Up, 2:Down

    [Header("点滅設定")]
    [SerializeField] private float _blinkStart01 = 0.25f;    // 残り25%で点滅開始
    [SerializeField] private float _blinkInterval = 0.08f;   // 点滅周期
    [SerializeField] private float _blinkDuration = 0.45f;   // 点滅持続時間（秒）

    [Header("ゲームコントローラー")]
    [SerializeField] private FlagGameController _cont;

    private float _blinkTimer;
    private float _blinkTotal;
    private bool _blinkState = true;
    private bool _blinkActive = false;

    void Update()
    {
        if (_cont == null) return;
        if (_cont.State != GameState.Playing) return;

        float remain = _cont.InstructionRemain01;
        bool shouldBlink = remain <= _blinkStart01;

        if (shouldBlink)
        {
            if (!_blinkActive)
            {
                _blinkActive = true;
                _blinkTimer = 0f;
                _blinkTotal = 0f;
                _blinkState = true;
            }

            _blinkTimer += Time.deltaTime;
            _blinkTotal += Time.deltaTime;

            if (_blinkTimer >= _blinkInterval)
            {
                _blinkState = !_blinkState;
                _blinkTimer = 0f;
            }

            if (_blinkTotal >= _blinkDuration)
            {
                _blinkActive = false;
                _blinkState = true; // 点滅終了
            }
        }
        else
        {
            _blinkState = true;
            _blinkActive = false;
            _blinkTimer = 0f;
            _blinkTotal = 0f;
        }

        UpdateSide(_leftImages, _cont.InstructionLeft);
        UpdateSide(_rightImages, _cont.InstructionRight);
    }

    private void UpdateSide(Image[] images, ArmOrder current)
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].enabled = (i == (int)current) && _blinkState;
        }
    }
}

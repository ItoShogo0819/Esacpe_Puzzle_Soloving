using UnityEngine;
using UnityEngine.UI;

public class InstractionGage : MonoBehaviour
{
    [SerializeField] private Image _fill;
    [SerializeField] private FlagGameController _gameController;

    [Header("ââèo")]
    [SerializeField] private float _line = 0.3f;
    void Update()
    {
        if (_gameController == null) return;

        float value = _gameController.InstructionRemain01;
        _fill.fillAmount = value;

        _fill.color = value <= _line ? Color.red : Color.white;
    }
}

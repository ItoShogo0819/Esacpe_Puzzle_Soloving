using UnityEngine;
using UnityEngine.UI;

public class InstructionUI : MonoBehaviour
{
    [SerializeField] private FlagGameController _controller;

    [Header("Left")]
    [SerializeField] private Image _leftUp;
    [SerializeField] private Image _leftDown;

    [Header("Right")]
    [SerializeField] private Image _rightUp;
    [SerializeField] private Image _rightDown;

    void Update()
    {
        if (_controller.State != GameState.Playing)
        {
            HideAll();
            return;
        }

        UpdateArm(_controller.InstructionLeft, _leftUp, _leftDown);
        UpdateArm(_controller.InstructionRight, _rightUp, _rightDown);
    }

    private void UpdateArm(ArmOrder order, Image up, Image down)
    {
        up.enabled = order == ArmOrder.Up;
        down.enabled = order == ArmOrder.Down;
    }

    private void HideAll()
    {
        _leftUp.enabled = false;
        _leftDown.enabled = false;
        _rightUp.enabled = false;
        _rightDown.enabled = false;
    }
}

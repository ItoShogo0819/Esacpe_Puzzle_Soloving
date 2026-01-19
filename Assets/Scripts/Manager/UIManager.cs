using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _uiScreens;
    [SerializeField] private FlagGameController _gameController;

    public void OnAnyButtonPressed(Difficulty difficulty)
    {
        HideAllUI();
        _gameController.SetDifficulty(difficulty);
        _gameController.StartGame();
    }

    public void ShowUI(GameObject ui)
    {
        HideAllUI();
        ui.SetActive(true);
    }

    public void HideAllUI()
    {
        foreach(var ui in _uiScreens)
        {
            ui.SetActive(false);
        }
    }
}

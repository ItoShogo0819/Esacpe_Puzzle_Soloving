using UnityEngine;

public class SelectDifficulty : MonoBehaviour
{
    [SerializeField] private FlagGameController _gameController;

    public void StartEasy() => StartGame(Difficulty.Easy);
    public void StartNormal() => StartGame(Difficulty.Normal);
    public void StartHard() => StartGame(Difficulty.Hard);
    public void StartEX() => StartGame(Difficulty.EX);

    public void StartGame(Difficulty difficulty)
    {
        _gameController.SetDifficulty(difficulty);
        _gameController.StartGame();
    }
}

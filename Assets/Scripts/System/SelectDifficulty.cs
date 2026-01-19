using UnityEngine;

public class SelectDifficulty : MonoBehaviour
{
    [SerializeField] private FlagGameController _gameController;

    
    public void StartGame(Difficulty difficulty)
    {
        _gameController.SetDifficulty(difficulty);
        _gameController.StartGame();
    }
}

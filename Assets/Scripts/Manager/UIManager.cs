using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UIパネル")]
    public GameObject TitleUI;
    public GameObject InGameUI;
    public GameObject GameOverUI;
    public GameObject ResultUI;

    [Header("ゲームコントローラー")]
    [SerializeField] private FlagGameController _gameController;

    private void Start()
    {
        if(_gameController != null)
        {
            _gameController.OnGameStart += ShowInGameUI;
            _gameController.OnGameOver += ShowGameOverUI;
            _gameController.OnResult += ShowResultUI;
        }

        UpdateUI(GameState.Start);
    }

    private void OnDestroy()
    {
        if (_gameController != null)
        {
            _gameController.OnGameStart -= ShowInGameUI;
            _gameController.OnGameOver -= ShowGameOverUI;
            _gameController.OnResult -= ShowResultUI;
        }
    }

    public void StartGameButton(Difficulty difficulty)
    {
        _gameController.SetDifficulty(difficulty);
        _gameController.StartGame();
    }

    private void ShowInGameUI()
    {
        if (_gameController.State != GameState.Playing) return;
        UpdateUI(GameState.Playing);
    }

    private void ShowGameOverUI()
    {
        if (_gameController.State != GameState.GameOver) return;
        UpdateUI(GameState.GameOver);
    }

    private void ShowResultUI()
    {
        if(_gameController.State != GameState.Result) return;
        UpdateUI(GameState.Result);
    }

    public void ReturnToTitleButton()
    {
        _gameController.ResetGame();
        UpdateUI(GameState.Start);
    }

    private void UpdateUI(GameState state)
    {
        TitleUI.SetActive(state == GameState.Start);
        InGameUI.SetActive(state == GameState.Playing);
        GameOverUI.SetActive(state == GameState.GameOver);
        ResultUI.SetActive(state == GameState.Result);
    }
}

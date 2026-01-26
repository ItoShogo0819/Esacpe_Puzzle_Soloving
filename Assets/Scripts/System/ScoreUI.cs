using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private FlagGameController _game;
    [SerializeField] private TMP_Text _successText;
    [SerializeField] private TMP_Text _missText;

    void Start()
    {
        if(_game != null)
            _game.OnScoreChanged += UpdateScore;
    }

    void UpdateScore(int success, int miss)
    {
        _successText.text = $"Success: {success}";
        _missText.text = $"Miss: {miss}";
    }

    void OnDestroy()
    {
        if(_game != null)
            _game.OnScoreChanged -= UpdateScore;
    }
}

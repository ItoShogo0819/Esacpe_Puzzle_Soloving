using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private TMP_Text _successText;
    [SerializeField] private TMP_Text _missText;

    void Start()
    {
        if(_scoreManager != null)
        {
            _scoreManager.OnScoreChanged += UpdateScore;
        }
    }

    void UpdateScore(int success, int miss)
    {
        _successText.text = $"Success: {success}";
        _missText.text = $"Miss: {miss}";
    }

    void OnDestroy()
    {
        if(_scoreManager != null)
        {
            _scoreManager.OnScoreChanged -= UpdateScore;
        }
    }
}

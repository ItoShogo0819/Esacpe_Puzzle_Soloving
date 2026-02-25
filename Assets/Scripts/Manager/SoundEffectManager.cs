using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    [Header("SE")]
    [SerializeField] private AudioClip _successSE;
    [SerializeField] private AudioClip _missSE;

    [SerializeField] private ScoreManager _scoreManager;

    private AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (_scoreManager != null)
        {
            _scoreManager.OnScoreChanged += OnScoreChanged;
        }
    }

    void OnDestroy()
    {
        if (_scoreManager != null)
        {
            _scoreManager.OnScoreChanged -= OnScoreChanged;
        }
    }

    private int _lastSuccess = 0;
    private int _lastMiss = 0;

    private void OnScoreChanged(int success, int miss)
    {
        if (success > _lastSuccess)
        {
            _audioSource.PlayOneShot(_successSE);
        }
        else if (miss > _lastMiss)
        {
            _audioSource.PlayOneShot(_missSE);
        }

        _lastSuccess = success;
        _lastMiss = miss;
    }
}

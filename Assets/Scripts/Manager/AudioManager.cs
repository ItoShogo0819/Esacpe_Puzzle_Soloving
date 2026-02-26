using UnityEngine;

/// <summary>
/// BGMとSEを管理するクラス
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [Header("BGM")]
    [SerializeField] private AudioClip _titleBGM;
    [SerializeField] private AudioClip _playBGM;
    [SerializeField] private AudioClip _resultBGM;
    [SerializeField] private AudioClip _gameOverBGM;

    [Header("SE")]
    [SerializeField] private AudioClip _successSE;
    [SerializeField] private AudioClip _missSE;

    [Header("Reference")]
    [SerializeField] private FlagGameController _flagGameController;
    [SerializeField] private ScoreManager _scoreManager;

    private AudioSource _bgmSource;
    private AudioSource _seSource;
    private GameState _gameState;
    private int _lastSuccess;
    private int _lastMiss;

    void Awake()
    {
        // AudioSourceをすべて取得
        AudioSource[] sources = GetComponents<AudioSource>();

        // AudioSourceが2つ未満の場合は追加
        if (sources.Length < 2)
        {
            gameObject.AddComponent<AudioSource>();
            sources = GetComponents<AudioSource>();
        }

        _bgmSource = sources[0];
        _seSource = sources[1];

        _bgmSource.loop = true;
    }

    void Start()
    {
        if(_scoreManager != null)
        {
            _scoreManager.OnScoreChanged += OnScoreChanged;
        }

        if(_flagGameController != null)
        {
             ChangeBGM(_gameState);
        }
    }

    void Update()
    {
        // ゲームの状態が変化したらBGMを切り替え
        if(_flagGameController != null && _flagGameController.State != _gameState)
        {
            _gameState = _flagGameController.State;
            ChangeBGM(_gameState);
        }
    }

    private void OnScoreChanged(int success, int miss)
    {
        // 成功数が前回より増えたら成功SEを、ミス数が前回より増えたらミスSEを再生
        if (success > _lastSuccess)
        {
            _seSource.PlayOneShot(_successSE);
        }
        else if (miss > _lastMiss)
        {
            _seSource.PlayOneShot(_missSE);
        }
        _lastSuccess = success;
        _lastMiss = miss;
    }

    private void ChangeBGM(GameState state)
    {
        _bgmSource.Stop();

        // ゲームの状態に応じてBGMを切り替える
        _bgmSource.clip = state switch
        {
            GameState.Start => _titleBGM,
            GameState.Playing => _playBGM,
            GameState.GameOver => _gameOverBGM,
            GameState.Result => _resultBGM,
            _ => null
        };

        if (_bgmSource.clip != null)
        {
            _bgmSource.Play();
        }
    }
}

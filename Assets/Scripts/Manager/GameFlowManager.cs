using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    public GameSelector Selector;
    private Queue<string> _gameQueue;

    public bool LastResault { get; private set; }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        StartNewCycle();
    }

    public void StartNewCycle()
    {
        _gameQueue = new Queue<string>(Selector.CreateNewCycle());
        LoadNextGame();
    }

    public void OnMiniGameFinished(bool success)
    {
        LastResault = success;
        SceneManager.LoadScene("Transition", LoadSceneMode.Single);
    }

    void LoadNextGame()
    {
        if (_gameQueue.Count == 0)
        {
            StartNewCycle();
            return;
        }
        string nextScene = _gameQueue.Dequeue();
        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }
}

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameOverState {
    Win,
    Lose
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _startingCoins = 500;
    [SerializeField] private int _goalExperience = 500;
    public int GoalExperience => _goalExperience;
    private readonly PlayerResources _playerResources = new();

    public static GameManager Instance;
    public static event Action<GameOverState> OnGameOver;

    public PlayerResources PlayerResources => _playerResources;

    private void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        PlayerResources.OnExperienceChanged += CheckWinningCondition;

        //Load UI scene, quick&dirty, for demonstration only
        SceneManager.LoadScene(1, LoadSceneMode.Additive); 
    }

    private void CheckWinningCondition(PlayerResources _playerResources, int currentExperience, int _change) {
        if(currentExperience >= _goalExperience) {
            OnGameOver?.Invoke(GameOverState.Win);
        }
    }

    public void TriggerGameOver()
    {
        OnGameOver?.Invoke(GameOverState.Lose);
    }
    private void Start() {
        _playerResources.GainCoins(_startingCoins);
    }
}


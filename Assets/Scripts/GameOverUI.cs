using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameLostText;
    [SerializeField] private TextMeshProUGUI gameWonText;
    [SerializeField] private CanvasGroup canvasGroup;

    void Awake()
    {
        GameManager.OnGameOver += ShowGameOverUI;
    }

    void OnDestroy()
    {
        GameManager.OnGameOver -= ShowGameOverUI;       
    }

    private void ShowGameOverUI(GameOverState state)
    {
        GameManager.OnGameOver -= ShowGameOverUI;
        canvasGroup.blocksRaycasts = true;
        if(state == GameOverState.Win)
        {
            canvasGroup.DOFade(1, 2f).OnComplete(() =>
            {
                gameWonText.DOFade(1, 2f);
            });
        }
        else
        {
            canvasGroup.DOFade(1, 2f).OnComplete(() =>
            {
                gameLostText.DOFade(1, 2f);
            });
        }
    }
}

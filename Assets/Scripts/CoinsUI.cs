using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CoinsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private int currentCoinCount = 0;

    private Tween counterTween;

    void Awake()
    {
        PlayerResources.OnCoinsChanged += UpdateCoinCounter;
    }

    void OnDestroy()
    {
        PlayerResources.OnCoinsChanged -= UpdateCoinCounter;
    }

    private void UpdateCoinCounter(PlayerResources resources, int current, int change)
    {
        counterTween?.Kill();

        int startValue = current - change;

        // If no animation needed
        if (change == 0)
        {
            currentCoinCount = current;
            text.text = currentCoinCount.ToString();
            return;
        }

        float duration = 2f;

        string sign = change > 0 ? "+" : "-";
        int absChange = Mathf.Abs(change);

        counterTween = DOVirtual.Int(startValue, current, duration, value =>
        {
            currentCoinCount = value;
            text.text = $"{currentCoinCount} {sign}{absChange}";
        })
        .SetEase(Ease.OutCubic)
        .OnComplete(() =>
        {
            currentCoinCount = current;
            text.text = currentCoinCount.ToString();
        });
    }
}
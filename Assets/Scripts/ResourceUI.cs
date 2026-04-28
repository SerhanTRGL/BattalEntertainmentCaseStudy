using DG.Tweening;
using TMPro;
using UnityEngine;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private int currentResourceCount = 0;

    private Tween counterTween;

    void Awake()
    {
        PlayerResources.OnResourceChanged += UpdateResourceCounter;
    }

    void OnDestroy()
    {
        PlayerResources.OnResourceChanged -= UpdateResourceCounter;
    }

    private void UpdateResourceCounter(PlayerResources resources, int current, int change)
    {
        counterTween?.Kill();

        int startValue = current - change;

        // If no animation needed
        if (change == 0)
        {
            currentResourceCount = current;
            text.text = currentResourceCount.ToString();
            return;
        }

        float duration = 2f;

        string sign = change > 0 ? "+" : "-";
        int absChange = Mathf.Abs(change);

        counterTween = DOVirtual.Int(startValue, current, duration, value =>
        {
            currentResourceCount = value;
            text.text = $"{currentResourceCount} {sign}{absChange}";
        })
        .SetEase(Ease.OutCubic)
        .OnComplete(() =>
        {
            currentResourceCount = current;
            text.text = currentResourceCount.ToString();
        });
    }
}

using System;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI experienceText;
    private int _goalExperience;
    private Tween _experienceValueTween;
    private Tween _fillTween;

    private int _displayedExperience;
    void Awake()
    {
        PlayerResources.OnExperienceChanged += UpdateExperienceUI;
        _goalExperience = GameManager.Instance.GoalExperience;

        UpdateExperienceUI(null, 0, 0);
    }

        private void OnDestroy()
    {
        PlayerResources.OnExperienceChanged -= UpdateExperienceUI;
    }

private void UpdateExperienceUI(PlayerResources resources, int current, int change)
{
    int trueStartExp = current - change;
    int targetExp = current;

    _experienceValueTween?.Kill();
    _fillTween?.Kill();

    int startExp = Mathf.Max(_displayedExperience, trueStartExp);

    _experienceValueTween = DOTween.To(
        () => _displayedExperience,
        x =>
        {
            _displayedExperience = x;
            experienceText.text = $"{_displayedExperience}/{_goalExperience}";
        },
        targetExp,
        2f
    ).SetEase(Ease.Linear);

    float startFill = Mathf.Max(
        fillImage.fillAmount,
        (float)trueStartExp / _goalExperience
    );

    float targetFill = Mathf.Clamp01((float)targetExp / _goalExperience);

    _fillTween = DOTween.To(
        () => fillImage.fillAmount,
        x => fillImage.fillAmount = x,
        targetFill,
        2f
    ).SetEase(Ease.Linear);
}
}

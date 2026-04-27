using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CustomDropdown : Selectable, IPointerClickHandler {
    [Space(20)]
    [SerializeField] private RectTransform arrow;
    [SerializeField] private CanvasGroup scrollViewCanvasGroup;

    private Tween arrowScaleTween;
    public override void OnPointerEnter(PointerEventData eventData) {
        base.OnPointerEnter(eventData);

        arrowScaleTween?.Kill();
        arrow.localScale = Vector3.one;

        arrowScaleTween = arrow.DOScale(1.25f, 0.5f)
            .SetEase(Ease.OutQuad);
    }

    public override void OnPointerExit(PointerEventData eventData) {
        base.OnPointerExit(eventData);

        arrowScaleTween?.Kill();

        arrowScaleTween = arrow.DOScale(1f, 0.25f)
            .SetEase(Ease.OutQuad);
    }


    Tween arrowRotationTween;
    Tween scrollViewFadeTween;

    bool isExpanded = false;

    public void OnPointerClick(PointerEventData eventData) {
        isExpanded = !isExpanded;

        arrowRotationTween?.Kill(true);

        float targetZ = isExpanded ? 180f : 0f;

        arrowRotationTween = arrow
            .DORotate(new Vector3(0, 0, targetZ), 0.25f)
            .SetEase(Ease.OutQuad);

        scrollViewFadeTween?.Kill(true);

        float targetAlpha = isExpanded ? 1f : 0f;

        scrollViewFadeTween = scrollViewCanvasGroup
            .DOFade(targetAlpha, 0.25f)
            .OnStart(() => {
                if (isExpanded) {
                    scrollViewCanvasGroup.blocksRaycasts = true;
                    scrollViewCanvasGroup.interactable = true;
                }
            })
            .OnComplete(() => {
                if (!isExpanded) {
                    scrollViewCanvasGroup.blocksRaycasts = false;
                    scrollViewCanvasGroup.interactable = false;
                }
            });
    }
}

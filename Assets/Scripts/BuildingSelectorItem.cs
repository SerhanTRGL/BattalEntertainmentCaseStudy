using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingSelectorItem : Selectable, IPointerClickHandler
{
    [SerializeField] private GridEntity building;
    [SerializeField] private TextMeshProUGUI coinText;
    private float coinPrice;

    public void OnPointerClick(PointerEventData eventData) {
        if (BuildingSelector.Instance == null) return;

        BuildingSelector.Instance.SetSelection(building);

    }

    protected override void Awake() {
        PlayerResources.OnCoinsChanged += UpdateItemVisual;
        coinPrice = building.buildCoinCost;
        coinText.text = coinPrice.ToString();
    }

    Tween coinTextColorTween;
    private void UpdateItemVisual(PlayerResources _, int current, int change) {
        coinTextColorTween?.Kill();

        coinTextColorTween = coinText.DOColor(current < coinPrice ? Color.red : Color.white, 0.5f);
    }

}

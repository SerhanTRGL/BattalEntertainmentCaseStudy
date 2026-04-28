using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingSelectorItem : Selectable, IPointerClickHandler
{
    [SerializeField] private GridEntity _entity;
    [SerializeField] private TextMeshProUGUI _coinText;
    private float _coinPrice;

    public void OnPointerClick(PointerEventData eventData) {
        if (BuildingSelector.Instance == null) return;

        BuildingSelector.Instance.SetSelection(_entity);

    }

    protected override void Awake() {
        PlayerResources.OnCoinsChanged += UpdateItemVisual;
        _coinPrice = _entity.buildCoinCost;
        _coinText.text = _coinPrice.ToString();
    }

    Tween _coinTextColorTween;
    private void UpdateItemVisual(PlayerResources _, int current, int change) {
        _coinTextColorTween?.Kill();

        _coinTextColorTween = _coinText.DOColor(current < _coinPrice ? Color.red : Color.white, 0.5f);
    }


    protected override void OnValidate() {
        if (_entity == null) return;

        _coinText.text = _entity.buildCoinCost.ToString();
    }
}

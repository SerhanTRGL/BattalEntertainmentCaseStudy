using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSelectorItem : Selectable
{
    [SerializeField] private GridEntity building;
    [SerializeField] private TextMeshProUGUI coinText;
    private float coinPrice;
    protected override void Awake() {
        PlayerResources.OnResourceValueChanged += UpdateItemVisual;
        coinPrice = building.buildCost.Where(t => t.Type == ResourceType.Coin).First().Amount;
        coinText.text = coinPrice.ToString();
    }

    private void UpdateItemVisual(ResourceChange change) {
        if (change.changedResource.Type != ResourceType.Coin) return;

        coinText.color = change.changedResource.Amount < coinPrice ? Color.red : Color.white;
    }
}

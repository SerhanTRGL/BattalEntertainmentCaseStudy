using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class BuildingSelector : Selectable{
    [Space(20)]
    [SerializeField] private Image arrow;

    public override void OnPointerEnter(PointerEventData eventData) {
        base.OnPointerEnter(eventData);

    }


}

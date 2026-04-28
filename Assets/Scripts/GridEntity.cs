using DG.Tweening;
using UnityEngine;

public class GridEntity : MonoBehaviour
{
    private GridEntitySO _entitySO;
    private GameObject _entityVisual;
    private bool _needsToMature;

   private Tween _maturingTween;
    public float MaturingPercent => _maturingTween != null && _maturingTween.IsActive()
    ? _maturingTween.ElapsedPercentage()
    : 1f;    private void Start()
    {
        _entityVisual.transform.localScale = Vector3.zero;
        if (_needsToMature)
            StartMaturing();
        else
            _entityVisual.transform.DOScale(1, 0.5f);
    }

    private void StartMaturing()
    {
        float duration = (_entitySO as ResourceEntitySO).maturingTime;

        _maturingTween = _entityVisual.transform
            .DOScale(1, duration)
            .SetEase(Ease.Linear);
    }


    public void PlaceEntityAtPosition(GridEntitySO entitySo, Vector3 position)
    {
        _entitySO = entitySo;

        if (entitySo is ResourceEntitySO)
        {
            _needsToMature = true;
        }

        _entityVisual = Instantiate(entitySo.prefab, transform);
        transform.position = position;
    }

    public void DestroyEntity()
    {
        transform.DOScale(0, 0.5f).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
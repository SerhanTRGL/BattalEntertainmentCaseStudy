using DG.Tweening;
using UnityEngine;

public class GridEntity : MonoBehaviour
{
    private GridEntitySO _entitySO;
    private GameObject _entityVisual;
    private bool _needsToMature;


    private void Start()
    {
        if (_needsToMature)
            StartMaturing();
        else
            _entityVisual.transform.localScale = Vector3.one;
    }

    private void StartMaturing()
    {
        _entityVisual.transform.localScale = Vector3.zero;
        _entityVisual.transform.DOScale(1, (_entitySO as ResourceEntitySO).maturingTime);
    }


    public void PlaceEntityAtPosition(GridEntitySO entitySo, Vector3 position)
    {
        _entitySO = entitySo;

        if (entitySo is ResourceEntitySO)
        {
            _needsToMature = true;
        }

        _entityVisual = Instantiate(entitySo.prefab, transform);
    }

    public void DestroyEntity()
    {
        transform.DOScale(0, 0.5f).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
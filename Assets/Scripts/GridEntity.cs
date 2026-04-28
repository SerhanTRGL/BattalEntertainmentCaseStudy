using DG.Tweening;
using UnityEngine;

public class GridEntity : MonoBehaviour
{
    private GridEntitySO _entitySO;
    private GameObject _entityVisual;
    private bool _needsToMature;

    private Tween _maturingTween;
    public float MaturingPercent => _maturingTween != null && _maturingTween.IsActive() ? _maturingTween.ElapsedPercentage() : 1f;
    private void Start()
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

        ManagePlacementResources();
    }

    private void ManagePlacementResources()
    {
        var playerResources = GameManager.Instance.PlayerResources;
        playerResources.SpendCoins(_entitySO.buildCoinCost);
        playerResources.GainExperience(Random.Range(_entitySO.buildExperienceRewardRange.x, _entitySO.buildExperienceRewardRange.y));
    }
    private void ManageDestructionResources()
    {
        var playerResources = GameManager.Instance.PlayerResources;

        float maturityFactor = _needsToMature ? MaturingPercent : 1f;

        int resourceGain = Mathf.CeilToInt(
            Random.Range(_entitySO.destroyResourceRewardRange.x, _entitySO.destroyResourceRewardRange.y)
            * maturityFactor
        );

        int experienceGain = Mathf.CeilToInt(
            Random.Range(_entitySO.destroyExperienceRewardRange.x, _entitySO.destroyExperienceRewardRange.y)
            * maturityFactor
        );

        int coinGain = Mathf.CeilToInt(_entitySO.destroyCoinReward * maturityFactor);

        playerResources.GainResource(resourceGain);
        playerResources.GainCoins(coinGain);
        playerResources.GainExperience(experienceGain);
    }
    public void DestroyEntity()
    {
        ManageDestructionResources();
        transform.DOScale(0, 0.5f).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
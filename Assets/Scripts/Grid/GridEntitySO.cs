using UnityEngine;

public class GridEntitySO : ScriptableObject
{
    public GameObject prefab;

    public int buildCoinCost;
    public int destroyCoinReward;

    public Vector2Int buildExperienceRewardRange;
    public Vector2Int destroyResourceRewardRange;
    public Vector2Int destroyExperienceRewardRange;
}

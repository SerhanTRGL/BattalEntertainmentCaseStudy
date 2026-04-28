using UnityEngine;


public abstract class GridEntity : ScriptableObject
{
    public GameObject prefab;
    public GridEntityType type;

    public int buildCoinCost;
    public int destroyCoinReward;

    public Vector2Int buildExperienceRewardRange;
    public Vector2Int destroyResourceRewardRange;
    public Vector2Int destroyExperienceRewardRange;
}

[CreateAssetMenu(fileName = "Building Entity", menuName = "Scriptable Objects/Building Entity")]
public class BuildingEntity : GridEntity {}

[CreateAssetMenu(fileName = "Resource Entity", menuName = "Scriptable Objects/Resource Entity")]
public class ResourceEntity : GridEntity {

    [Tooltip("Maturing time in seconds. Will give fraction of rewards if destroyed without maturing")]
    public float maturingTime;
}
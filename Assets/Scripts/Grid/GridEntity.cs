using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Grid Entity", menuName = "ScriptableObjects/Grid Entity")]
public class GridEntity : ScriptableObject
{
    public GameObject prefab;
    public GridEntityType type;
    public List<Resource> buildCost;
    public List<Resource> buildReward;
    public List<Resource> destructionReward;
}

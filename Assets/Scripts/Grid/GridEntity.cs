using System.Collections.Generic;
using UnityEngine;

public class GridEntity : ScriptableObject
{
    public GameObject prefab;
    public GridEntityType type;
    public List<Resource> buildCost;
    public List<Resource> destructionReward;
}

using System;
using System.Collections.Generic;
using UnityEngine;

public struct CellData
{
    public Vector2Int coordinate;
    public GridEntity occupyingEntity;
}

public enum EntityType
{
    Building,
    Resource
}
public class GridEntity : ScriptableObject
{
    public GameObject prefab;
    public EntityType type;
    
    public List<Resource> buildCost;

    public List<Resource> destructionReward;
}

[Serializable]
public class Resource
{
    [SerializeField] private ResourceType _type;
    [SerializeField] private int _amount;
    public ResourceType Type{get => _type; set=> _type = value;}
    public int Amount { get=> _amount; set => _amount = value;}
    public Resource(ResourceType type, int amount)
    {
        _type = type;
        _amount = amount;
    }
}

public class GridEntityDatabase : ScriptableObject
{
    public readonly List<GridEntity> entities = new();

    public int GetEntityIndex(GridEntity entity)
    {
        if(entities.Contains(entity))
            return entities.IndexOf(entity);
        
        return -1;
    }

    public GridEntity GetEntityAtIndex(int index)
    {
        if(index >= 0 && index < entities.Count)
            return entities[index];

        return null;
    }
}
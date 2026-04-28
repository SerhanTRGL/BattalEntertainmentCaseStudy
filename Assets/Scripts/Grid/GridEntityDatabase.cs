using System.Collections.Generic;
using UnityEngine;

public class GridEntityDatabase : ScriptableObject
{
    public readonly List<GridEntitySO> entities = new();

    public int GetEntityIndex(GridEntitySO entity)
    {
        if(entities.Contains(entity))
            return entities.IndexOf(entity);
        
        return -1;
    }

    public GridEntitySO GetEntityAtIndex(int index)
    {
        if(index >= 0 && index < entities.Count)
            return entities[index];

        return null;
    }
}
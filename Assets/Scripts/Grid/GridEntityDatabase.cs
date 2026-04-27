using System.Collections.Generic;
using UnityEngine;

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
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int startingCoins = 500;
    public int goalExperience = 500;

}

public enum ResourceType
{
    Coin,
    Experience,
    Resource
}
public class PlayerResources
{
    private Dictionary<ResourceType, int> _resources = new();

    public PlayerResources()
    {
        List<ResourceType> resourceTypes = Enum.GetValues(typeof(ResourceType)).Cast<ResourceType>().ToList();
        foreach(var resourceType in resourceTypes)
        {
            _resources[resourceType] = 0;
        }
    }   

    public int GetResourceOfType(ResourceType type)
    {
        return _resources[type];
    }

    public bool SpendResource(ResourceType resource, int amountToSpend)
    {
        int resourceAmount = _resources[resource];
        if(resourceAmount - amountToSpend < 0) 
            return false;
        
        _resources[resource] -= amountToSpend;
        return true;
    }

    public void GainResource(ResourceType resource, int amountToGain)
    {
        _resources[resource] += amountToGain;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

public class PlayerResources
{
    private readonly Dictionary<ResourceType, Resource> _resources = new();
    public static event Action<ResourceChange> OnResourceValueChanged;
    public PlayerResources()
    {
        List<ResourceType> resourceTypes = Enum.GetValues(typeof(ResourceType)).Cast<ResourceType>().ToList();
        foreach(var type in resourceTypes)
        {
            _resources.Add(type, new Resource(type, 0));
            OnResourceValueChanged?.Invoke(new ResourceChange{
                changedResource = _resources[type],
                changeAmount = 0
            });
        }
    }   

    public Resource GetResourceOfType(ResourceType type)
    {
        return _resources[type];
    }

    public bool SpendResource(ResourceType type, int amountToSpend)
    {
        int resourceAmount = _resources[type].Amount;
        if(resourceAmount - amountToSpend < 0) 
            return false;
        
        _resources[type].Amount -= amountToSpend;
        OnResourceValueChanged?.Invoke(new ResourceChange{
                changedResource = _resources[type],
                changeAmount = 0
        });
        return true;
    }

    public void GainResource(ResourceType type, int amountToGain)
    {
        _resources[type].Amount += amountToGain;
        OnResourceValueChanged?.Invoke(new ResourceChange{
            changedResource = _resources[type],
            changeAmount = amountToGain
            }
        );
    }
}
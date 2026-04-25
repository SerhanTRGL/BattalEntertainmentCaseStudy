using System;
using UnityEngine;

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

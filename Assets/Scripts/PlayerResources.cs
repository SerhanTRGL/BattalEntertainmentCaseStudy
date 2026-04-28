using System;
using UnityEngine;

public class PlayerResources
{
    private int _coins;
    private int _experience;
    private int _resource;

    public int Coins { get => _coins; }
    public int Experience { get => _experience; }
    public int Resource { get => _resource; }

    /// <summary>
    /// Current coin amount, change amount
    /// </summary>
    public static event Action<PlayerResources, int, int> OnCoinsChanged;

    /// <summary>
    /// Current experience amount, change amount
    /// </summary>
    public static event Action<PlayerResources, int, int> OnExperienceChanged;

    /// <summary>
    /// Current resource amount, change amount
    /// </summary>
    public static event Action<PlayerResources, int, int> OnResourceChanged;

    public PlayerResources()
    {
        _coins = 0;
        OnCoinsChanged?.Invoke(this, 0, 0);

        _experience = 0;
        OnExperienceChanged?.Invoke(this, 0, 0);

        _resource = 0;
        OnResourceChanged?.Invoke(this, 0, 0);
    }


    public bool SpendCoins(int amountToSpend) {
        if (amountToSpend < 0) 
            return false;

        if (_coins - amountToSpend < 0)
            return false;

        _coins -= amountToSpend;
        OnCoinsChanged?.Invoke(this, _coins, -amountToSpend);
        return true;
    }
    
    public void GainCoins(int amountToGain) {
        if (amountToGain < 0) return;

        _coins += amountToGain;
        OnCoinsChanged?.Invoke(this, _coins, amountToGain);
    }

    public void GainExperience(int amountToGain) {
        if (amountToGain < 0) return;

        _experience += amountToGain;
        OnExperienceChanged?.Invoke(this, _experience, amountToGain);
    }

    public void GainResource(int amountToGain) {
        if (amountToGain < 0) return;

        _resource += amountToGain;
        OnResourceChanged?.Invoke(this, _resource, amountToGain);
    }
}
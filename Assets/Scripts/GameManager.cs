using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int startingCoins = 500;
    public int goalExperience = 500;
    public readonly PlayerResources playerResources = new();

    private void Start() {
        playerResources.GainResource(ResourceType.Coin, startingCoins);
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "Resource Entity SO", menuName = "Scriptable Objects/Resource Entity")]
public class ResourceEntitySO : GridEntitySO {

    [Space(10)]
    [Tooltip("Maturing time in seconds. Will give fraction of rewards if destroyed without maturing")]
    public float maturingTime;
}
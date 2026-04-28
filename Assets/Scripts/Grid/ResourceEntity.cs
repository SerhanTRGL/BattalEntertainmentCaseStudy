using UnityEngine;

[CreateAssetMenu(fileName = "Resource Entity", menuName = "Scriptable Objects/Resource Entity")]
public class ResourceEntity : GridEntity {

    [Space(10)]
    [Tooltip("Maturing time in seconds. Will give fraction of rewards if destroyed without maturing")]
    public float maturingTime;
}
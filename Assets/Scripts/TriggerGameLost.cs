using UnityEngine;

public class TriggerGameLost : MonoBehaviour
{
    [SerializeField] private GridEntity entity;
    void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponentInParent<Enemy>();
        if(enemy.TargetEntity == entity)
        {
            GameManager.Instance.TriggerGameOver();
        }
    }
}

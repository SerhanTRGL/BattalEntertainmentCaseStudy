using System;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    [SerializeField] private GridEntity targetEntity;
    [SerializeField] private float movementSpeed;
    public void InitializeEnemy(Vector3 startPoint)
    {
        transform.position = startPoint;

        GridEntityManager.OnGridEntityDestroyed += HandleEntityDestroyed;
        GridEntityManager.OnGridEntityPlaced += HandleEntityPlaced;

        FindNewTarget();
    }

    private void OnDestroy()
    {
        GridEntityManager.OnGridEntityDestroyed -= HandleEntityDestroyed;
        GridEntityManager.OnGridEntityPlaced -= HandleEntityPlaced;
    }

    private void Update()
    {
        if (targetEntity == null) return;

        Vector3 targetPos = targetEntity.transform.position;

        // Move
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            movementSpeed * Time.deltaTime
        );

        // Look at target
        Vector3 dir = targetPos - transform.position;
        if (dir.sqrMagnitude > 0.001f)
            transform.forward = dir.normalized;
    }


        private void HandleEntityDestroyed(Vector2Int _, GridEntity entity)
    {
        if (entity != targetEntity) return;

        targetEntity = null;
        FindNewTarget();
    }

    private void HandleEntityPlaced(Vector2Int _, GridEntity entity)
    {
        // Only grab a new target if we don't have one
        if (targetEntity == null)
            targetEntity = entity;
    }

    private void FindNewTarget()
    {

        var all = FindObjectsByType<GridEntity>(FindObjectsSortMode.None);

        if (all.Length > 0)
        {
            targetEntity = all[0];
        }
        else
        {
            targetEntity = null;
        }
    }
}

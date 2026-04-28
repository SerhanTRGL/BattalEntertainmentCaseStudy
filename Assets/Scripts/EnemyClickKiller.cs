using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyClickKiller : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private InputActionReference clickAction;

    void Awake()
    {
        clickAction.action.performed += CheckIfClickedOnEnemy;
    }

private void CheckIfClickedOnEnemy(InputAction.CallbackContext context)
{
    Vector2 screenPos = Mouse.current.position.ReadValue();

    Ray ray = cam.ScreenPointToRay(screenPos);

    if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, enemyLayer))
    {
        Debug.Log(hit.collider);
        var enemy = hit.collider.GetComponentInParent<Enemy>();
        if(enemy != null)
            Destroy(enemy.gameObject);
    }
}
}

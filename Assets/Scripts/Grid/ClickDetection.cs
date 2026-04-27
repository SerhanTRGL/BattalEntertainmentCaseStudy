using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickDetection : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask gridLayerMask;
    [SerializeField] private Transform testSphere;
    private static Action<Vector3, ClickType> OnClickedOnGrid;
    void Awake()
    {
        InputHandler.OnClick += DetectClick;
        OnClickedOnGrid += MoveSphereToCell;
    }

    private void MoveSphereToCell(Vector3 vector, ClickType type)
    {
        var helper = new GridHelper(GameObject.FindGameObjectWithTag("Grid").GetComponent<CustomGrid>());
        var cell = helper.WorldPointToCellCoordinate(vector);

        testSphere.position = helper.GetCellPosition(cell);
    }

    private void DetectClick(Vector2 screenPosition, ClickType type)
    {
        if(EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        Ray ray = cam.ScreenPointToRay(screenPosition);

        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, gridLayerMask))
        {
            Vector3 hitPoint = hit.point;
            OnClickedOnGrid?.Invoke(hitPoint, type);
            
            var helper = new GridHelper(GameObject.FindGameObjectWithTag("Grid").GetComponent<CustomGrid>());
            var cell = helper.WorldPointToCellCoordinate(hitPoint); 
            Debug.Log($"Clicked on:{cell}");
        }
    }  
}


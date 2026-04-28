using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private InputActionReference zoomInput;
    [SerializeField] private float zoomSpeed = 1f;
    private CinemachineOrbitalFollow orbitalFollow;
    private float minRadius;
    private float maxRadius;

    void Awake()
    {
        GameGrid.OnGridReady += CalculateOrbitalRadius;
    }

    private void CalculateOrbitalRadius(GameGrid grid)
    {
        var gridSize = grid.Size;
        var cellSize = grid.CellSize;

        minRadius = Math.Max(gridSize.x, gridSize.y) * cellSize * 0.5f;
        maxRadius = Math.Max(gridSize.x, gridSize.y) * cellSize * 1.5f;
    }

    void Start()
    {
        orbitalFollow = GetComponent<CinemachineOrbitalFollow>();
    }

    void Update()
    {
        //Don't scroll when mouse is over UI elements
        if(EventSystem.current.IsPointerOverGameObject())
            return;
        float scroll = zoomInput.action.ReadValue<Vector2>().y;

        if(Mathf.Abs(scroll) < 0.01f)
            return;

        orbitalFollow.Radius *= 1f - scroll * zoomSpeed * Time.deltaTime;
        orbitalFollow.Radius = Mathf.Clamp(
            orbitalFollow.Radius,
            minRadius,
            maxRadius
        );
    }
}

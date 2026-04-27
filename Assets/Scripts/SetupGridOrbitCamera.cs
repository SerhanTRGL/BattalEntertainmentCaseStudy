using System;
using Unity.Cinemachine;
using UnityEngine;

public class SetupGridOrbitCamera : MonoBehaviour
{
    void Awake()
    {
        GridBuilder.OnGridReady += SetupCamera;
    }

    private void SetupCamera(GridBuilder grid)
    {
        var orbitalComponent = GetComponent<CinemachineOrbitalFollow>();
        orbitalComponent.Radius = Math.Max(grid.GridSize.x, grid.GridSize.y) * grid.CellSize;
    }
}

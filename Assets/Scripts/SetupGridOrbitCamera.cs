using System;
using Unity.Cinemachine;
using UnityEngine;

public class SetupGridOrbitCamera : MonoBehaviour
{
    void Awake()
    {
        CustomGrid.OnGridReady += SetupCamera;
    }

    private void SetupCamera(CustomGrid grid)
    {
        var orbitalComponent = GetComponent<CinemachineOrbitalFollow>();
        orbitalComponent.Radius = Math.Max(grid.GridSize.x, grid.GridSize.y) * grid.CellSize;
    }
}

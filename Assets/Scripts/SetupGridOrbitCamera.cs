using System;
using Unity.Cinemachine;
using UnityEngine;

public class SetupGridOrbitCamera : MonoBehaviour
{
    void Awake()
    {
        GameGrid.OnGridReady += SetupCamera;
    }

    private void SetupCamera(GameGrid grid)
    {
        var orbitalComponent = GetComponent<CinemachineOrbitalFollow>();
        orbitalComponent.Radius = Math.Max(grid.Size.x, grid.Size.y) * grid.CellSize;
    }
}

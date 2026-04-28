using System;
using Unity.Cinemachine;
using UnityEngine;

public class SetupGridOrbitCamera : MonoBehaviour
{
    void Awake()
    {
        Grid.OnGridReady += SetupCamera;
    }

    private void SetupCamera(Grid grid)
    {
        var orbitalComponent = GetComponent<CinemachineOrbitalFollow>();
        orbitalComponent.Radius = Math.Max(grid.Size.x, grid.Size.y) * grid.CellSize;
    }
}

using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class CustomGrid : MonoBehaviour
{
    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private float cellSize;

    public static event Action<CustomGrid> OnGridReady;
    public Vector2Int GridSize => gridSize;
    public float CellSize => cellSize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = GridMeshGenerator.GenerateMesh(gridSize, cellSize);
        var collider = GetComponentInChildren<BoxCollider>();
        collider.size = new Vector3(gridSize.x * cellSize, 0, gridSize.y * cellSize);
        collider.center = Vector3.zero;

        OnGridReady?.Invoke(this);
    }

}

public class GridHelper
{
    private CustomGrid _grid;

    public CustomGrid Grid => _grid;
    public GridHelper(CustomGrid grid)
    {
        _grid = grid;
    }

    public Vector2Int WorldPointToCellCoordinate(Vector3 worldPoint) {
        Vector3 localPoint = _grid.transform.InverseTransformPoint(worldPoint);

        float cellSize = _grid.CellSize;

        Vector3 centerOffset = new Vector3(
            _grid.GridSize.x * cellSize * 0.5f,
            0,
            _grid.GridSize.y * cellSize * 0.5f
        );

        localPoint += centerOffset;

        int x = Mathf.FloorToInt(localPoint.x / cellSize);
        int y = Mathf.FloorToInt(localPoint.z / cellSize);

        if (x >= _grid.GridSize.x || x < 0 || y >= _grid.GridSize.y || y < 0)
            return -Vector2Int.one;

        return new Vector2Int(x, y);
    }

    public Vector3 GetCellPosition(Vector2Int cellCoordinate) {
        if (cellCoordinate.x >= _grid.GridSize.x || cellCoordinate.x < 0 ||
           cellCoordinate.y >= _grid.GridSize.y || cellCoordinate.y < 0)
            return -Mathf.Infinity * Vector3.one;

        float cellSize = _grid.CellSize;

        Vector3 centerOffset = new Vector3(
            _grid.GridSize.x * cellSize * 0.5f,
            0,
            _grid.GridSize.y * cellSize * 0.5f
        );

        Vector3 localPos = new Vector3(
            cellCoordinate.x * cellSize + cellSize * 0.5f,
            0,
            cellCoordinate.y * cellSize + cellSize * 0.5f
        ) - centerOffset;

        return _grid.transform.TransformPoint(localPos);
    }
}

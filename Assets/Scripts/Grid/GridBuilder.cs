using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GridBuilder : MonoBehaviour
{
    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private float cellSize;
    [SerializeField] private Material gridMaterial;

    public static event Action<GridBuilder> OnGridReady;
    public Vector2Int GridSize => gridSize;
    public float CellSize => cellSize;

    void Start()
    {
        var meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = GridMeshGenerator.GenerateMesh(gridSize, cellSize);

        var meshRenderer = GetComponent<MeshRenderer>();
        gridMaterial.SetVector("_GridSize", new Vector4(gridSize.x, gridSize.y));
        meshRenderer.material = gridMaterial;

        var collider = GetComponentInChildren<BoxCollider>();
        collider.size = new Vector3(gridSize.x * cellSize, 0, gridSize.y * cellSize);
        collider.center = Vector3.zero;
        OnGridReady?.Invoke(this);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.white;

        float width = gridSize.x * cellSize;
        float height = gridSize.y * cellSize;


        Vector3 origin = transform.position - new Vector3(width, 0, height) * 0.5f;

        //vertical lines
        for(int x = 0; x <= gridSize.x; x++) {
            Vector3 start = origin + new Vector3(x * cellSize, 0, 0);
            Vector3 end = start + new Vector3(0, 0, height);

            Gizmos.DrawLine(start, end);
        }

        //horizontal lines
        for(int y = 0; y <= gridSize.y; y++) {
            Vector3 start = origin + new Vector3(0, 0, y * cellSize);
            Vector3 end = start + new Vector3(width, 0, 0);

            Gizmos.DrawLine(start, end);
        }
    }

}

public class GridHelper
{
    private GridBuilder _grid;

    public GridBuilder Grid => _grid;
    public GridHelper(GridBuilder grid)
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

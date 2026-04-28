using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour
{
    [SerializeField] private Vector2Int _size;
    [SerializeField] private float _cellSize;
    [SerializeField] private Material _gridMaterial;

    public static event Action<Grid> OnGridReady;
    public Vector2Int Size => _size;
    public float CellSize => _cellSize;

    void Start()
    {
        var meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = GridMeshGenerator.GenerateMesh(_size, _cellSize);

        var meshRenderer = GetComponent<MeshRenderer>();
        _gridMaterial.SetVector("_GridSize", new Vector4(_size.x, _size.y));
        meshRenderer.material = _gridMaterial;

        var collider = GetComponentInChildren<BoxCollider>();
        collider.size = new Vector3(_size.x * _cellSize, 0, _size.y * _cellSize);
        collider.center = Vector3.zero;
        OnGridReady?.Invoke(this);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.white;

        float width = _size.x * _cellSize;
        float height = _size.y * _cellSize;


        Vector3 origin = transform.position - new Vector3(width, 0, height) * 0.5f;

        //vertical lines
        for(int x = 0; x <= _size.x; x++) {
            Vector3 start = origin + new Vector3(x * _cellSize, 0, 0);
            Vector3 end = start + new Vector3(0, 0, height);

            Gizmos.DrawLine(start, end);
        }

        //horizontal lines
        for(int y = 0; y <= _size.y; y++) {
            Vector3 start = origin + new Vector3(0, 0, y * _cellSize);
            Vector3 end = start + new Vector3(width, 0, 0);

            Gizmos.DrawLine(start, end);
        }
    }

}

public class GridHelper
{
    private Grid _grid;

    public Grid Grid => _grid;
    public GridHelper(Grid grid)
    {
        _grid = grid;
    }

    public Vector2Int WorldPointToCellCoordinate(Vector3 worldPoint) {
        Vector3 localPoint = _grid.transform.InverseTransformPoint(worldPoint);

        float cellSize = _grid.CellSize;

        Vector3 centerOffset = new Vector3(
            _grid.Size.x * cellSize * 0.5f,
            0,
            _grid.Size.y * cellSize * 0.5f
        );

        localPoint += centerOffset;

        int x = Mathf.FloorToInt(localPoint.x / cellSize);
        int y = Mathf.FloorToInt(localPoint.z / cellSize);

        if (x >= _grid.Size.x || x < 0 || y >= _grid.Size.y || y < 0)
            return -Vector2Int.one;

        return new Vector2Int(x, y);
    }

    public Vector3 GetCellPosition(Vector2Int cellCoordinate) {
        if (cellCoordinate.x >= _grid.Size.x || cellCoordinate.x < 0 ||
           cellCoordinate.y >= _grid.Size.y || cellCoordinate.y < 0)
            return -Mathf.Infinity * Vector3.one;

        float cellSize = _grid.CellSize;

        Vector3 centerOffset = new Vector3(
            _grid.Size.x * cellSize * 0.5f,
            0,
            _grid.Size.y * cellSize * 0.5f
        );

        Vector3 localPos = new Vector3(
            cellCoordinate.x * cellSize + cellSize * 0.5f,
            0,
            cellCoordinate.y * cellSize + cellSize * 0.5f
        ) - centerOffset;

        return _grid.transform.TransformPoint(localPos);
    }

    public bool IsValidCellCoordinate(Vector2Int cellCoordinate) {
        return cellCoordinate.x >= 0 && cellCoordinate.x < _grid.Size.x && cellCoordinate.y >= 0 && cellCoordinate.y < _grid.Size.y;
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class GridEntityManager : MonoBehaviour
{
    [SerializeField] private GameObject _gridEntityPrefab;
    private Dictionary<Vector2Int, GridEntity> _gridEntities = new();
    private HashSet<Vector2Int> _occupiedCells = new();
    private HashSet<Vector2Int> _availableCells = new();

    #if UNITY_EDITOR
    [SerializeField] private List<Vector2Int> _occupiedCellsList = new();
    [SerializeField] private List<Vector2Int> _availableCellsList = new();
    #endif

    private GridHelper _gridHelper;

    public static event Action<Vector2Int, GridEntity> OnGridEntityPlaced;
    public static event Action<Vector2Int, GridEntity> OnGridEntityDestroyed;

    private void Awake() {
        GameGrid.OnGridReady += InitializeGridEntityManager;
    }

    private void OnDestroy()
    {
        GameGrid.OnGridReady -= InitializeGridEntityManager;       
    }
    private void InitializeGridEntityManager(GameGrid grid) {
        _gridHelper = new GridHelper(grid);

        //Initialize grid entities
        for(int x = 0; x < _gridHelper.Grid.Size.x; x++) {
            for(int y = 0; y < _gridHelper.Grid.Size.y; y++) {
                var cellCoordinate = new Vector2Int(x, y);
                _gridEntities[cellCoordinate] = null;
                _availableCells.Add(cellCoordinate);

                #if UNITY_EDITOR
                _availableCellsList.Add(cellCoordinate);
                #endif
            }
        }
    }

    public GridEntity GetEntityAtCell(Vector2Int cellCoordinate) {
        if (!_gridHelper.IsValidCellCoordinate(cellCoordinate)) return null;

        return _gridEntities[cellCoordinate];
    }

    public bool CanPlaceEntityAtCell(Vector2Int cellCoordinate)
    {
        return _availableCells.Contains(cellCoordinate);
    }

    public void PlaceEntityAtCell(GridEntitySO entitySO, Vector2Int cellCoordinate)
    {
        if(!_availableCells.Contains(cellCoordinate)) return; //Already occupied

        var entity = Instantiate(_gridEntityPrefab).GetComponent<GridEntity>();
        entity.PlaceEntityAtPosition(entitySO, _gridHelper.CellCoordinateToWorldPosition(cellCoordinate));

        _gridEntities[cellCoordinate] = entity;
        _availableCells.Remove(cellCoordinate);
        _occupiedCells.Add(cellCoordinate);
        
        #if UNITY_EDITOR
        _availableCellsList.Remove(cellCoordinate);
        _occupiedCellsList.Add(cellCoordinate);
        #endif

        OnGridEntityPlaced?.Invoke(cellCoordinate, entity);
    }

    public void DestroyEntityAtCell(Vector2Int cellCoordinate)
    {
        if(!_occupiedCells.Contains(cellCoordinate)) return; //Nothing to destroy

        var entity = _gridEntities[cellCoordinate];
        OnGridEntityDestroyed?.Invoke(cellCoordinate, entity);

        entity.DestroyEntity();

        _occupiedCells.Remove(cellCoordinate);
        _availableCells.Add(cellCoordinate);
        _gridEntities[cellCoordinate] = null;

        #if UNITY_EDITOR
        _occupiedCellsList.Remove(cellCoordinate);
        _availableCellsList.Add(cellCoordinate);
        #endif

        
    }
}

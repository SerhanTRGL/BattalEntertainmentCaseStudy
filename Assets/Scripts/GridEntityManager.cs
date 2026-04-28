using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GridEntityManager : MonoBehaviour
{
    private Dictionary<Vector2Int, GridEntity> _gridEntities = new();
    private GridHelper _gridHelper;

    public static event Action<Vector2Int, GridEntity> OnGridEntityPlaced;
    public static event Action<Vector2Int, GridEntity> OnGridEntityDestroyed;

    private void Awake() {
        Grid.OnGridReady += InitializeGridEntityManager;
    }

    private void InitializeGridEntityManager(Grid grid) {
        _gridHelper = new GridHelper(grid);

        //Initialize grid entities
        for(int x = 0; x < _gridHelper.Grid.Size.x; x++) {
            for(int y = 0; y < _gridHelper.Grid.Size.y; y++) {
                _gridEntities[new Vector2Int(x, y)] = null;
            }
        }
    }

    public GridEntity GetGridEntityAtCell(Vector2Int cellCoordinate) {
        if (!_gridHelper.IsValidCellCoordinate(cellCoordinate)) return null;

        return _gridEntities[cellCoordinate];
    }
}

public class GridEntity : MonoBehaviour{
    private GridEntitySO _entitySO;

    private bool needsToMature;

    public void InitializeGridEntity(GridEntitySO entitySo) {
        _entitySO = entitySo;

        if(entitySo is ResourceEntitySO) {
            needsToMature = true;
        }
    }

    private void Start() {
        if (needsToMature)
            StartMaturing();
    }

    private void StartMaturing() {
        transform.localScale = Vector3.zero;
        transform.DOScale(1, (_entitySO as ResourceEntitySO).maturingTime);
    }
}
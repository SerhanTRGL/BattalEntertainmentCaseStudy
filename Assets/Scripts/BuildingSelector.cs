using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingSelector : MonoBehaviour
{
    [SerializeField] private InputActionReference _cancelAction;
    [SerializeField] private InputActionReference _clickAction;
    public static BuildingSelector Instance;

    private GameObject _previewInstance;
    private GridHelper _gridHelper;

    [SerializeField] private LayerMask _gridLayerMask;

    private GridEntityManager _entityManager;
    private Vector2Int _currentCell;
    private bool _canPlace;
    public GridEntitySO CurrentSelection { get; set; }

    void OnEnable()
    {
        DoubleClickHandler.OnDoubleClick += HandleDoubleClick;
        _clickAction.action.performed += HandleClick;
    }

    private void HandleClick(InputAction.CallbackContext context)
    {
        if (_gridHelper == null || _entityManager == null) return;
        if (_currentCell == -Vector2Int.one) return;

        if (CurrentSelection != null && _canPlace)
        {
            _entityManager.PlaceEntityAtCell(CurrentSelection, _currentCell);
            
            SetSelection(null);
        }
    }

    void OnDisable()
    {
        DoubleClickHandler.OnDoubleClick -= HandleDoubleClick;
        _clickAction.action.performed -= HandleClick;
    }

    private void HandleDoubleClick(Vector2 screenPos)
    {
        if (_gridHelper == null || _entityManager == null) return;

        Ray ray = Camera.main.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _gridLayerMask))
        {
            Vector2Int cell = _gridHelper.WorldPointToCellCoordinate(hit.point);

            if (cell != -Vector2Int.one)
            {
                _entityManager.DestroyEntityAtCell(cell);
            }
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _cancelAction.action.performed += CancelSelection;
        GameGrid.OnGridReady += g =>
        {
            _gridHelper = new GridHelper(g);
            _entityManager = FindFirstObjectByType<GridEntityManager>();
        };
    }

    private void CancelSelection(InputAction.CallbackContext context)
    {
        SetSelection(null);
    }

    private void Update()
    {
        if (CurrentSelection == null || _previewInstance == null || _gridHelper == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            Vector2Int cell = _gridHelper.WorldPointToCellCoordinate(hit.point);

            if (cell != -Vector2Int.one)
            {
                _currentCell = cell;

                Vector3 snappedPos = _gridHelper.CellCoordinateToWorldPosition(cell);
                _previewInstance.transform.position = snappedPos;

                _canPlace = _entityManager.CanPlaceEntityAtCell(cell);
                SetPreviewColor(_canPlace ? Color.green : Color.red);
            }
            else
            {
                _currentCell = -Vector2Int.one;
                _canPlace = false;
                _previewInstance.transform.position = hit.point;
                SetPreviewColor(Color.red);
            }
        }
        else
        {
            _currentCell = -Vector2Int.one;
            _canPlace = false;
        }
    }

    private void SetPreviewColor(Color color)
    {
        foreach (var r in _previewInstance.GetComponentsInChildren<Renderer>())
        {
            var c = color;
            c.a = 0.5f;
            r.material.color = c;
        }
    }

    public void SetSelection(GridEntitySO entity)
    {
        CurrentSelection = entity;

        if (_previewInstance != null)
            Destroy(_previewInstance);

        if (entity == null || entity.prefab == null)
            return;

        _previewInstance = Instantiate(entity.prefab);

        SetGhostMode(_previewInstance);
    }

    private void SetGhostMode(GameObject obj)
    {
        foreach (var col in obj.GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }

        foreach (var r in obj.GetComponentsInChildren<Renderer>())
        {
            var c = r.material.color;
            c.a = 0.5f;
            c *= Color.red;
            r.material.color = c;
        }
    }
}

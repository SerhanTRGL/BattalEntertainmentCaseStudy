using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingSelector : MonoBehaviour
{
    [SerializeField] private InputActionReference cancelAction;
    public static BuildingSelector Instance;

    private GameObject previewInstance;
    private GridHelper gridHelper;

    public GridEntitySO CurrentSelection { get; set; }
    private void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        cancelAction.action.performed += CancelSelection;
        GameGrid.OnGridReady += g => gridHelper = new GridHelper(g);
    }

    private void CancelSelection(InputAction.CallbackContext context) {
        SetSelection(null);
    }

    private void Update() {
        if (CurrentSelection == null || previewInstance == null || gridHelper == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) {
            Vector2Int cell = gridHelper.WorldPointToCellCoordinate(hit.point);

            if(cell != -Vector2Int.one) {
                Vector3 snappedPos = gridHelper.CellCoordinateToWorldPosition(cell);
                previewInstance.transform.position = snappedPos;
            }
            else {
                previewInstance.transform.position = hit.point;
            }
        }
    }

    public void SetSelection(GridEntitySO entity) {
        CurrentSelection = entity;

        if (previewInstance != null)
            Destroy(previewInstance);

        if (entity == null || entity.prefab == null)
            return;

        previewInstance = Instantiate(entity.prefab);

        SetGhostMode(previewInstance);
    }

    private void SetGhostMode(GameObject obj) {
        foreach(var col in obj.GetComponentsInChildren<Collider>()) {
            col.enabled = false;
        }

        foreach(var r in obj.GetComponentsInChildren<Renderer>()) {
            var c = r.material.color;
            c.a = 0.5f;
            c *= Color.red;
            r.material.color = c;
        }
    }
}

#if UNITY_EDITOR
using UnityEngine;


[ExecuteInEditMode]
public class UpdateTerrainInSceneView : MonoBehaviour {
    [SerializeField] private GridTerrainBuilder _terrainBuilder;
    [SerializeField] private GameGrid _grid;

    private Vector2Int previousGridSize = Vector2Int.zero;
    private float previousCellSize = 0f;
    private float timer = 0f;
    private void Update() {
        timer += Time.deltaTime;
        if (timer < 0.25f) return;
        timer = 0f;

        if(_grid.Size != previousGridSize || _grid.CellSize != previousCellSize) {
            previousGridSize = _grid.Size;
            previousCellSize = _grid.CellSize;
            _terrainBuilder.BuildTerrain(_grid);
        }
    }
}

#endif
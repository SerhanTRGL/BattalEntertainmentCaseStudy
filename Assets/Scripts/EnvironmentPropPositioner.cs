using System;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentPropPositioner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _propPrefabs;
    [SerializeField] private int spawnCount = 50;
    [SerializeField] private Grid _grid;


    private void Start() {
        if (_grid == null) return;

        float halfWidth = _grid.Size.x * _grid.CellSize * 0.5f;
        float halfHeight = _grid.Size.y * _grid.CellSize * 0.5f;

        float innerRadius = Mathf.Sqrt(halfWidth * halfWidth + halfHeight * halfHeight);
        innerRadius += _grid.CellSize * 2f;

        float outerRadius = innerRadius + _grid.CellSize * 5f;

        Vector3 center = _grid.transform.position;

        for(int i = 0; i < spawnCount; i++) {
            Vector3 pos = GetRandomPointInRing(center, innerRadius, outerRadius);

            GameObject prefab = _propPrefabs[UnityEngine.Random.Range(0, _propPrefabs.Count)];
            Instantiate(prefab, pos, Quaternion.identity, transform);
        }
    }

    private Vector3 GetRandomPointInRing(Vector3 center, float minRadius, float maxRadius) {
        float angle = UnityEngine.Random.value * Mathf.PI * 2f;

        // Uniform distribution between radii
        float r = Mathf.Sqrt(UnityEngine.Random.Range(minRadius * minRadius, maxRadius * maxRadius));

        float x = Mathf.Cos(angle) * r;
        float z = Mathf.Sin(angle) * r;

        return center + new Vector3(x, 0f, z);
    }
}

using System;
using UnityEngine;

public class GridTerrainBuilder : MonoBehaviour
{
    [SerializeField] private CustomGrid _grid;
    [SerializeField] private Terrain _terrain;
    [SerializeField] private TerrainData _data;
    [SerializeField] private GridRing _mountainSpawnRing;

    private void Awake() {
        CustomGrid.OnGridReady += BuildTerrain;
    }

    public void BuildTerrain(CustomGrid grid) {
        _grid = grid;

        _terrain = GetComponent<Terrain>();
        _data = _terrain.terrainData;

        int res = _data.heightmapResolution;
        float[,] heights = new float[res, res];

        Vector3 center = _mountainSpawnRing.Center;
        float size = _mountainSpawnRing.OuterRadius * 2f;
        _data.size = new Vector3(size, 200f, size);
        Vector3 terrainSize = _data.size;


        _terrain.transform.position = new Vector3(
            center.x - size * 0.5f,
            center.y - 0.01f,
            center.z - size * 0.5f
        );

        Vector3 terrainPos = _terrain.transform.position;

        float inner = _mountainSpawnRing.InnerRadius;
        float outer = _mountainSpawnRing.OuterRadius;

        for (int y = 0; y < res; y++) {
            for (int x = 0; x < res; x++) {

                float normX = (float)x / (res - 1);
                float normY = (float)y / (res - 1);

                float worldX = terrainPos.x + normX * terrainSize.x;
                float worldZ = terrainPos.z + normY * terrainSize.z;

                Vector3 worldPos = new Vector3(worldX, 0f, worldZ);

                float dist = Vector3.Distance(worldPos, center);

                // Distort ring so it's not a perfect circle
                float dNoise1 = Mathf.PerlinNoise(worldX * 0.02f, worldZ * 0.02f);
                float dNoise2 = Mathf.PerlinNoise(worldX * 0.07f, worldZ * 0.07f) * 0.5f;
                float distortedDist = dist + (dNoise1 + dNoise2) * 6f;

                float height = 0f;

                if (distortedDist >= inner && distortedDist <= outer) {

                    float mid = (inner + outer) * 0.5f;
                    float halfWidth = (outer - inner) * 0.5f;

                    // distance from peak zone
                    float d = Mathf.Abs(distortedDist - mid);

                    // 1 at center, 0 at edges
                    float t = 1f - Mathf.Clamp01(d / halfWidth);

                    // sharpen peak (important)
                    t = Mathf.Pow(t, 1.6f);

                    // layered noise
                    float n1 = Mathf.PerlinNoise(worldX * 0.01f, worldZ * 0.01f);
                    float n2 = Mathf.PerlinNoise(worldX * 0.04f, worldZ * 0.04f);
                    float n3 = Mathf.PerlinNoise(worldX * 0.12f, worldZ * 0.12f);

                    float combinedNoise = (n1 * 0.6f + n2 * 0.3f + n3 * 0.1f);

                    float baseHeight = t * 0.45f;

                    // noise shapes the mountain mass
                    height = baseHeight * (0.5f + combinedNoise);
                }

                heights[y, x] = height;
            }
        }
        _data.SetHeights(0, 0, heights);

        PaintTerrain();
    }
    public void PaintTerrain() {
        int res = _data.alphamapResolution;
        int layers = _data.alphamapLayers;

        float[,,] map = new float[res, res, layers];

        Vector3 terrainPos = _terrain.transform.position;
        Vector3 terrainSize = _data.size;

        Vector3 gridCenter = _grid.transform.position;

        float halfWidth = _grid.GridSize.x * _grid.CellSize * 0.5f;
        float halfHeight = _grid.GridSize.y * _grid.CellSize * 0.5f;

        float blendWidth = _grid.CellSize * 2f;

        for(int y = 0; y < res; y++) {
            for(int x = 0; x < res; x++) {

                float normX = (float)x / (res - 1);
                float normY = (float)y / (res - 1);

                float worldX = terrainPos.x + normX * terrainSize.x;
                float worldZ = terrainPos.z + normY * terrainSize.z;

                //distance from grid rectangle center;
                float dx = Mathf.Abs(worldX - gridCenter.x);
                float dz = Mathf.Abs(worldZ - gridCenter.z);

                //how far outside the rectangle we currently are
                float distToEdgeX = dx - halfWidth;
                float distToEdgeZ = dz - halfHeight;

                float distToEdge = Mathf.Max(distToEdgeX, distToEdgeZ);

                //slight noise to break perfect edge
                float noise = Mathf.PerlinNoise(worldX * 0.05f, worldZ * 0.05f) * blendWidth * 0.5f;
                distToEdge += noise;
                
                float dirt = 0f;
                float grass = 0f;

                //Inside grid rectangle
                if(distToEdge <= 0f) {
                    dirt = 1f;
                }
                //blend zone
                else if(distToEdge < blendWidth) {
                    float t = distToEdge / blendWidth;
                    dirt = 1f - t;
                    grass = t;
                }
                //completely outside
                else {
                    grass = 1f;
                }

                map[y, x, 0] = dirt;
                map[y, x, 1] = grass;
            }
        }

        _data.SetAlphamaps(0, 0, map);   
    }
}

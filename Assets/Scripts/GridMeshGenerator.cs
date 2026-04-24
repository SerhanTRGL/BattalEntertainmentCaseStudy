using UnityEngine;

public static class GridMeshGenerator
{
    public static Mesh GenerateMesh(Vector2Int size, float cellSize)
    {
        
        int vertexCountX = size.x + 1;
        int vertexCountY = size.y + 1;
        
        var vertices = new Vector3[vertexCountX * vertexCountY];
        var uvs = new Vector2[vertices.Length];
        var triangles = new int[size.x * size.y * 6];
        
        // --- vertices and UVs ---

        int v = 0;
        for(int y = 0; y < vertexCountY; y++)
        {
            for(int x = 0; x < vertexCountX; x++)
            {
                vertices[v] = new Vector3(x * cellSize, 0f, y * cellSize);

                uvs[v] = new Vector2(
                    (float)x/size.x,
                    (float)y/size.y
                );

                v++;
            }
        }

        // --- Triangles ---
        int t = 0;
        for(int y = 0; y < size.y; y++)
        {
            for(int x = 0; x < size.x; x++)
            {
                int i = y * vertexCountX + x;

                //Triangle 1
                triangles[t++] = i;
                triangles[t++] = i + vertexCountX;
                triangles[t++] = i + 1;

                //Triangle 2
                triangles[t++] = i + 1;
                triangles[t++] = i + vertexCountX;
                triangles[t++] = i + vertexCountX + 1;
            }
        }

        // --- Mesh ---
        Mesh mesh = new()
        {
            vertices = vertices,
            uv = uvs,
            triangles = triangles
        };

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        return mesh;
    }
}

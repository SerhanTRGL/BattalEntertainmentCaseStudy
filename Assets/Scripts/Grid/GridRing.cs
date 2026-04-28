using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class GridRing : MonoBehaviour {
    [SerializeField] private Grid grid;
    [SerializeField] private float innerOffset = 2f;
    [SerializeField] private float outerOffset = 5f;

    public Vector3 Center => grid != null ? grid.transform.position : transform.position;

    public float InnerRadius {
        get {
            if (grid == null) return 0f;

            float halfWidth = grid.Size.x * grid.CellSize * 0.5f;
            float halfHeight = grid.Size.y * grid.CellSize * 0.5f;

            float baseRadius = Mathf.Sqrt(halfWidth * halfWidth + halfHeight * halfHeight);
            return baseRadius + grid.CellSize * innerOffset;
        }
    }

    public float OuterRadius => InnerRadius + (grid != null ? grid.CellSize * outerOffset : 0f);

    public Vector3 GetRandomPoint() {
        float angle = Random.value * Mathf.PI * 2f;
        float r = Mathf.Sqrt(Random.Range(InnerRadius * InnerRadius, OuterRadius * OuterRadius));

        float x = Mathf.Cos(angle) * r;
        float z = Mathf.Sin(angle) * r;

        return Center + new Vector3(x, 0f, z);
    }

    private void OnDrawGizmos() {
        if (grid == null) return;
#if UNITY_EDITOR
        Vector3 center = Center;

        Handles.color = new Color(0f, 1f, 0f, 0.1f);
        Handles.DrawSolidDisc(center, Vector3.up, OuterRadius);

        Handles.color = new Color(1f, 0f, 0f, 0.1f);
        Handles.DrawSolidDisc(center, Vector3.up, InnerRadius);
#endif
    }
    private void OnDrawGizmosSelected() {
        if (grid == null) return;
#if UNITY_EDITOR
        Vector3 center = Center;

        Handles.color = new Color(0f, 1f, 0f, 0.5f);
        Handles.DrawSolidDisc(center, Vector3.up, OuterRadius);

        Handles.color = new Color(1f, 0f, 0f, 0.5f);
        Handles.DrawSolidDisc(center, Vector3.up, InnerRadius);
#endif
    }
}
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnables = new();

    void Start()
    {
        var helper = new GridHelper(GetComponent<GridBuilder>());
        for(int x = 0; x < helper.Grid.GridSize.x; x++)
        {
            for(int y = 0; y < helper.Grid.GridSize.y; y++)
            {
                var spawnedObj = Instantiate(spawnables[Random.Range(0, spawnables.Count)]);
                spawnedObj.transform.position = helper.GetCellPosition(new Vector2Int(x, y));
            }
        }
    }
}

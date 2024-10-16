using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    public GameObject gridCellPrefab; 
    public int rows = 5;
    public int cols = 5; 
    public float spacingX = 2.0f;
    public float spacingZ = 2.0f; 

    public Transform[,] gridPositions; 

    void OnEnable()
    {
        gridPositions = new Transform[rows, cols];
        SpawnGrid();
    }

    void SpawnGrid()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Vector3 spawnPosition = new Vector3(i * spacingX, 0, j * spacingZ);

                GameObject gridCell = Instantiate(gridCellPrefab, spawnPosition, Quaternion.identity);
                gridPositions[i, j] = gridCell.transform;
            }
        }
    }

    public Transform[] GetAllSpawnPoints()
    {
        List<Transform> spawnPoints = new List<Transform>();
        foreach (Transform pos in gridPositions)
        {
            spawnPoints.Add(pos);
        }

        return spawnPoints.ToArray();
    }
}

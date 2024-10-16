using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject molePrefab;
    public GameObject hammerPrefab;
    public GameObject vfxPrefab;
    public float moleActiveTime = 1.5f;
    public float spawnDelay = 2.0f;
    public float riseTime = 0.5f;
    public int maxMolesAtOnce = 3;
    public TMP_Text scoreText;

    private List<GameObject> activeMoles = new List<GameObject>();
    private int score = 0;
    private Transform[] spawnPoints;

    private GridSpawner gridSpawner;

    private GameObject hammerInstance;

    void Start()
    {
        gridSpawner = FindObjectOfType<GridSpawner>();
        spawnPoints = gridSpawner.GetAllSpawnPoints();
        score = 0;
        UpdateScore();
        hammerInstance = Instantiate(hammerPrefab);
        hammerInstance.SetActive(false);

        StartCoroutine(SpawnMoles());
    }
    IEnumerator SpawnMoles()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);

            if (activeMoles.Count < maxMolesAtOnce)
            {
                int numMolesToSpawn = Random.Range(1, maxMolesAtOnce - activeMoles.Count + 1);

                for (int i = 0; i < numMolesToSpawn; i++)
                {
                    SpawnRandomMole();
                }
            }
        }
    }
    void SpawnRandomMole()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);

        bool isPositionOccupied = false;
        foreach (GameObject mole in activeMoles)
        {
            if (mole.transform.position == spawnPoints[randomIndex].position)
            {
                isPositionOccupied = true;
                break;
            }
        }

        if (!isPositionOccupied)
        {
            Vector3 spawnPosition = spawnPoints[randomIndex].position;
            Vector3 hiddenPosition = new Vector3(spawnPosition.x, spawnPosition.y - 1.0f, spawnPosition.z);

            GameObject newMole = Instantiate(molePrefab, hiddenPosition, Quaternion.identity);
            activeMoles.Add(newMole);
            StartCoroutine(MoveMoleUp(newMole, spawnPosition, riseTime));
            StartCoroutine(RemoveMoleAfterTime(newMole, moleActiveTime + riseTime));
        }
    }
    IEnumerator MoveMoleUp(GameObject mole, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = mole.transform.position;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            mole.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        mole.transform.position = targetPosition;
    }
    public void MoleHit(GameObject mole)
    {
        if (activeMoles.Contains(mole))
        {
            score++;
            UpdateScore();
            hammerInstance.SetActive(true);
            hammerInstance.GetComponent<HammerController>().ShowHammerAtPosition(mole.transform.position);
            Instantiate(vfxPrefab, mole.transform.position, Quaternion.identity);
            activeMoles.Remove(mole);
            Destroy(mole);
        }
    }

    IEnumerator RemoveMoleAfterTime(GameObject mole, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (activeMoles.Contains(mole))
        {
            activeMoles.Remove(mole);
            Destroy(mole);
        }
    }

    void UpdateScore()
    {
        scoreText.text = score.ToString()+" Points";
    }
}

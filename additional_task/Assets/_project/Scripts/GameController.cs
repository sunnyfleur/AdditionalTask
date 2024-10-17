using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject rabbitPrefab;
    public GameObject hammerPrefab;
    public GameObject vfxPrefab;
    public float rabbitActiveTime = 1.5f;
    public float spawnDelay = 2.0f;
    public float riseTime = 0.5f;
    public int maxRabbitAtOnce = 3;
    public TMP_Text scoreText;

    private List<GameObject> activeRabbits = new List<GameObject>();
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

        StartCoroutine(SpawnRabbit());
    }
    IEnumerator SpawnRabbit()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);

            if (activeRabbits.Count < maxRabbitAtOnce)
            {
                int numRabbitsToSpawn = Random.Range(1, maxRabbitAtOnce - activeRabbits.Count + 1);

                for (int i = 0; i < numRabbitsToSpawn; i++)
                {
                    SpawnRandomRabbit();
                }
            }
        }
    }
    void SpawnRandomRabbit()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);

        bool isPositionOccupied = false;
        foreach (GameObject rabbit in activeRabbits)
        {
            if (rabbit.transform.position == spawnPoints[randomIndex].position)
            {
                isPositionOccupied = true;
                break;
            }
        }

        if (!isPositionOccupied)
        {
            Vector3 spawnPosition = spawnPoints[randomIndex].position;
            Vector3 hiddenPosition = new Vector3(spawnPosition.x, spawnPosition.y - 1.0f, spawnPosition.z);

            GameObject newRabbit = Instantiate(rabbitPrefab, hiddenPosition, Quaternion.identity);
            activeRabbits.Add(newRabbit);
            StartCoroutine(MoveRabbitUp(newRabbit, spawnPosition, riseTime));
            StartCoroutine(RemoveRabbitAfterTime(newRabbit, rabbitActiveTime + riseTime));
        }
    }
    IEnumerator MoveRabbitUp(GameObject rabbit, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = rabbit.transform.position;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            rabbit.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        rabbit.transform.position = targetPosition;
    }
    public void RabbitHit(GameObject rabbit)
    {
        if (activeRabbits.Contains(rabbit))
        {
            score++;
            UpdateScore();
            hammerInstance.SetActive(true);
            hammerInstance.GetComponent<HammerController>().ShowHammerAtPosition(rabbit.transform.position);
            Instantiate(vfxPrefab, rabbit.transform.position, Quaternion.identity);
            activeRabbits.Remove(rabbit);
            Destroy(rabbit);
        }
    }

    IEnumerator RemoveRabbitAfterTime(GameObject rabbit, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (activeRabbits.Contains(rabbit))
        {
            activeRabbits.Remove(rabbit);
            Destroy(rabbit);
        }
    }

    void UpdateScore()
    {
        scoreText.text = score.ToString()+" Points";
    }
}

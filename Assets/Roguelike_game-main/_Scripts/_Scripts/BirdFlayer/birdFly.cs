using System.Collections;
using UnityEngine;

public class BirdFlyAcrossScreen : MonoBehaviour
{
    public float minInterval = 5f;
    public float maxInterval = 15f;
    public float birdSpeed = 5f; 
    public float spawnDistance = 10f; 

    public GameObject birdPrefabLeft;
    public GameObject birdPrefabRight;
    public GameObject birdPrefabDown;

    private Transform playerTransform;

    private void Start()
    {
        StartCoroutine(FindPlayerAndSpawnBirds());
    }

    private IEnumerator FindPlayerAndSpawnBirds()
    {
        while (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerTransform = player.transform;
            yield return null;
        }

        while (true)
        {
            float randomInterval = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(randomInterval);

            SpawnBird();
        }
    }

    private void SpawnBird()
    {
        int direction = Random.Range(0, 3);
        GameObject birdPrefab = GetBirdPrefab(direction);

        Vector3 startPos = GetRandomStartPosition(direction);
        Vector3 endPos = GetEndPosition(startPos, direction);

        GameObject bird = Instantiate(birdPrefab, startPos, Quaternion.identity);

        StartCoroutine(MoveBird(bird, endPos));
    }

    private GameObject GetBirdPrefab(int direction)
    {
        switch (direction)
        {
            case 0: return birdPrefabRight;
            case 1: return birdPrefabLeft;
            case 2: return birdPrefabDown;
            default: return null;
        }
    }

    private Vector3 GetRandomStartPosition(int direction)
    {
        Vector3 playerPos = playerTransform.position;

        float randomXOffset = Random.Range(spawnDistance, spawnDistance + 5f);
        float randomYOffset = Random.Range(spawnDistance / 2f, spawnDistance);

        switch (direction)
        {
            case 0:
                return playerPos + new Vector3(-randomXOffset, Random.Range(-randomYOffset, randomYOffset), 0);
            case 1:
                return playerPos + new Vector3(randomXOffset, Random.Range(-randomYOffset, randomYOffset), 0);
            case 2:
                return playerPos + new Vector3(Random.Range(-randomXOffset, randomXOffset), randomYOffset, 0);
            default:
                return playerPos;
        }
    }

    private Vector3 GetEndPosition(Vector3 startPos, int direction)
    {
        float screenWidth = 20f;
        float screenHeight = 10f;

        switch (direction)
        {
            case 0: return startPos + new Vector3(screenWidth, 0, 0);
            case 1: return startPos + new Vector3(-screenWidth, 0, 0);
            case 2: return startPos + new Vector3(0, -screenHeight, 0);
            default: return startPos;
        }
    }

    private IEnumerator MoveBird(GameObject bird, Vector3 endPos)
    {
        while (Vector3.Distance(bird.transform.position, endPos) > 0.1f)
        {
            bird.transform.position = Vector3.MoveTowards(bird.transform.position, endPos, birdSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(bird);
    }
}
